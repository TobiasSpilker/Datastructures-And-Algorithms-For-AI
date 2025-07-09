using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zagen
{
    internal class Program
    {
        static void Main(string[] args)
        //Stores the data, creates objects and calls the correct methods (based on K)
        {
            #region Reading first line
            string HBK_String = Console.ReadLine();
            string[] HBK_Numbers = HBK_String.Split(' ');

            int H = Convert.ToInt32(HBK_Numbers[0]); //height
            int B = Convert.ToInt32(HBK_Numbers[1]); //width
            int K = Convert.ToInt32(HBK_Numbers[2]); //decides which method
            #endregion

            #region Reading the rest

            //Horizontal costs:
            long[] Horizontal_Costs = new long[H - 1];

            for (int i = 0; i < Horizontal_Costs.Length; i++)
            {
                string temp = Console.ReadLine();
                Horizontal_Costs[i] = Convert.ToInt64(temp.Split(' ')[0]);
            }

            //Vertical costs
            long[] Vertical_Costs = new long[B - 1];

            for (int i = 0; i < Vertical_Costs.Length; i++)
            {
                string temp = Console.ReadLine();
                Vertical_Costs[i] = Convert.ToInt64(temp.Split(' ')[0]);
            }

            #endregion

            #region Transfering data to object
            List<ComparableObject> objects_list = new List<ComparableObject>();

            //Storing horizontal costs and their respective "keys"
            for (int i = 0; i < Horizontal_Costs.Length; i++)
            {
                objects_list.Add(new ComparableObject(Horizontal_Costs[i], "Horizontal"));
            }

            //Storing vertical costs and their respective "keys"
            for (int i = 0; i < Vertical_Costs.Length; i++)
            {
                objects_list.Add(new ComparableObject(Vertical_Costs[i], "Vertical"));
            }

            #endregion

            #region Sorting (quicksort) and reversing order

            //Calling quicksort and storing sorted list:
            List<ComparableObject> ZaagPlanRev = QuickSortMethod(objects_list, 0, objects_list.Count - 1, K);

            //Reverse order:
            List<ComparableObject> ZaagPlan = new List<ComparableObject>();
            int b = 0, j = ZaagPlanRev.Count - 1;

            for (int q = 0; q < ZaagPlanRev.Count; q++)
            {
                ZaagPlan.Add(new ComparableObject(ZaagPlanRev[j].Kosten, ZaagPlanRev[j].BewegingType));
                j -= 1; b += 1;
            }

            #endregion

            ZaagPlanUitvoeren(ZaagPlan);
        }

        #region Quicksorting
        static List<T> QuickSortMethod<T>(List<T> objects_list, int begin, int end, int K) where T : IComparable<T>
        {
            //Ends if array cant be devided anymore [base case]:
            if (end - begin < K)
            {
                return SelectionSortMethod(objects_list, begin, end);
            }

            //Calling helper method:
            int Pivot = Splitter(objects_list, begin, end);

            //Left spliced Array:
            QuickSortMethod(objects_list, begin, Pivot - 1, K);

            //Right spliced Array:
            QuickSortMethod(objects_list, Pivot + 1, end, K);

            return objects_list;
        }

        static int Splitter<T>(List<T> objects_list, int begin, int end) where T : IComparable<T>
        //Helper method for QuickSortMethod - Splices the array
        {
            T Pivot = objects_list[end];
            int i = begin - 1;

            for (int j = begin; j <= end - 1; j++)
            {
                if (objects_list[j].CompareTo(objects_list[end]) == 1)
                {
                    i++;

                    //Value swap costs:
                    T tijdelijkA = objects_list[i];
                    objects_list[i] = objects_list[j];
                    objects_list[j] = tijdelijkA;
                }
            }
            i++;

            //Value swap costs:
            T tijdelijk2A = objects_list[i];
            objects_list[i] = objects_list[end];
            objects_list[end] = tijdelijk2A;

            //Value swap type:
            T tijdelijk2B = objects_list[i];
            objects_list[i] = objects_list[end];
            objects_list[end] = tijdelijk2B;

            return i;
        }

        #endregion

        #region Selection sorting
        static List<T> SelectionSortMethod<T>(List<T> objects_list, int begin, int end) where T : IComparable
        {
            for (int i = begin; i < end; i++)
            {
                int min = i;

                for (int j = min + 1; j <= end; j++)
                {
                    if (objects_list[j].CompareTo(objects_list[i]) == 1)
                    {
                        min = j;
                    }
                }

                //Value swapping for costs:
                T temp = objects_list[i];
                objects_list[i] = objects_list[min];
                objects_list[min] = temp;
            }

            return objects_list;
        }
        #endregion

        static void ZaagPlanUitvoeren(List<ComparableObject> ZaagPlan)
        {
            #region Body
            long SomKosten = 0;
            long VerticalSnedes = 1;
            long HorizontalSnedes = 1;
            long Temp = 0;
            long Zaagsnedes = 0;

            for (int i = 0; i < ZaagPlan.Count; i++)
            {
                //Keeping track of howmany (and what type of) cuts are made:
                if (ZaagPlan[i].BewegingType == "Vertical")
                { VerticalSnedes++; Zaagsnedes += HorizontalSnedes; }

                if (ZaagPlan[i].BewegingType == "Horizontal")
                { HorizontalSnedes++; Zaagsnedes += VerticalSnedes; }


                if (ZaagPlan[i].BewegingType == "Vertical")
                {
                    Temp = ZaagPlan[i].Kosten * HorizontalSnedes;
                }

                if (ZaagPlan[i].BewegingType == "Horizontal")
                {
                    Temp = ZaagPlan[i].Kosten * VerticalSnedes;
                }


                SomKosten += Temp;
            }

            Console.WriteLine(SomKosten + " " + Zaagsnedes);

            #endregion
        }
    }

}

#region Object
namespace Zagen
{

    internal class ComparableObject : IComparable<ComparableObject>
    {
        public long Kosten;
        public string BewegingType;

        public ComparableObject(long kosten, string bewegingtype)
        //Zaagbeweging object
        {
            this.Kosten = kosten;
            this.BewegingType = bewegingtype;
        }

        public int CompareTo(ComparableObject other)
        //Checks which zaagbeweging is lower / higher etc..
        {
            if (this.Kosten < other.Kosten)
            {
                return 1;
            }

            else if (this.Kosten > other.Kosten)
            {
                return -1;
            }

            else
            {
                return 0;
            }
        }

    }
}
#endregion