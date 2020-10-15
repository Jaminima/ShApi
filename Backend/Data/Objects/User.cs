namespace ShApi.Backend.Data.Objects
{
    public class User
    {
        #region Fields

        public string userName { get; set; }
        public string hashPassword { get; set; }
        public int Id { get; set; }

        #endregion Fields

        #region Constructors

        public User()
        {
        }

        public User(string uName, string password)
        {
            this.userName = uName;
            this.hashPassword = Security.Hashing.Hash(password);
        }

        #endregion Constructors

        #region Methods

        public static User Find(string username)
        {
            return MemoryHandler.Users.FindOne(x => x.userName == username);
        }

        #endregion Methods
    }
}