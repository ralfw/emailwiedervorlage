using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace explore
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Ping();
            var result = p.Send("ralfw.de");
            Console.WriteLine(result.Status);
            // success, wenn es geklappt hat. sonst ein anderes resultat oder exception.
        }
    }
}
