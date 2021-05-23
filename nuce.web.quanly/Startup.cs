
using CKSource.CKFinder.Connector.Config;
using CKSource.CKFinder.Connector.Core;
using CKSource.CKFinder.Connector.Core.Acl;
using CKSource.CKFinder.Connector.Core.Builders;
using CKSource.CKFinder.Connector.Core.Events;
using CKSource.CKFinder.Connector.Core.Logs;
using CKSource.CKFinder.Connector.Host.Owin;

using CKSource.FileSystem.Local;
using Microsoft.Owin;
using nuce.web.quanly.CKFinder;
using nuce.web.quanly.Common;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

[assembly: OwinStartupAttribute(typeof(MVCIntegrationExample.Startup))]
namespace MVCIntegrationExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            FileSystemFactory.RegisterFileSystem<LocalStorage>();
            app.Map("/ckfinder/connector", SetupConnector);
        }

        private static void SetupConnector(IAppBuilder app)
        {
            /*
             * Create a connector instance using ConnectorBuilder. The call to the LoadConfig() method
             * will configure the connector using CKFinder configuration options defined in Web.config.
             */
            var connectorFactory = new OwinConnectorFactory();
            var connectorBuilder = new ConnectorBuilder();

            /*
             * Create an instance of authenticator implemented in the previous step.
             */
            var customAuthenticator = new CustomCKFinderAuthenticator();

            connectorBuilder
                /*
                 * Provide the global configuration.
                 *
                 */
                .LoadConfig()
                .SetAuthenticator(customAuthenticator)
                .SetRequestConfiguration(
                    (request, config) =>
                    {
                        config.LoadConfig();
                        /*
                         * Configure settings per request.
                         *
                         * The minimal configuration has to include at least one backend, one resource type
                         * and one ACL rule.
                         *
                         * For example:
                         */
                        string folderName = "";
                        if (request.Cookies.ContainsKey(UserParameters.JwtAccessToken))
                        {
                            var accessToken = "";
                            request.Cookies.TryGetValue(UserParameters.JwtAccessToken, out accessToken);
                            var handler = new JwtSecurityTokenHandler();
                            var jwtSecurityToken = handler.ReadJwtToken(accessToken);
                            var username = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                            folderName = username;
                        }
                        //try
                        //{
                        //    config.RemoveBackend("default");
                        //}
                        //catch (System.Exception)
                        //{

                        //}
                        //config.AddBackend("default", new LocalStorage(@"D:/Code/NUCE/code/nuce.web.resources"));
                        config.AddResourceType("images", builder => builder.SetBackend("default", folderName));
                        config.AddAclRule(new AclRule(
                            new StringMatcher("*"),
                            new StringMatcher("*"),
                            new StringMatcher("*"),
                            new Dictionary<Permission, PermissionType> { { Permission.All, PermissionType.Allow } }));
                    }
                );

            /*
             * Build the connector middleware.
             */
            var connector = connectorBuilder
                .Build(connectorFactory);

            /*
             * Add the CKFinder connector middleware to the web application pipeline.
             */
            app.UseConnector(connector);
        }
    }
}