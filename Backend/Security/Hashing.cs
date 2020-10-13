using Scrypt;

namespace ShApi.Backend.Security
{
    public static class Hashing
    {
        #region Fields

        private static ScryptEncoder encoder = new ScryptEncoder();

        #endregion Fields

        #region Methods

        public static string Hash(string Raw)
        {
            return encoder.Encode(Raw);
        }

        public static bool Match(string Hash, string Raw)
        {
            return encoder.Compare(Raw, Hash);
        }

        #endregion Methods
    }
}