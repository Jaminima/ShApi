using ShApi.Backend.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShApi.Backend.Security
{
    internal class Login
    {
        #region Fields

        public string TokenHash;
        public Data.Objects.User User;

        #endregion Fields

        #region Constructors

        public Login(string Token, User user)
        {
            this.User = user;
            this.TokenHash = Hashing.Hash(Token);
        }

        #endregion Constructors
    }

    public static class LoginTokens
    {
        #region Fields

        private static Random rnd = new Random();
        private static List<Login> Tokens = new List<Login>();

        #endregion Fields

        #region Methods

        private static string GenerateToken(int length = 32)
        {
            string S = "";

            for (int i = 0; i < length; i++) { S += (char)rnd.Next(65, 123); }

            return S.Replace('\\', '/');
        }

        public static bool ContainsUser(User user)
        {
            return Tokens.Count(x => x.User.userName == user.userName) != 0;
        }

        public static string CreateToken(User user)
        {
            string Token = "";

            while (Tokens.Count(x => x.TokenHash == Token) > 0 || Token == "") Token = GenerateToken();

            Tokens.RemoveAll(x => x.User.userName == user.userName);

            Tokens.Add(new Login(Token, user));
            return Token;
        }

        public static User FindUserByToken(string Token)
        {
            return Tokens.First(x => Hashing.Match(x.TokenHash, Token))?.User;
        }

        public static User FindUserByName(string uname)
        {
            return Tokens.First(x => x.User.userName == uname)?.User;
        }

        public static bool IsLoggedIn(string username, string authtoken)
        {
            Login token = Tokens.Find(x => x.User.userName == username);

            return token != null && Hashing.Match(token.User.hashPassword, authtoken);
        }

        public static bool RemoveUser(string username, string authtoken)
        {
            Login token = Tokens.Find(x => x.User.userName == username);

            if (token == null || !Hashing.Match(token.TokenHash, authtoken)) return false;

            Tokens.Remove(token);
            return true;
        }

        #endregion Methods
    }
}