using System;
using System.Collections.Generic;
using System.Linq;

namespace HigherOrLower
{
    public class Menu
    {
        private List<string> options = new List<string>();
        private string[] titleFragments;
        private readonly string title;
        private string largestTitleFragment;
        private int selected = 0;
        private bool completed = false;
        private bool numbering = true;

        public string Title => title;

        public Menu(string title)
        {
            this.title = title;
            this.titleFragments = title.Split("\n");

            // sort the title fragments with lambda expression in
            // descending order to locate the largest fragment which will be the box width
            Array.Sort(this.titleFragments, (x, y) => y.Length - x.Length);
            largestTitleFragment = this.titleFragments[0];
            
        }
        public Menu AddOptions(string[] options)
        {
            foreach (string option in options)
            {
                this.options.Add(option);
            }

            return this;
        }

        public Menu EnableNumbering(bool on)
        {
            numbering = on;

            return this;
        }

        public void DisplayBox()
        {
            Console.ForegroundColor = ConsoleColor.Red;

            // border top
            HorizontalLine(largestTitleFragment.Length + 2, "┌", "┐");

            // print each title fragment to add box borders
            foreach (string titleFragment in title.Split("\n"))
            {
                // calculate the remaining space for each fragment which is the difference between largest fragment
                // length minus this fragment length
                string[] spacingArr = Enumerable.Repeat(" ", largestTitleFragment.Length - titleFragment.Length).ToArray();
                string spacer = string.Join("", spacingArr);
                if (titleFragment.Length > 0)
                    Console.WriteLine($"│ {titleFragment}{spacer} │");
            }

            // border bottom
            HorizontalLine(largestTitleFragment.Length + 2, "└", "┘");
            Console.ResetColor();
        }

        public string GetOption()
        {
            // While there is no selected option, render menu
            Console.CursorVisible = false;
            while (!completed)
            {
                
                Console.Clear();

                DisplayBox();

                // List all options
                for (int i = 0; i < options.Count; i++)
                {
                    // Filler changes depending whether the option is selected or not
                    string filler = "   ";
                    if (i == selected)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        filler = " > ";
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    string prefix = numbering ? (i + 1).ToString() : "";

                    Console.WriteLine($"{prefix}|{filler}{options[i]}");
                    Console.ResetColor();
                }

                // Get readkey info
                var keyInfo = Console.ReadKey();
                int key = (int)keyInfo.Key;

                // Down arrow
                if (key == 40)
                {
                    if (selected + 1 < options.Count)
                        selected++;
                    // If the current selected is out of bounds, reset it
                    else
                        selected = 0;
                }
                // Up arrow
                else if (key == 38)
                {
                    if (selected - 1 >= 0)
                        selected--;
                    // If the current selected is out of bounds, reset it
                    else
                        selected = options.Count - 1;
                }
                // Enter
                else if (key == 13)
                {
                    completed = true;
                    Console.Clear();
                }

            }

            Console.CursorVisible = true;

            return GetSelected();
        }
        private void HorizontalLine(int length, string start, string end)
        {
            Console.Write(start);
            Console.Write(new string('─', length));
            Console.Write(end);
            Console.WriteLine();
        }
        public string GetSelected()
        {
            return options[selected];
        }
    }
}