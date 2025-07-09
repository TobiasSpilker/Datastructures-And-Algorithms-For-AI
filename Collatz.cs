//Tobias Spilker - Utrecht University

using System;
class Program
{
    static void Main(string[] args)
    {
        //Retrieving the first number (N):
        string FirstInput = Console.ReadLine();
        string ModifiedInput = FirstInput.Split(' ')[0];
        long Amount = long.Parse(ModifiedInput);

        //Creating array to store input numbers:
        long[] Input = new long[Amount];

        //Filling array with input numbers
        for (int i = 0; i < Input.Length; i++)
        {
            //Parsing and changing input into int's:
            string RawInput = Console.ReadLine();
            string ChangedInput = RawInput.Split(' ')[0];
            Input[i] = long.Parse(ChangedInput);
        }

        //Creating array to store output numbers:
        long[] Output = new long[Amount];

        for (int i = 0; i < Input.Length; i++)
        {
            //Puts the input through the collatz algorithm
            //Which in turn returns the collatz length for that specific number
            //And gets stored into the output array
            Output[i] = Collatz(Input[i]);
        }

        for (int i = 0; i < Output.Length; i++)
        {
            Console.WriteLine(Output[i]);
        }

    }

    static long Collatz(long x)
    {
        long CollatzLength = 0;

        while (x != 1)
        {
            CollatzLength += 1;

            //If number is even:
            if (x % 2 == 0)
            {
                x = x / 2;
            }

            //If number is odd:
            else
            {
                x = 3 * x + 1;
            }

        }

        return CollatzLength;

    }
}

/*

2 % het aantal getallen
15 // het eerste getal
22 !!!!! en het tweede


200 % het aantal getallen
15 // het eerste getal
22 !!!!! en het tweede

*/