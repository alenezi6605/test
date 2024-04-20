using TestsService.Infrastructure.Configuration;
using System.Globalization;

namespace TestsService.API.Utility
{

    public class CultureProviderMiddleware
    {
        private readonly DatabaseContext _context;
        private readonly RequestDelegate _next;

        public CultureProviderMiddleware(RequestDelegate next, DatabaseContext context)
        {
            _next = next;

            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //TODO:: get languages from database
            List<string> supportedCientLanguages = new List<string>//TODO:: replace with DB
                {
                    "en-US",
                    "ar-KW",
                };

            //TODO:: get default language from all languages
            string defaultLanguage = supportedCientLanguages.First();//TODO:: replace from DB

            //get language preferred by client
            var clientLanguage = httpContext.Request.Headers["Accept-Language"].ToString();

            //If client is not prefering any language then set default language
            if (clientLanguage == null || string.IsNullOrEmpty(clientLanguage))
            {
                //let client know that he is getting which language content
                httpContext.Response.Headers.ContentLanguage = defaultLanguage;

                //set language culture globally
                Thread.CurrentThread.CurrentCulture = new CultureInfo(defaultLanguage);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(defaultLanguage);

                await _next(httpContext).ConfigureAwait(true);

                return;
            }

            //check if client language is valid, if its not valid then again set default language
            if (!supportedCientLanguages.Contains(clientLanguage))
            {
                //let client know that is getting which language content
                httpContext.Response.Headers.ContentLanguage = defaultLanguage;

                //set language culture globally
                Thread.CurrentThread.CurrentCulture = new CultureInfo(defaultLanguage);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(defaultLanguage);

                await _next(httpContext).ConfigureAwait(false);

                return;
            }

            //set client selected language culture globally
            Thread.CurrentThread.CurrentCulture = new CultureInfo(clientLanguage);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(clientLanguage);

            await _next(httpContext).ConfigureAwait(false);

        }
    }

}
