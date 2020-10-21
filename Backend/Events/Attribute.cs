namespace ShApi.Backend.Events
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class WebEvent : System.Attribute
    {
        #region Fields

        public string urlPath, method;
        public bool WebSocket = false;

        #endregion Fields

        #region Constructors

        public WebEvent(string urlPath, string Method)
        {
            this.urlPath = urlPath.ToLower();
            this.method = Method.ToLower();
        }

        public WebEvent(string urlPath, string Method, bool WebSocket = false)
        {
            this.urlPath = urlPath.ToLower();
            this.method = Method.ToLower();
            this.WebSocket = WebSocket;
        }

        #endregion Constructors

        #region Methods

        public bool Equals(string urlPath, string Method, bool WebSocket = false)
        {
            return urlPath.ToLower() == this.urlPath && Method.ToLower() == this.method && WebSocket == this.WebSocket;
        }

        #endregion Methods
    }
}