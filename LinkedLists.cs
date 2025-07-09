//Tobias Spilker - Utrecht University
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace LinkedAssignment5
{
    class Program
    {
        static void Main()
        {
            //Inlezen van 1e regel input:
            string[] FirstLine = Console.ReadLine().Split(' ');
            string UitvoerModus = FirstLine[0];
            int InputSize = int.Parse(FirstLine[1]);

            //Linked List object:
            TobyList tobyList = new TobyList();

            //Inlezen en toevoegen van verdere input in de linkedlist lijst:
            #region Inlezen en toevoegen van verdere input
            for (int i = 0; i < InputSize; i++)
            {
                string[] CurrentLine = Console.ReadLine().Split(' ');
                int Key = int.Parse(CurrentLine[0]);
                string Mutatie = CurrentLine[1];

                if (Mutatie == "A")
                {
                    int x = int.Parse(CurrentLine[2]);
                    int y = int.Parse(CurrentLine[3]);

                    tobyList.Add(new Element(Key, Mutatie, x, y, null, null));
                }
                else
                {
                    int x = int.Parse(CurrentLine[2]);

                    //MOGELIJK 0 LATER VERWIJDEREN!!!!!!
                    tobyList.Add(new Element(Key, Mutatie, x, 0, null, null));
                }

            }
            #endregion

            //Door de lijst gaan en alle mutaties verwerken:
            tobyList.VerwerkMutaties(UitvoerModus);

            //Aantal elementen in de lijst:
            if (UitvoerModus ==  "C")
            {
                #region Body
                Console.WriteLine(tobyList.Count);
                #endregion
            }

            //Alle data in de lijst, getal per regel:
            if (UitvoerModus == "S")
            {
                #region Body
                string[] output_lines = tobyList.StringMakerij().Split('\n');

                for (int i = 0; i < output_lines.Length - 1; i++)
                {
                    string[] temp = output_lines[i].Split('(');
                    string[] temp2 = temp[1].Split(',');

                    Console.WriteLine(temp2[0]);
                }
                #endregion
            }

            //Frequentie tabal van de data in de lijst:
            if (UitvoerModus == "D")
            {
                #region Body
                SortedDictionary<int, int> freq = new SortedDictionary<int, int>();
                Element p = tobyList.HeadNext();

                while (p != tobyList.Tail())
                {
                    int waarde = p.waarde;

                    if (freq.ContainsKey(waarde))
                        freq[waarde]++;
                    else
                        freq[waarde] = 1;

                    p = p.next;
                }

                foreach (var pair in freq)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
                #endregion

            }

            //Alle effectieve operaties:
            if (UitvoerModus == "P")
            {
                //Wordt automatisch al gedaan bij "VerwerkMutaties" indien van toepassing
            }

        }
    }
}



namespace LinkedAssignment5
{
    //Het element object, bevat {waarde, mutatie, x, (y)}
    #region Element object
    public class Element
    {
        public int waarde;
        public string mutatie;
        public int x;
        public int y;

        public Element prev;
        public Element next;

        public Element(int WAARDE, string MUTATIE, int x, int y, Element prev, Element next)
        {
            this.waarde = WAARDE;
            this.mutatie = MUTATIE;
            this.x = x;
            this.y = y;
            this.prev = prev;
            this.next = next;
        }

        public override string ToString()
        {
            return $"({waarde}, {mutatie}, {x}, {y})";
        }
    }
    #endregion

    public class TobyList
    {
        //Nodig om de grootte bij te houden:
        private int count = 0;
        public int Count => count;

        //De standaard implementaties voor LinkedList spul:
        #region LinkedList spul
        //variables to be used
        public Element head;
        public Element tail;
        public Element curser;

        public TobyList()
        //Initializing the variables
        {
            head = new Element(-1, "", -1, -1, null, null);
            tail = new Element(-1, "", -1, -1, head, null);
            head.next = tail;
            curser = head;
        }

        public string StringMakerij()
        //Stringbuilder (list object --> string)
        {
            Element deze = head.next;
            StringBuilder temp = new StringBuilder();

            while (deze != tail)
            {
                temp.AppendLine(deze.ToString());
                deze = deze.next;
            }

            return temp.ToString();
        }

        public void Add(Element element)
        //Adding an element
        {
            Element newElement = new Element(element.waarde, element.mutatie, element.x, element.y, curser, curser.next);

            curser.next.prev = newElement;
            curser.next = newElement;
            curser = newElement;
            count++;
        }

