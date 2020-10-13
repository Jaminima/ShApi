namespace ShApi.Backend.Events
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class WebEvent : System.Attribute
    {
        #region Fields

        public string urlPath, method;

        #endregion Fields

        #region Constructors

        public WebEvent(string urlPath, string Method)
        {
            this.urlPath = urlPath.ToLower();
            this.method = Method.ToLower();
        }

        #endregion Constructors

        #region Methods

        public bool Equals(string urlPath, string Method)
        {
            return urlPath == this.urlPath && Method == this.method;
        }

        #endregion Methods
    }
}