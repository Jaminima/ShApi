using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockr.Backend.Events
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class WebEvent : System.Attribute
    {
        public string urlPath, method;

        public WebEvent(string urlPath, string Method)
        {
            this.urlPath = urlPath.ToLower();
            this.method = Method.ToLower();
        }

        public bool Equals(string urlPath, string Method)
        {
            return urlPath == this.urlPath && Method == this.method;
        }
    }
}
