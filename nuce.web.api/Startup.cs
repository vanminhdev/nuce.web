using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Extensions.FileProviders;
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
using nuce.web.api.Services.EduData.Implements;
using nuce.web.api.Services.EduData.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using nuce.web.api.Models.Status;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.Services.Status.Implements;
using System.Net;
using nuce.web.api.Services.Background;
using nuce.web.api.Services.Survey.BackgroundTasks;
using nuce.web.api.Services.Shared;
using nuce.web.api.Services.EduData.BackgroundTasks;
using nuce.web.api.Repositories.EduData.Interfaces;
using nuce.web.api.Repositories.EduData.Implements;

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
            services.AddDbContext<StatusContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NUCE_SURVEY"))
            );
            #endregion

            #region config usermanager
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<NuceCoreIdentityContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region config authentication
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
                                        .SetIsOriginAllowed((host) => true)
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
                    Title = "NUCE API"
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
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            #region core service
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IManagerRestoreService, ManagerRestoreService>();
            services.AddScoped<INewsManagerService, NewsManagerService>();
            services.AddScoped<IClientParameterService, ClientParameterService>();
            services.AddScoped<ISendEmailService, SendEmailService>();
            services.AddScoped<IUserFileService, UserFileService>();
            services.AddScoped<IInitializeService, InitializeService>();
            #endregion
            #region config service
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IAsEduSurveyCauHoiService, AsEduSurveyCauHoiService>();
            services.AddScoped<IAsEduSurveyDapAnService, AsEduSurveyDapAnService>();
            services.AddScoped<IAsEduSurveyDeThiService, AsEduSurveyDeThiService>();
            services.AddScoped<IAsEduSurveyDotKhaoSatService, AsEduSurveyDotKhaoSatService>();
            services.AddScoped<IAsEduSurveyBaiKhaoSatService, AsEduSurveyBaiKhaoSatService>();
            services.AddScoped<IAsEduSurveyBaiKhaoSatSinhVienService, AsEduSurveyBaiKhaoSatSinhVienService>();
            services.AddScoped<BaiKhaoSatSinhVienBackgroundTask>();
            services.AddScoped<IAsEduSurveyReportTotalService, AsEduSurveyReportTotalService>();

            services.AddScoped<IAsEduSurveyGraduateStudentService, AsEduSurveyGraduateStudentService>();
            services.AddScoped<IAsEduSurveyGraduateDotKhaoSatService, AsEduSurveyGraduateDotKhaoSatService>();
            services.AddScoped<IAsEduSurveyGraduateBaiKhaoSatService, AsEduSurveyGraduateBaiKhaoSatService>();
            services.AddScoped<IAsEduSurveyGraduateBaiKhaoSatSinhVienService, AsEduSurveyGraduateBaiKhaoSatSinhVienService>();

            services.AddScoped<IAsEduSurveyUndergraduateStudentService, AsEduSurveyUndergraduateStudentService>();
            services.AddScoped<IAsEduSurveyUndergraduateDotKhaoSatService, AsEduSurveyUndergraduateDotKhaoSatService>();
            services.AddScoped<IAsEduSurveyUndergraduateBaiKhaoSatService, AsEduSurveyUndergraduateBaiKhaoSatService>();
            services.AddScoped<IAsEduSurveyUndergraduateBaiKhaoSatSinhVienService, AsEduSurveyUndergraduateBaiKhaoSatSinhVienService>();

            services.AddScoped<SurveyStatisticBackgroundTask>();
            #endregion
            #region sync edu database service
            services.AddScoped<ISyncEduDatabaseService, SyncEduDatabaseService>();

            services.AddScoped<SyncEduDataBackgroundTask>();
            #endregion
            #region edu data repo
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IFacultyRepository, FacultyRepository>();
            services.AddScoped<ILecturerRepository, LecturerRepository>();
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
            services.AddScoped<ILoaiDichVuRepository, LoaiDichVuRepository>();
            services.AddScoped<IVeXeBusRepository, VeXeBusRepository>();
            services.AddScoped<ICapLaiTheRepository, CapLaiTheRepository>();
            services.AddScoped<IMuonHocBaRepository, MuonHocBaRepository>();

            services.AddScoped<IThamSoDichVuService, ThamSoDichVuService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IDichVuService, DichVuService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IParameterService, ParameterService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            #region background
            services.AddSingleton<BackgroundTaskWorkder>();
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            #endregion

            services.AddScoped<FakerService, FakerService>();
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

            //app.Run(async context =>
            //{
            //    var userStatus = context.Items["UserStatus"];
            //    if (userStatus != null && ((int)userStatus == (int)UserStatus.Deactive || (int)userStatus == (int)UserStatus.Deleted))
            //    {
            //        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //        context.Response.Cookies.Delete(UserParameters.JwtAccessToken);
            //        context.Response.Cookies.Delete(UserParameters.JwtRefreshToken);
            //    }
            //});

            app.UseHttpsRedirection();

            // using Microsoft.Extensions.FileProviders;
            // using System.IO;
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Resources")),
                RequestPath = "/Resources"
            });

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
