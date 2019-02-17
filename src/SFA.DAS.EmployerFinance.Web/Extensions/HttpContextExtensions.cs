using Microsoft.AspNetCore.Routing;

namespace SFA.DAS.EmployerFinance.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool TryGetValue<T>(this RouteValueDictionary values, string key, out T value)
        {
            var exists = values.TryGetValue(key, out var obj);

            if (exists)
            {
                value = obj == null ? default : (T)obj;
            }
            else
            {
                value = default;
            }

            return exists;
        }
    }
}