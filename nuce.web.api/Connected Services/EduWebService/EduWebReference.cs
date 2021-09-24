using Microsoft.Extensions.Configuration;

namespace EduWebService
{
    public partial class ServiceSoapClient
    {
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials)
        {
            var endpoint = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ServiceEdu");
            
            serviceEndpoint.Address =
                new System.ServiceModel.EndpointAddress(new System.Uri(endpoint.Value),
                new System.ServiceModel.DnsEndpointIdentity(""));
        }
    }
}
