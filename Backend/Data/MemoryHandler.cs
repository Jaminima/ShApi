﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Stockr.Backend.Data
{
    public static class MemoryHandler
    {
        public static Objects.User[] Users = new Objects.User[] { new Objects.User("Jaminima","William") };
    }
}