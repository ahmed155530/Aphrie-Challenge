using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Extensions
{
    public class RouteDataRequestCultureProvider : RequestCultureProvider
    {
        public int IndexOfCulture;
        public int IndexofUICulture;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            string culture = null;
            string uiCulture = null;

            culture = uiCulture = httpContext.Request.Path.Value.Split('/')[IndexOfCulture]?.ToString();
            //culture = uiCulture = httpContext.Request.Headers.Add();
            var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

            return Task.FromResult(providerResultCulture);
        }
    }
}
