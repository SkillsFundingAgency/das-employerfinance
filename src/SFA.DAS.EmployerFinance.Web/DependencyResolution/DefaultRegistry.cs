using StructureMap;
using SFA.DAS.EmployerFinance.Web.Urls;

namespace SFA.DAS.EmployerFinance.Web.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IEmployerUrls>().Use<EmployerUrls>();

//            Scan(s =>
//            {
//                s.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS"));
//                s.With(new ControllerConvention());
//            });
        }
    }
}