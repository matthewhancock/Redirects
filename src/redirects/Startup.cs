using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Runtime;
using Newtonsoft.Json;

namespace redirects {
    public class Startup {
        List<SiteConfig> sites;
        public void Configure(IApplicationBuilder app, IApplicationEnvironment env) {
            sites = JsonConvert.DeserializeObject<List<SiteConfig>>(Util.File.LoadToString(env.ApplicationBasePath, "sites.json"));
            app.Run(Redirect);
        }

        private async Task Redirect(HttpContext context) {
            var domainName = context.Request.Host.ToString();
            if (domainName.StartsWith("www.")) {
                domainName = domainName.Substring(4);
            }
            var config = sites.Find((c) => c.DomainName == domainName);
            if (config != null) {
                var url = config.IncludePath ? config.DomainName + context.Request.Path.ToString() : config.DomainName;
                context.Response.Redirect(url, config.Permanent);
            } else {
                await context.Response.WriteAsync("Invalid Domain");
            }
        }
    }
    public class SiteConfig {
        private string _domainname, _url;
        private bool _includePath, _permanent;

        public SiteConfig(string DomainName, bool IncludePath, bool Permanent, string Url) {
            _domainname = DomainName;
            _includePath = IncludePath;
            _permanent = Permanent;
            _url = Url;
        }
        public string DomainName { get { return _domainname; } }
        public bool IncludePath { get { return _includePath; } }
        public bool Permanent { get { return _permanent; } }
        public string Url { get { return _url; } }
    }
}
