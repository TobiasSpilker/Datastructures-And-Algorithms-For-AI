//Tobias Spilker - Utrecht University
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;


namespace LinkedAssignment5
{
    class Program
    {

        //Voorkomt gedoe met bitshiften:
        private const ulong MASK = (1UL << 5) - 1;
        static ulong LettersMask(int n) => (1UL << (n * 5)) - 1UL;

        static void Main()
        {
            //Reading the firstline:
            string[] FirstLine = Console.ReadLine().Split(' ');
            int N_letters = Convert.ToInt32(FirstLine[0]);
            string Modus = FirstLine[1];

            //Reading the 2nd line:
            string HuidigWoord = Console.ReadLine();

            //Reading the 3th line:
            string DoelWoord = Console.ReadLine();

            //Het echte werk:
            BFS(N_letters, HuidigWoord, DoelWoord, Modus);
        }

        static void BFS(int N_letters, string HuidigWoord, string DoelWoord, string Modus)
        //Breadth First Search implementatie
        {
            #region Start stadia
            //Start stadia:
            long start = Encodeer(HuidigWoord, '?');
            long doel = Encodeer(DoelWoord, '?');

            //Aanmaken van queue:
            var wachtrij = new Queue<long>();

            //Bijhouden welke al geweest zijn:
            var geweest = new HashSet<long>();

            //Waarde v/d vorige toestand:
            var vorige = new Dictionary<long, (long, char)>();

            //Begin met met het huidige woord:
            wachtrij.Enqueue(start);
            geweest.Add(start);
            #endregion

            while (wachtrij.Count > 0)
            {
                //Houdt bij wat de huidige permutatie is
                long huidig = wachtrij.Dequeue();

                if (huidig == doel)
                //Als deze gelijk is aan doel dan overgaan op checken welke output modus
                {
                    #region 3x verschillende output modus [L, S, A]
                    if (Modus == "L")
                    {
                        //Tel hoeveel stappen er zijn gezet door de count op te vragen vd lijst
                        Console.WriteLine(Reconstruer(huidig, vorige).Count);
                    }

                    else if (Modus == "S")
                    {
                        //Vraag count op + haal lijst op met de stappen zelf
                        var stappen = Reconstruer(huidig, vorige);
                        Console.WriteLine(stappen.Count);
                        Console.WriteLine(string.Join("", stappen));
                    }

                    else if (Modus == "A")
                    {
                        //Speciale "animatie" methode
                        AnimatieHelper(huidig, vorige, N_letters);
                    }
                    #endregion

                    return;
                }

                //UITVOEREN VAN DE MOGELIJKE MUTATIES:
                #region Uitvoeren van de verschillende mutaties [R, L, W]

                //Rechtsrol:
                long rechts = RechtsRol(huidig, N_letters);
                if (!geweest.Contains(rechts))
                {
                    geweest.Add(rechts);
                    wachtrij.Enqueue(rechts);
                    vorige[rechts] = (huidig, 'R');
                }

                //Linksrol:
                long links = LinksRol(huidig, N_letters);
                if (!geweest.Contains(links))
                {
                    geweest.Add(links);
                    wachtrij.Enqueue(links);
                    vorige[links] = (huidig, 'L');
                }

                //Wissel:
                long wissel = Wissel(huidig, N_letters);
                if (!geweest.Contains(wissel))
                {
                    geweest.Add(wissel);
                    wachtrij.Enqueue(wissel);
                    vorige[wissel] = (huidig, 'W');
                }

                #endregion
            }
        }

        static long RechtsRol(long input, int n)
        //Plaatst de laatste letter vooraan met bitshift gedoe
        {
            #region Body
            //Bitshift gedoe:
            ulong state = (ulong)input;
            ulong letters = state & LettersMask(n);
            ulong los = state >> (n * 5);

            ulong first = letters & MASK;
            ulong rest = letters >> 5;

            ulong rotated = (first << ((n - 1) * 5)) | rest;
            ulong result = (los << (n * 5)) | rotated;
            #endregion

            return (long)result;
        }

