//Tobias Spilker - Utrecht University

using System;
class Program
{
    static void Main(string[] args)
    //Parsing and storing input data
    {
        #region Catalog parsing
        //Retrieving size of catalog N:
        string StringN = Console.ReadLine();
        string StringN2 = StringN.Split(' ')[0];
        long N_Size = long.Parse(StringN2);

        //Creating array to store catalog numbers:
        long[] Catalog = new long[N_Size + 1];
        Catalog[0] = 0; //Starting of with 0 because you can also start counting from 0

        //Filling array with catalog numbers
        for (int i = 1; i < Catalog.Length; i++)
        {
            //Parsing and changing input into long's:
            string RawInput = Console.ReadLine();
            string ChangedInput = RawInput.Split(' ')[0];
            Catalog[i] = long.Parse(ChangedInput);
        }
        #endregion

        #region Customer parsing

        //Retrieving size of customer order M:
        string StringM = Console.ReadLine();
        string StringM2 = StringM.Split(' ')[0];
        long M_Size = long.Parse(StringM2);

        //Creating array to store customer numbers:
        long[] Customer = new long[M_Size];

        for (int i = 0; i < Customer.Length; i++)
        {
            //Parsing and changing input into long's:
            string RawInput = Console.ReadLine();
            string ChangedInput = RawInput.Split(' ')[0];
            Customer[i] = long.Parse(ChangedInput);
        }

        #endregion

        #region Binary search stuff
        //Creating variable that stores the total amount of chips:
        long TotalChips = 0;

        //Activating BinarySearch:
        for (int i = 0; i < Customer.Length; i++)
        {
            //Console.WriteLine("Current Chip: " + BinarySearch(Catalog, Customer[i]));
            TotalChips += BinarySearch(Catalog, Customer[i]);
        }

        //Final output:
        Console.WriteLine(TotalChips);
        #endregion
    }

    static long BinarySearch(long[] Catalog, long CustomerNumber)
    //Searching over catalog for specific customer and returning minimum chips needed
    {
        long low = 0;                       //Most left-point of array
        long high = Catalog.Length - 1;     //Most right-point of array

        while (low < high) //If they are the same it means there is only 1 item left
                           //or the correct item was found
        {
            //Really weird way to round down to the nearest int (in "long" form):
            long middle_point = (long)Math.Floor((double)(low + high) / 2);

            //If selected value is too low, the LHS can be thrown away
            if (Catalog[middle_point] < CustomerNumber)
            {
                low = middle_point + 1;
            }
            //If selected value is too high. the RHS can be thrown away
            else
            {
                high = middle_point;
            }

        }

        //X - A: X is always the bigger one and we want to know how many chips we still need to add:
        long Chips_Needed = CustomerNumber - Catalog[low];

        //If its negative its clearly taken a too high catalog number:
        if (Chips_Needed < 0)
        {
            Chips_Needed = CustomerNumber - Catalog[low - 1];
        }
        else
        {
            //nothing, keep it like this
        }

        //Returns the needed amount of chips for the specific cusotmer:
        return Chips_Needed;
    }

}
/*

3 // Omvang catalogus
13 // Kleinste standaardblok
18 // Elk blok is max 999999999
23 // De catalogus is gesorteerd

4 // Aantal te produceren steunen
18 // Blok van 18 past precies
25 // Blok van 23 plus twee schijfjes
5 // Alleen vijf schijfjes
21 // Gebruik nog een blok van 18, met drie schijfje


//////////////////////////////////////


0 // Omvang catalogus
4 // Aantal te produceren steunen
18 // Blok van 18 past precies
25 // Blok van 23 plus twee schijfjes
5 // Alleen vijf schijfjes
21 // Gebruik nog een blok van 18, met drie schijfje

*/