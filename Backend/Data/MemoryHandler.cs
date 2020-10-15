using LiteDB;
using ShApi.Backend.Data.Objects;

namespace ShApi.Backend.Data
{
    public static class MemoryHandler
    {
        public static LiteDatabase db = new LiteDatabase("./Dataset.db");

        public static ILiteCollection<User> Users = db.GetCollection<User>("Users");

        public static void Setup()
        {
            Users.EnsureIndex(x => x.userName, true);
        }
    }
}