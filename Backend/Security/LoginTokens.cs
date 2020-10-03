using Stockr.Backend.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace Stockr.Backend.Security
{
    class Login
    {
        public string TokenHash;
        public Data.Objects.User User;

        public Login(string Token, User user)
        {
            this.User = user;
            this.TokenHash = Hashing.Hash(Token);
        }
    }

    public static class LoginTokens
    {
        static Random rnd = new Random();
        static List<Login> Tokens = new List<Login>();

        public static bool ContainsUser(User user)
        {
            return Tokens.Count(x => x.User.userName == user.userName) != 0;
        }

        public static bool IsLoggedIn(string username, string authtoken)
        {
            Login token = Tokens.Find(x => x.User.userName == username);

            return token!=null && Hashing.Match(token.User.hashPassword, authtoken);
        }

        public static User FindUser(string Token)
        {
            return Tokens.First(x => Hashing.Match(x.TokenHash, Token))?.User;
        }

        public static bool RemoveUser(string username, string authtoken)
        {
            Login token = Tokens.Find(x => x.User.userName == username);

            if (token==null || !Hashing.Match(token.TokenHash, authtoken)) return false;

            Tokens.Remove(token);
            return true;
        }

        public static string CreateToken(User user)
        {
            string Token="";

            while (Tokens.Count(x => x.TokenHash == Token) > 0 || Token == "") Token = GenerateToken();

            Tokens.RemoveAll(x => x.User.userName == user.userName);

            Tokens.Add(new Login(Token, user));
            return Token;
        }

        static string GenerateToken(int length=32)
        {
            string S = "";

            for (int i = 0; i < length; i++) { S += (char)rnd.Next(65, 123); }

            return S.Replace('\\','/');
        }
    }
}
