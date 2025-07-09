//Tobias Spilker - Utrecht University
using System;
using System.Collections.Generic;

namespace Bouwmarkt
{
    class Program
    {
        static void Main(string[] args)
        {
            //Eerste regel lezen en opslaan in variabelen:
            string firstline = Console.ReadLine();
            string[] firstline_vars = firstline.Split(' ');
            int K = Convert.ToInt32(firstline_vars[0]);
            int N = Convert.ToInt32(firstline_vars[1]);
            string Modus = firstline_vars[3];

            //
        }
    }
}