        static long LinksRol(long input, int n)
        //Plaatst de voorste letter achteraan met bitshift gedoe
        {
            #region Body
            //Bitshift gedoe:
            ulong state = (ulong)input;
            ulong letters = state & LettersMask(n);
            ulong los = state >> (n * 5);

            ulong last = letters >> ((n - 1) * 5);
            ulong lower = letters & (LettersMask(n) >> 5);

            ulong shifted = ((letters << 5) & LettersMask(n)) | last;
            ulong result = (los << (n * 5)) | shifted;
            #endregion

            return (long)result;
        }

        static long Wissel(long input, int n)
        //Verwisseld de losse letter met de laatste met bitshift gedoe
        {
            #region Body
            //Bitshift gedoe:
            ulong state = (ulong)input;
            ulong letters = state & LettersMask(n);
            ulong los = state >> (n * 5);

            ulong first = letters & MASK;
            if (first == los) return input;

            ulong newLetters = (letters & ~MASK) | los;
            ulong result = (first << (n * 5)) | newLetters;
            #endregion

            return (long)result;
        }

        #region Encoderen en Decoderen

        static long Encodeer(string plank, char los)
        //Transformeerd de string naar 5 bit representatie
        {
            ulong s = (ulong)CharToInt(los);
            foreach (char c in plank)
            {
                s <<= 5; //5 aka de 5 bits
                s |= (ulong)CharToInt(c);
            }
            return (long)s;
        }

        static (string plank, char los) Decodeer(long enc, int n)
        //Transformeerd de 5-bit representatie weer naar een string
        {
            ulong s = (ulong)enc;
            char[] plank = new char[n];
            for (int i = n - 1; i >= 0; i--)
            {
                plank[i] = IntToChar((int)(s & MASK));
                s >>= 5; //5 als in 5 bits
            }
            char los = IntToChar((int)(s & MASK));

            //Return waarde:
            return (new string(plank), los);
        }

        static int CharToInt(char c) => c == '?' ? 26 : c - 'a';
        static char IntToChar(int v) => v == 26 ? '?' : (char)('a' + v);

        #endregion


        static List<char> Reconstruer(long eind, Dictionary<long, (long, char)> vorige)
        //Reconstrueerd de gemaakte stappen (helper voor modus S en A)
        {
            #region Body
            //Lijst v/d stappen:
            var stappen = new List<char>();

            while (vorige.ContainsKey(eind))
            {
                var (vorig, mutatie) = vorige[eind];
                stappen.Add(mutatie);
                eind = vorig;
            }

            stappen.Reverse(); //MOGELIJK PROBLEMATISCH?!
            #endregion

            return stappen;
        }

        static void AnimatieHelper(long eind, Dictionary<long, (long, char)> vorige, int n)
        //Helper methode om de juiste output te krijgen bij modus A
        {
            #region Body
            var steps = new List<(char, long)>();
            long current = eind;

            //Reconstrueer in omgekeerde volgorde (ook nodig net als bij modus S)
            while (vorige.ContainsKey(current))
            {
                var (vorig, mutatie) = vorige[current];
                steps.Add((mutatie, current));
                current = vorig;
            }

            steps.Reverse(); //MOGELIJK PROBLEMATISCH?!

            //Print het aantal stappen:
            Console.WriteLine(steps.Count);

            //Print de stappen volgorde:
            Console.WriteLine(string.Join("", steps.Select(s => s.Item1)));

            //Print de beginstand:
            var (plankStart, losStart) = Decodeer(current, n);
            Console.WriteLine("S " + plankStart + "." + losStart);

            //Print de opvolgende gemaakte mutaties:
            foreach (var stap in steps) //var vanwege compiler gedoe
            {
                char actie = stap.Item1;
                long toestand = stap.Item2;

                var decode = Decodeer(toestand, n);
                string plank = decode.Item1;
                char los = decode.Item2;

                Console.WriteLine(actie + " " + plank + "." + los);
            }
            #endregion
        }

    }
}