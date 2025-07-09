//Tobias Spilker - Utrecht University
using System;
using System.Collections.Generic;
using System.Linq;

class AirTraffic
{
    static void Main()
    //Processes the data and calls the methods
    {
        #region Data reading

        //reads and stores the amount of planes:
        long AmountOfPlanes = long.Parse(Console.ReadLine().Trim().Split(' ')[0]);

        //reads and stores the plane coordinates in an X-list and a Y-list:
        List<Tuple<long, long>> Coordinates = new List<Tuple<long, long>>();

        for (int i = 0; i < AmountOfPlanes; i++)
        {
            string[] temp = Console.ReadLine().Split(' ');

            Coordinates.Add(new Tuple<long, long>(long.Parse(temp[0]), long.Parse(temp[1])));
        }

        #endregion

        Console.WriteLine(Recursion(Coordinates));
    }

    static long Recursion(List<Tuple<long, long>> Coordinates)
    //Initial setup needed for recusion
    {
        //Sorting the coordinates:
        List<Tuple<long, long>> Sorted_Coordinates = Coordinates.OrderBy(t => t.Item1).ToList();

        //PREPARTION:
        #region boundary

        //Setting a line boundary:
        int Half_Length = (Sorted_Coordinates.Count / 2); //Will be used many times more;   

        if (Half_Length == 0) //Second part too?  || Half_Length == 1
        {
            return 1000000000000000000; //its a little weird but it works (no clue as to why)
        }

        long Line_Location = Sorted_Coordinates[Half_Length].Item1;
        //ERROR: uiteindelijk is de set compleet leeg (0) maar wil die m' met 0 indexeren wat
        //niet kan want set[0] bevat 1 item op de 0'de positie. Probleem moet aangepakt worden bij 
        //Half_Length zelf anders gaat deze overal een 0 doorgeven terwijl het leeg is. Vroegtijdig 
        //stoppen dus!.

        //Deciding what the area around the boundary should be:
        long Pos_A_LS = Math.Abs(Sorted_Coordinates[0].Item1 - Sorted_Coordinates[1].Item1);
        long Pos_A_RS = Math.Abs(Sorted_Coordinates[Sorted_Coordinates.Count - 2].Item1
            - Sorted_Coordinates[Sorted_Coordinates.Count - 1].Item1);

        long Beam_Thickness = 14;

        /*
        if (Pos_A_LS < Pos_A_RS) { Beam_Thickness = Convert.ToInt32(Math.Sqrt(Pos_A_LS)); }

        else { Beam_Thickness = Convert.ToInt32(Math.Sqrt(Pos_A_RS)); }
        */
        //Deciding the beam's location:

        long BeamLeftSide = Line_Location - Beam_Thickness;
        long BeamRightSide = Line_Location + Beam_Thickness;

        #endregion

        #region Deviding the coordinates into their respective "sets"
        //Creating the sets later to be filled with their respective coordinates:
        List<Tuple<long, long>> LeftSet_Coordinates = new List<Tuple<long, long>>();
        List<Tuple<long, long>> RightSet_Coordinates = new List<Tuple<long, long>>();
        List<Tuple<long, long>> SpecialSet_Coordinates = new List<Tuple<long, long>>();

        //Deviding the coordinates into 3 sets: Left set, middle set (boundary set) and right set:
        for (int i = 0; i < Sorted_Coordinates.Count; i++)
        {
            if (Sorted_Coordinates[i].Item1 < BeamLeftSide)
            {
                LeftSet_Coordinates.Add(Sorted_Coordinates[i]);
            }

            else if (Sorted_Coordinates[i].Item1 > BeamRightSide)
            {
                RightSet_Coordinates.Add(Sorted_Coordinates[i]);
            }

            else
            {
                SpecialSet_Coordinates.Add(Sorted_Coordinates[i]);
            }
        }
        #endregion

        //RECURSION
        #region Base case

        int N = Sorted_Coordinates.Count;


        if (N == 2)
        {
            return DistanceCalculator(Sorted_Coordinates[0], Sorted_Coordinates[1]);
        }

        if (N == 3)
        {
            return Distance3Points(Sorted_Coordinates[0], Sorted_Coordinates[1], Sorted_Coordinates[2]);
        }

        #endregion

        #region Devide

        long Middle = N / 2;
        long Left = Recursion(LeftSet_Coordinates);
        long Right = Recursion(RightSet_Coordinates);
        long Distance = Math.Min(Left, Right);

        #endregion

        #region Combine

        //Order special set based on its Y-values:
        SpecialSet_Coordinates = SpecialSet_Coordinates.OrderByDescending(t => t.Item2).ToList();

        long TEMPX = 1000000000; //Big enough because coordinates are below 1 million

        int q = 0; //Meant for indexing 2nd loop and keeping track as to when to break

        //Pairing every point within the special set with the next 15 points:
        for (int i = 0; i < SpecialSet_Coordinates.Count; i++)
        {
            while (true)
            {
                //Possible sensitive to bugs: If there are two points that are the same:
                if (TEMPX > DistanceCalculator(SpecialSet_Coordinates[i], SpecialSet_Coordinates[q])
                    && SpecialSet_Coordinates[i] != SpecialSet_Coordinates[q])
                {
                    TEMPX = DistanceCalculator(SpecialSet_Coordinates[i], SpecialSet_Coordinates[q]);
                }

                q += 1;

                //Checks if all the coordinates OR the limit of 15 is reached
                if (q == 15 || q == SpecialSet_Coordinates.Count) { break; }
            }

            q = 0;
        }

        //Compare beam area with the minimum of left/right:
        Distance = Math.Min(TEMPX, Distance);

        #endregion

        return Distance;
    }

