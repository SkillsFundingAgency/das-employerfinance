using Microsoft.Extensions.FileProviders;

namespace SFA.DAS.EmployerFinance.Configuration
{
    public class HostingEnvironmentAdapter : Microsoft.Extensions.Hosting.IHostingEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }

        public HostingEnvironmentAdapter(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            EnvironmentName = hostingEnvironment.EnvironmentName;
            ApplicationName = hostingEnvironment.ApplicationName;
            ContentRootPath = hostingEnvironment.ContentRootPath;
            ContentRootFileProvider = hostingEnvironment.ContentRootFileProvider;
        }
    }
}