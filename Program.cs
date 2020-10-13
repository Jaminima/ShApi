using System;

namespace ShApi
{
    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            Backend.Endpoints.Listener.Start();
            Console.ReadLine();
        }

        #endregion Methods
    }
}