    static long DistanceCalculator(Tuple<long, long> Point_1, Tuple<long, long> Point_2)
    //Calculates the difference between 2 points using Pythagoras theorom
    {
        #region Changing the data a little
        long X_1 = Point_1.Item1;
        long Y_1 = Point_1.Item2;

        long X_2 = Point_2.Item1;
        long Y_2 = Point_2.Item2;
        #endregion

        #region Calculating c squared

        //A squared:
        long a = Math.Abs(X_1 - X_2) * Math.Abs(X_1 - X_2);

        //B squared:
        long b = Math.Abs(Y_1 - Y_2) * Math.Abs(Y_1 - Y_2);

        //C squared:
        long c = a + b;

        #endregion

        return c;
    }

    static long Distance3Points(Tuple<long, long> Point_1, Tuple<long, long> Point_2, Tuple<long, long> Point_3)
    //Calculates the difference between 3 points using Pythagoras theorom
    {
        #region Changing the data a little
        long X_1 = Point_1.Item1;
        long Y_1 = Point_1.Item2;

        long X_2 = Point_2.Item1;
        long Y_2 = Point_2.Item2;

        long X_3 = Point_3.Item1;
        long Y_3 = Point_3.Item2;
        #endregion

        #region Calculating c squared

        //POINT 1 AND 2:
        long a_1_2 = Math.Abs(X_1 - X_2) * Math.Abs(X_1 - X_2);
        long b_1_2 = Math.Abs(Y_1 - Y_2) * Math.Abs(Y_1 - Y_2);
        long c_1_2 = a_1_2 + b_1_2;

        //POINT 2 AND 3:
        long a_2_3 = Math.Abs(X_2 - X_3) * Math.Abs(X_2 - X_3);
        long b_2_3 = Math.Abs(Y_2 - Y_3) * Math.Abs(Y_2 - Y_3);
        long c_2_3 = a_2_3 + b_2_3;

        //POINT 1 AND 3:
        long a_1_3 = Math.Abs(X_1 - X_3) * Math.Abs(X_1 - X_3);
        long b_1_3 = Math.Abs(Y_1 - Y_3) * Math.Abs(Y_1 - Y_3);
        long c_1_3 = a_1_3 + b_1_3;

        //Deciding shortest distance:
        long[] SketchySolution = { c_1_2, c_2_3, c_1_3 };
        Array.Sort(SketchySolution);
        long c = SketchySolution[0];
        #endregion

        return c;
    }

}

#region Test Cases
/*

10 // 10 vliegtuigen
34 22 /// eerste vliegtuig
73 46 /2de vliegtuig
30 50 % et
31 24 $ cetera
34 63
28 79
24 26
10 81
42 52
95 5 dat was het alweer!

*/

#endregion

#region Resources
/*

PDF FILES:
C:\Users\Tobias\Desktop\Kunstmatige intelligentie\Jaar 2\Blok 1\Datastructuren en algoritmen voor KI\Seminars\Code Assignments\3\PR_AirTrafficBb.pdf
C:\Users\Tobias\Desktop\Kunstmatige intelligentie\Jaar 2\Blok 1\Datastructuren en algoritmen voor KI\Seminars\Code Assignments\3\SEE ASSIGNMENT 22.pdf

YOUTUBE LINKS:
https://www.youtube.com/watch?v=4VqmGXwpLqc
https://www.youtube.com/watch?v=7LN9z140U90

https://www.youtube.com/watch?v=6u_hWxbOc7E [Complete explenation in pseudocode]
https://www.youtube.com/watch?time_continue=149&v=0W_m46Q4qMc [Complete explenation in pseudocode]

*/

#endregion