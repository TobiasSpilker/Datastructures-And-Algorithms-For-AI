//Tobias Spilker - Utrecht University
using System;
using System.Collections.Generic;

namespace Skyline
{
    class Program
    {

        static void Main(string[] args)
        {
            #region Parsing and storing input data into object
            //Reading the N amount of buildings:
            long Amount_N = Int64.Parse(Console.ReadLine().Split(' ')[0]);

            //Creating the object:
            IComparable[] Buildings = new IComparable[Amount_N];

            for (long i = 0; i < Amount_N; i++)
            {
                //Read the entire line, to be spliced later on
                string[] CurrentInputString = Console.ReadLine().Split(' ');

                //Filling variables with their values:
                long a = Int64.Parse(CurrentInputString[0]);
                long b = Int64.Parse(CurrentInputString[1]);
                long c = Int64.Parse(CurrentInputString[2]);

                //Filling the object array with the 3-tupples:
                Buildings[i] = new ComparableBuilding(a, b, c);
            }
            #endregion

            #region Calling methods
            //Sorting the buildings from most left to most right
            //(looking at the left X coordinate of the building)
            MergeSortBuildings(Buildings, 0, Amount_N - 1);

            //Change all these buildings into silhouets:
            List<IComparable> Silhouets = SilhouetMaker(Buildings);

            //Sorting the silhouetes from low to high???
            MergeSortSilhouets(Silhouets, 0, Silhouets.Count - 1);

            for (int i = 0; i < Silhouets.Count; i++)
            {
                ComparableSilhouet silhouet = Silhouets[i] as ComparableSilhouet;

                Console.WriteLine("h: " + silhouet.h);
            }


            #endregion

            #region Output gebeuren
            //ComparableBuilding other = Buildings[i] as ComparableBuilding;
            #endregion
        }

        static void MergeSortBuildings(IComparable[] Buildings, long p, long r)
        {
            #region Body
            if (p < r)
            {
                long q = (long)Math.Floor((double)(p + r) / 2);
                MergeSortBuildings(Buildings, p, q);
                MergeSortBuildings(Buildings, q + 1, r);
                MergeBuildings(Buildings, p, q, r);
            }
            #endregion
        }    

        static void MergeBuildings(IComparable[] Buildings, long p, long q, long r)
        {
            #region Body
            long n_1 = q - p + 1;
            long n_2 = r - q;

            IComparable[] L = new IComparable[n_1];
            IComparable[] R = new IComparable[n_2];

            for (long ii = 0; ii < n_1; ii++)
            {
                L[ii] = Buildings[p + ii];
            }

            for (long jj = 0; jj < n_2; jj++)
            {
                R[jj] = Buildings[q + jj + 1];
            }

            long i = 0;
            long j = 0;

            for (long k = p; k <= r; k++) 
            {
                if (i < n_1 && (j >= n_2 || L[i].CompareTo(R[j]) == 1))
                {
                    Buildings[k] = L[i];
                    i++;
                }

                else
                {
                    Buildings[k] = R[j];
                    j++;
                }
            }

            #endregion
        }

        static List<IComparable> SilhouetMaker(IComparable[] Buildings)
        //Makes a silhouet from a single building
        {
            #region Body
            List<IComparable> Silhouets = new List<IComparable>();

            for (long i = 0; i < Buildings.Length; i++)
            {
                //So we can accas the buildings object and its variables:
                ComparableBuilding building = Buildings[i] as ComparableBuilding;

                //Add the higher most left point:
                Silhouets.Add(new ComparableSilhouet(building.left_X, building.height));

                //Add the lower most right point thingy:
                Silhouets.Add(new ComparableSilhouet(building.right_X, 0));

            }
            #endregion

            return Silhouets;
        }

        static void MergeSortSilhouets(List<IComparable> Silhouets, int p, int r)
        {
            #region Body
            if (p < r)
            {
                int q = (int)Math.Floor((double)(p + r) / 2);
                MergeSortSilhouets(Silhouets, p, q);
                MergeSortSilhouets(Silhouets, q + 1, r);
                MergeSilhouets(Silhouets, p, q, r);
            }
            #endregion
        }

        static void MergeSilhouets(List<IComparable> Silhouets, int p, int q, int r)
        {
            #region Body
            long n_1 = q - p + 1;
            long n_2 = r - q;

            List<IComparable> L = new List<IComparable>();
            List<IComparable> R = new List<IComparable>();

            for (int ii = 0; ii < n_1; ii++)
            {
                L.Add(Silhouets[p + ii]);
            }

            for (int jj = 0; jj < n_2; jj++)
            {
                R.Add(Silhouets[q + jj + 1]);
            }

            int i = 0;
            int j = 0;

            for (int k = p; k <= r; k++)
            {
                if (i < n_1 && (j >= n_2 || L[i].CompareTo(R[j]) == 1))
                {
                    Silhouets[k] = L[i];
                    i++;
                }

                else
                {
                    Silhouets[k] = R[j];
                    j++;
                }
            }

            #endregion
        }
    }
}


namespace Skyline
{

    internal class ComparableBuilding : IComparable
    {
        #region Building Object
        public long left_X;
        public long height;
        public long right_X;

        public ComparableBuilding(long Left_X, long Height, long Right_X)
        {
            this.left_X = Left_X;
            this.height = Height;
            this.right_X = Right_X;
        }

        public int CompareTo(object other2)
        {
            ComparableBuilding other = other2 as ComparableBuilding;

            if (this.left_X <= other.left_X)
            {
                return 1;
            }

            else if (this.left_X > other.left_X)
            {
                return -1;
            }

            else
            {
                return 0;
            }
        }

        #endregion
    }

    internal class ComparableSilhouet : IComparable
    {
        #region Silhouet Object
        public long x;
        public long h;

        public ComparableSilhouet(long X, long H)
        {
            this.x = X;
            this.h = H;
        }

        public int CompareTo(object other2)
        {
            ComparableSilhouet other = other2 as ComparableSilhouet;

            
            if (this.h <= other.h)
            {
                return 1;
            }

            else if (this.h > other.h)
            {
                return -1;
            }

            else
            {
                return 0;
            }

            return 0;
        }

        #endregion
    }
}

/*
INPUT:
4 // het aantal gebouwen
6 5 10 4 18 25 gebouw 2
3 3 12 nog een gebouw
4 7 7 alle mensen! Nog eentje!?! Deze is heel hoog, relatief dan.
1 5 5 de laatste alweer. Doei

OUTPUT:
5
1 5
4 7
7 5
10 3
12 0
*/
