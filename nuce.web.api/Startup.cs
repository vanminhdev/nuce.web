using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using nuce.web.api.Common;
using nuce.web.api.Models.Core;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Survey;
using nuce.web.api.Repositories.Ctsv.Implements;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Core.Implements;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Ctsv.Implements;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.Services.Survey.Implements;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.Services.Synchronization.Implements;
using nuce.web.api.Services.Synchronization.Interfaces;

namespace nuce.web.api
{
    public class Startup
    {
        readonly string _allowSpecificOrigins = "_allowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region config db
            services.AddDbContext<NuceCoreIdentityContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NUCE_CORE"))
            );
            services.AddDbContext<SurveyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NUCE_SURVEY"))
            );
            services.AddDbContext<CTSVNUCE_DATAContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NUCE_CTSV"))
            );
            services.AddDbContext<EduDataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NUCE_SURVEY"))
            );
            #endregion

            #region config usermanager
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<NuceCoreIdentityContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                    ClockSkew = TimeSpan.FromDays(1) //expiration token
                };
                //get jwt token in httponly cookies
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[UserParameters.JwtAccessToken];
                        return Task.CompletedTask;
                    }
                };
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
            #endregion

            #region config cors
            //config danh sách url allow cors
            var corsUrlsSection = Configuration.GetSection("CorsUrl");
            var corsUrls = corsUrlsSection
                .AsEnumerable()
                .Where(item => item.Value != null)
                .Select(item => item.Value)
                .ToArray();
            services.AddCors(options =>
            {
                options.AddPolicy(name: _allowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins(corsUrls)
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials();
                                  });
            });
            #endregion

            #region config swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API V1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            #endregion

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddHttpContextAccessor();
            services.AddSingleton<IPathProvider, PathProvider>();

            #region config service
            services.AddScoped<IAsEduSurveyCauHoiService, AsEduSurveyCauHoiService>();
            services.AddScoped<IAsEduSurveyDapAnService, AsEduSurveyDapAnService>();
            services.AddScoped<IAsEduSurveyDeThiService, AsEduSurveyDeThiService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILogService, LogService>();
            #endregion
            #region sync edu database service
            services.AddScoped<ISyncEduDatabaseService, SyncEduDatabaseService>();
            #endregion
            #region ctsv service
            services.AddScoped<IXacNhanRepository, XacNhanRepository>();
            services.AddScoped<IGioiThieuRepository, GioiThieuRepository>();
            services.AddScoped<IUuDaiGiaoDucRepository, UuDaiGiaoDucRepository>();
            services.AddScoped<IVayVonRepository, VayVonRepository>();
            services.AddScoped<IThueNhaRepository, ThueNhaRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<INewsItemsRepository, NewsItemsRepository>();
            services.AddScoped<ITinNhanRepository, TinNhanRepository>();
            services.AddScoped<IGiaDinhRepository, GiaDinhRepository>();
            services.AddScoped<IThiHsgRepository, ThiHsgRepository>();
            services.AddScoped<IQuaTrinhHocRepository, QuaTrinhHocRepository>();


            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IDichVuService, DichVuService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IParameterService, ParameterService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nuce API V1");
                    c.RoutePrefix = "swagger";
                });
            }
            var loggingOptions = this.Configuration.GetSection("Log4NetCore").Get<Log4NetProviderOptions>();
            loggerFactory.AddLog4Net(loggingOptions);
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(_allowSpecificOrigins);
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
