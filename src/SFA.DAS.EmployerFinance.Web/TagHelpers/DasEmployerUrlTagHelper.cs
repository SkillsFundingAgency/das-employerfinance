using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.EmployerFinance.Web.Urls;

namespace SFA.DAS.EmployerFinance.Web.TagHelpers
{
    [HtmlTargetElement("a", Attributes = EmployerUrlAttributeName)]
//    [HtmlTargetElement("a", Attributes = EmployerUrlPrefix + "*")]
    public class DasAnchorTagHelper : TagHelper
    {
        private const string EmployerUrlAttributeName = "das-employer-url";
//        private const string EmployerUrlPrefix = "das-employer-url-";
        private const string Href = "href";

        public DasAnchorTagHelper(IEmployerUrls employerUrls)
        {
            _employerUrls = employerUrls;
        }

        private IEmployerUrls _employerUrls;
        
        [HtmlAttributeName(EmployerUrlAttributeName)]
        public string EmployerUrl { get; set; }
        
        /// <remarks>Does nothing if user provides an <c>href</c> attribute.</remarks>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            
            // If "href" is already set, it means the user is attempting to use a normal anchor.
//            if (output.Attributes.ContainsName(Href))
//            {
//                if (EmployerUrl != null)
//                {
//                    //todo: interaction with AnchorTagHelper
//                    throw new InvalidOperationException($"Cannot override the '{Href}' attribute for <a>. Both {EmployerUrlAttributeName} and '{Href}' are present.");
//                }
//
//                return;
//            }
            
            //todo: not sure if this is the way to go, e.g. lose intellisense
            // for urls accepting accountHashedId, could have separate attribute or we could pick it up automatically
            var employerUrlMethod = typeof(EmployerUrls).GetMethod(EmployerUrl);
            var url = employerUrlMethod.Invoke(_employerUrls, null);

            output.Attributes.SetAttribute(Href, url);
        }
    }
}