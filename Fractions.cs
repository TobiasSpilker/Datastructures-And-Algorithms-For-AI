//Tobias Spilker - Utrecht University
using System;
using System.Collections.Generic;

namespace Breuk
{
    class Program
    {

        static void Main(string[] args)
        {
            //Reading beginning number:
            string RawAmount = Console.ReadLine();
            string[] amountR = RawAmount.Split(' ');
            int amount = int.Parse(amountR[0]);

            //Breuk object array:
            BreukObjectje[] breukObjectjes = new BreukObjectje[amount];

            //Looping door de input en maak breuk objecten aan en stop deze in de array
            for (int i = 0; i < amount; i++)
            {
                #region Inlezen
                //De ruwe input lezen:
                string RawInput = Console.ReadLine();

                //De ruwe input opslitsen in breuk en overige tekst:
                string[] BreukInput = RawInput.Split(' ');

                //De breuk extraheren als string:
                string BreukString = BreukInput[0];

                //De breuk opslitsen in teller en noemer:
                string[] GesplitsteBreukString = BreukString.Split('/');

                //Extraheer teller en noemer als ints:
                int Teller = int.Parse(GesplitsteBreukString[0]);
                int Noemer = int.Parse(GesplitsteBreukString[1]);

                //Voeg deze toe aan het breuk object array:
                breukObjectjes[i] = new BreukObjectje(Teller, Noemer);
                #endregion
            }

            //Lekker al het sorteer werk:
            BreukObjectje[] breukObjectjes_teller = MergeSort(breukObjectjes, "teller");
            BreukObjectje[] breukObjectjes_noemer = MergeSort(breukObjectjes, "noemer");
            BreukObjectje[] breukObjectjes_waarde = MergeSort(breukObjectjes, "waarde");

            #region Output
            Console.WriteLine(amount);

            for (int i = 0; i < amount; i++)
            {
                Console.WriteLine(
                    breukObjectjes_teller[i].numerator.ToString() + "/" + breukObjectjes_teller[i].denominator.ToString()
                    + " " +
                    breukObjectjes_noemer[i].numerator.ToString() + "/" + breukObjectjes_noemer[i].denominator.ToString()
                    + " " +
                    breukObjectjes_waarde[i].numerator.ToString() + "/" + breukObjectjes_waarde[i].denominator.ToString());
            }
            #endregion
        }

        public static BreukObjectje[] MergeSort(BreukObjectje[] array, string type)
        //Standaard implementatie van mergesort :)
        {
            if (array.Length <= 1)
                return array;

            int mid = array.Length / 2;
            BreukObjectje[] left = new BreukObjectje[mid];
            BreukObjectje[] right = new BreukObjectje[array.Length - mid];

            Array.Copy(array, 0, left, 0, mid);
            Array.Copy(array, mid, right, 0, array.Length - mid);

            BreukObjectje[] sortedLeft = MergeSort(left, type);
            BreukObjectje[] sortedRight = MergeSort(right, type);

            return Merge(sortedLeft, sortedRight, type);
        }

        private static BreukObjectje[] Merge(BreukObjectje[] left, BreukObjectje[] right, string type)
        //Standaard implementatie van mergesort :)
        {
            BreukObjectje[] result = new BreukObjectje[left.Length + right.Length];
            int i = 0, j = 0, k = 0;

            while (i < left.Length && j < right.Length)
            {

                //Sorteer op basis van teller klein naar groot:
                if (type == "teller")
                {
                    if (left[i].numerator <= right[j].numerator)
                        result[k++] = left[i++];
                    else
                        result[k++] = right[j++];
                }

                //Sorteer op basis van noemer groot naar klein:
                else if (type == "noemer")
                {
                    if (left[i].denominator >= right[j].denominator)
                        result[k++] = left[i++];
                    else
                        result[k++] = right[j++];
                }

                //Sorteer op bais van echte waarde vd breuk:
                else if (type == "waarde")
                {
                    long leftValue = left[i].numerator * right[j].denominator;
                    long rightValue = right[j].numerator * left[i].denominator;

                    if (leftValue <= rightValue)
                        result[k++] = left[i++];
                    else
                        result[k++] = right[j++];
                }
            }

            while (i < left.Length)
                result[k++] = left[i++];

            while (j < right.Length)
                result[k++] = right[j++];

            return result;
        }



    }
}


/*

////INPUT/////
4 o hier staat iets achter
1/4 kwart
22/7 pi hi hi
9/1 geheel
5/2

////OUTPUT/////
4
1/4 22/7 1/4
5/2 1/4 5/2
9/1 5/2 22/7
22/7 9/1 9/1


*/


//BREUK OBJECT:
namespace Breuk
{

    internal class BreukObjectje
    {
        #region Fractions Object
        public long numerator;
        public long denominator;

        public BreukObjectje(long Numerator, long Denominator)
        {
            this.numerator = Numerator;
            this.denominator = Denominator;
        }

        public int CompareTo(object other2)
        {
            BreukObjectje other = other2 as BreukObjectje;
            //Doing it like this because i am not allowed to devide >:(
            long thisValue = this.numerator * other.denominator;
            long otherValue = other.numerator * this.denominator;

            if (thisValue < otherValue)
            {
                return 1;
            }

            else if (thisValue > otherValue)
            {
                return -1;
            }

            else //This is for when the values are the same
            {
                //Evaluating them by denimantor size:
                if (this.denominator < other.denominator)
                {
                    return 1;
                }

                else if (this.denominator > other.denominator)
                {
                    return -1;
                }

                else
                {
                    return 0;
                }
            }
        }

        #endregion
    }
}