using System;
using System.Collections.Generic;
using System.Linq;

class Bestelbus
{
    static void Main()
    //processes the data and calls both methods
    {
        #region first input handling
        //Reads the 1st console input
        string Line_1_Input = Console.ReadLine().Trim();
        //and splices this into two different numbers
        string[] Spliced_Line_1_Input = Line_1_Input.Split(' ');
        //and stores this in 2 different variables
        int Aantal_Pakjes = int.Parse(Spliced_Line_1_Input[0]);
        int Aantal_Ritten = int.Parse(Spliced_Line_1_Input[1]);
        #endregion

        //Creates the list to be filled with all the packages (their weights)
        List<long> Pakjes = new List<long>();

        //Reads all the following input and stores this in the list
        #region while loop
        while (true)
        {
            string Input = Console.ReadLine().Trim();

            foreach (string pakje in Input.Split(' '))
            {
                Pakjes.Add(long.Parse(pakje));
            }

            if (Pakjes.Count == Aantal_Pakjes)
            {
                break;
            }
        }
        #endregion

        //Calls the next method
        BinairB(Pakjes, Aantal_Ritten);
    }

    static void BinairB(List<long> Pakjes, int Aantal_Ritten)
    //Calculates begin value of B, Calls RijtjesMaker method, uses binary sort to find lowest possible B
    {
        #region calculate begin value of B
        //minimum value of B should include the highest package weight because it needs to be atleast this
        long B_min = Pakjes.Max();
        //the maximum obviously never exceeds the total sum of the packages
        long B_max = Pakjes.Sum();
        //takes the middle value of B
        long B_Begin = (B_max + B_min) / 2;
        #endregion

        #region Binary search on B
        //Minimal amount of capacity needed:
        long B = B_Begin;

        while(true)
        {
            //Taking the B(-1) through the RijtjesMaker algorithm
            long Possible_R = RijtjesMaker(B, Pakjes);
            long Possible_R_1 = RijtjesMaker(B - 1, Pakjes);

            //checking if condition is satisfied (if optimal solution was found)
            bool Satisfied = Possible_R == Aantal_Ritten && Possible_R_1 > Aantal_Ritten;

            if (Satisfied || B_min == B)
            {
                break ;
            }

            if (Possible_R <= Aantal_Ritten)
            //if the possible amount of "ritten" is smaller or equal, B can shrink. We want to be biased towards
            //decreasing B.
            {
                double Floaty2 = 2;
                double temp = (B_min + B) / Floaty2;
                B_max = B;
                B = Convert.ToInt64(Math.Floor(temp));
            }

            else if (Possible_R > Aantal_Ritten)
            //If it is not doable with the given B, B should increase so Possible_R decreases
            {
                double Floaty2 = 2;
                double temp = (B + B_max) / Floaty2;
                B_min = B;
                B = Convert.ToInt64(Math.Ceiling(temp));
            }
        }

        Console.WriteLine(B);

        #endregion
    }

    static long RijtjesMaker(long B, List<long> Pakjes)
    {
        #region Calculate Possible_R given our B and packages
        //Creates the list to store the possible "ritten"
        List<long> MogelijkeRitten = new List<long>();

        //Temporary sum to keep track of weight per "rit"
        long Temp_Sum = 0;

        for (int i = 0; i < Pakjes.Count; ++i)
        {

            //If our current sum is still low enough in the next iteration it is okay
            if (Temp_Sum + Pakjes[i] <= B)
            {
                Temp_Sum += Pakjes[i];
            }

            //Else it will get stored in our new list and be set to 0 again
            else 
            {
                MogelijkeRitten.Add(Temp_Sum);
                Temp_Sum = Pakjes[i];
            }

            //It will also store it in the list if its the last number (i didnt know how else to do it haha)
            if (i == Pakjes.Count - 1)
            {
                MogelijkeRitten.Add(Pakjes[i]);
            }

        }
        long Possible_R = MogelijkeRitten.Count();
        #endregion

        return Possible_R;
    }
}