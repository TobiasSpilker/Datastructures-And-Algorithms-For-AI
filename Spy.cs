//Tobias Spilker - Utrecht University
using System;
using System.Collections.Generic;
using System.Text;

namespace Spion
{
    class Program
    {
        static void Main(string[] args)
        {
            //Reading the amount of passwords:
            long N = Convert.ToInt64(Console.ReadLine());

            //Creating an array to store the final solved passwords:
            string[] test = new string[N];

            //For every password
            for (int k = 0; k < N; k++)
            {
                //Create a new list:
                TobyList<char> LinkedList = new TobyList<char>();
                string password = Console.ReadLine(); //storing the current password

                //for every character in a single password
                for (int j = 0; j < password.Length; j++)
                {
                    #region LinkedList operations
                    char currentChar = password[j];
                    //Move cursor to the left
                    if (currentChar == '<')
                    {
                        LinkedList.MoveCursorLeft();
                    }
                    else if (currentChar == '>')
                    //Move cursor to ther ight
                    {
                        LinkedList.MoveCursorRight();
                    }
                    //Delete a character
                    else if (currentChar == '-')
                    {
                        LinkedList.Remove();
                    }
                    //Add a character
                    else
                    {
                        LinkedList.Add(currentChar);
                    }
                    #endregion
                }

                //Adding this list thingy to the string array with a stringmaker
                test[k] = LinkedList.StringMakerij();
            }

            #region output
            //Writing the output
            for (int k = 0; k < test.Length; k++)
            {
                Console.WriteLine(test[k]);
            }
            #endregion
        }
    }
}

namespace Spion
{
    #region Element object
    public class Element<T>
    {
        public T key;
        public Element<T> prev;
        public Element<T> next;

        public Element(T Key, Element<T> Prev, Element<T> Next)
        {
            this.key = Key;
            this.prev = Prev;
            this.next = Next;
        }
    }
    #endregion

    public class TobyList<T>
    {
        //variables to be used
        private Element<T> head;
        private Element<T> tail;
        private Element<T> curser;

        public TobyList()
        //Initializing the variables
        {
            head = new Element<T>(default(T), null, null); //sentinal!
            tail = new Element<T>(default(T), head, null); //sentinal!
            head.next = tail;
            curser = head;
        }

        public string StringMakerij()
        //Stringbuilder (list object --> string)
        {
            Element<T> deze = head.next;
            StringBuilder temp = new StringBuilder();

            while (deze != tail)
            {
                temp.Append(deze.key);
                deze = deze.next;
            }

            return temp.ToString();
        }

        public void Add(T element)
        //Adding an element
        {
            Element<T> newElement = new Element<T>(element, curser, curser.next);

            curser.next.prev = newElement;
            curser.next = newElement;
            curser = newElement;
        }

        public void Remove()
        //Removing an element
        {
            if (curser != head)
            {
                Element<T> elementToRemove = curser;
                curser = curser.prev;
                curser.next = elementToRemove.next; //not sure why ¯\_(ツ)_/¯ 
                elementToRemove.next.prev = curser;
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
    }
}
/*
2
ThIsIsG3h31m
<<d<okl-i>akl<->-i<k>>

hoort de volgende uitvoer:

ThIsIsG3h31m
okidaki

*/
