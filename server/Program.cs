﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_4
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.SingletonServer(5002, 10);
        }
    }
}