        public void Remove()
        //Removing an element
        {
            if (curser != head)
            {
                Element elementToRemove = curser;
                curser = curser.prev;
                curser.next = elementToRemove.next; //not sure why ¯\_(ツ)_/¯ 
                elementToRemove.next.prev = curser;
                count--;
            }
        }

        public void MoveCursorLeft()
        //Moving the cursor
        {
            if (curser != head)
            {
                curser = curser.prev;
            }
        }

        public void MoveCursorRight()
        //Moving the cursor
        {
            if (curser.next != tail)
            {
                curser = curser.next;
            }
        }
        #endregion

        //Diverse methodes om de mutaties te kunnen uitvoeren op de lijst:
        #region Mutaties
        public void VerwerkMutaties(string uitvoerModus)
        //Let op, uitvoerModus is alleen als check voor 'p' omdat deze tijdens de methode bekent moet zijn
        {
            Element huidige = head.next;

            while (huidige != tail)
            {
                string mutatie = huidige.mutatie;
                int x = huidige.x;
                int y = huidige.y;

                //Uitvoeren van mutaties
                if (mutatie == "A")
                {
                    Element doel = ZoekOffset(huidige, x);

                    if (doel != null)
                    {
                        //Maakt nieuwe element aan:
                        Element nieuweNode = new Element(y, "", 0, 0, null, null);
                        VoegToeVoor(doel, nieuweNode);

                        if (uitvoerModus == "P")
                        {
                            Console.WriteLine($"toegevoegd {y}");
                        }

                    }
                }
                else if (mutatie == "R")
                {
                    //Maakt nieuw element aan:
                    Element doel = ZoekOffset(huidige, x);

                    if (doel != null && doel != head && doel != tail)
                    {
                        int verwijderWaarde = doel.waarde;
                        Verwijder(doel);

                        if (uitvoerModus == "P")
                        {
                            Console.WriteLine($"verwijderd {verwijderWaarde}");
                        }

                    }
                }

                huidige = huidige.next;
            }
        }

        private Element ZoekOffset(Element start, int offset)
        //Hulp methode voor de offset van x
        {
            //Maak element aan voor start:
            Element p = start;
            int stappen = Math.Abs(offset);

            for (int i = 0; i < stappen; i++)
            {
                p = offset > 0 ? p.next : p.prev;
                if (p == head || p == tail)
                {
                    return null;
                }

            }

            return p;
        }

        private void VoegToeVoor(Element target, Element nieuw)
        //Aangepaste Add methode
        {
            nieuw.prev = target.prev;
            nieuw.next = target;
            target.prev.next = nieuw;
            target.prev = nieuw;
            count++;
        }

        private void Verwijder(Element target)
        //Aangepaste Remove methode
        {
            target.prev.next = target.next;
            target.next.prev = target.prev;
            count--;
        }

        #endregion

        public Element HeadNext()
        {
            return head.next;
        }
        public Element Tail()
        {
            return tail;
        }

    }
}

/*
            if (UitvoerModus == "D")
            {
                int[] output_numbers = new int[tobyList.Count];

                string[] output_lines = tobyList.StringMakerij().Split('\n');

                for (int i = 0; i < output_lines.Length - 1; i++)
                {
                    string[] temp = output_lines[i].Split('(');
                    string[] temp2 = temp[1].Split(',');

                    output_numbers[i] = Convert.ToInt32(temp2[0]);
                }

                for (int i = 0; i < output_numbers.Length; i++)
                {
                    Console.WriteLine(output_numbers[i]);
                }
            }





                int[] output_numbers = new int[tobyList.Count];
                string[] output_lines = tobyList.StringMakerij().Split('\n');

                //Het opslaan van alle waardes in een array:
                for (int i = 0; i < output_lines.Length - 1; i++)
                {
                    string[] temp = output_lines[i].Split('(');
                    string[] temp2 = temp[1].Split(',');
                    output_numbers[i] = Convert.ToInt32(temp2[0]);
                }

                //Maak er een frequentietabel van:
                Dictionary<int, int> frequentieTabel = new Dictionary<int, int>();
                foreach (int getal in output_numbers)
                {
                    if (frequentieTabel.ContainsKey(getal))
                        frequentieTabel[getal]++;
                    else
                        frequentieTabel[getal] = 1;
                }

                //Geeft de output hiervan (gesoorteerd):
                foreach (var kvp in frequentieTabel.OrderBy(kvp => kvp.Key))
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }

*/