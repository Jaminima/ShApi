using System.Collections.Generic;
using System.Linq;

namespace ShApi.Backend.Data.Objects
{
    public class User
    {
        #region Fields

        public static List<User> Users = new List<Objects.User>();

        public string userName, hashPassword;

        #endregion Fields

        #region Constructors

        public User(string uName, string password)
        {
            this.userName = uName;
            this.hashPassword = Security.Hashing.Hash(password);
        }

        #endregion Constructors

        #region Methods

        public static User Find(string username)
        {
            Objects.User[] u = Users.Where(x => x.userName == username).ToArray();
            if (u.Length == 0) return null;
            return u[0];
        }

        #endregion Methods
    }
}