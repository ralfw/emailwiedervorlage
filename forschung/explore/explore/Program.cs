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
        static void Main()
        {
            f(1).Cont(g);

            h(4).Cont(f).Cont(g);
        }

        static string f(int a)
        {
            return (a + 1).ToString();
        }

        static void g(string t)
        {
            Console.WriteLine(t);
        }

        static int h(int a)
        {
            return a*10;
        }



        static void Main_GetHostEntry()
        {
            System.Net.IPHostEntry i = System.Net.Dns.GetHostEntry("www.google.com");
            // internetverbindung, solange keine exception geworfen
        }


        static void Main_Ping(string[] args)
        {
            var p = new Ping();
            var result = p.Send("ralfw.de");
            Console.WriteLine(result.Status);
            // success, wenn es geklappt hat. sonst ein anderes resultat oder exception.
        }
    }


    static class FlowExtensions
    {
        public static void Cont<T>(this T input, Action<T> continuation)
        {
            continuation(input);
        }

        public static U Cont<T, U>(this T input, Func<T, U> continuation)
        {
            return continuation(input);
        }
    }
}
