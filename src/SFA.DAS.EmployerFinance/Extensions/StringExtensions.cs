using System.Net;

namespace SFA.DAS.EmployerFinance.Extensions
{
    public static class StringExtensions
    {
        public static string HtmlDecode(this string input)
        {
            var output = WebUtility.HtmlDecode(input);

            return output;
        }
    }
}
