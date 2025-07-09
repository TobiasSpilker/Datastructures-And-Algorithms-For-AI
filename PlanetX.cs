//Tobias Spilker - Universiteit Utrecht

using System;

class Program
{
    static void Main()
    {
        long NumP = Convert.ToInt64(Console.ReadLine());

        long[] Data = new long[NumP];

        for (long i = 0; i < NumP; i++)
        {

            string[] spliced = Console.ReadLine().Split(' ');

            long Num1 = Convert.ToInt64(spliced[0]);
            long Num2 = Convert.ToInt64(spliced[1]);
            //split into different numbers and add them together

            Data[i] = Num1 + Num2;
        }

        foreach (long data in Data)
        {
            Console.WriteLine(data);
        }

    }
}