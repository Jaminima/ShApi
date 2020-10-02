using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrypt;

namespace Stockr.Backend.Security
{
    public static class Hashing
    {
        static ScryptEncoder encoder = new ScryptEncoder();
        public static bool Match(string Hash, string Raw)
        {
            return encoder.Compare(Raw, Hash);
        }

        public static string Hash(string Raw)
        {
            return encoder.Encode(Raw);
        }
    }
}
