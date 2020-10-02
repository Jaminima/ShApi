using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockr
{
    class Program
    {
        static void Main(string[] args)
        {
            Backend.Endpoints.Listener.Start();
            Console.ReadLine();
        }
    }
}
