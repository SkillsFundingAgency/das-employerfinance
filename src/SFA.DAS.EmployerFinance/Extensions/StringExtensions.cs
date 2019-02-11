using System.IO;
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

        public static string HtmlEncode(this string input)
        {
            var output = WebUtility.HtmlEncode(input);

            return output;
        }
        
        /// <returns>Stream that contains the supplied source string. The caller is responsible for disposing the stream.</returns>
        public static Stream ToStream(this string source)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            
            writer.Write(source);
            writer.Flush();
            stream.Position = 0;
            
            return stream;
        }
    }
}
