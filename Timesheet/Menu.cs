using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet
{
    // A class that handles the creation of menus and allows the user to interact with them
    internal class Menu
    {
        string[] menuItems = Array.Empty<string>(); // MenuArray initialization
        int selectedIndex = 0; // Keeps track of menu positions
        ConsoleColor color = ConsoleColor.DarkGreen; // Color of the menu
        string output = string.Empty; // Output - Printed above the menu
        private char selectedItem = '\u21aa'; // Char to show currently selected item in menu
        private char item = ' '; // Char for the rest

        // Takes an array of strings on class instantiation
        public Menu() { }
        public Menu(string[] items)
        {
            menuItems = items;
        }
        #region GettersAndSetters
        // Getter and setter for the selected index
        // SelectedIndex is the currently selected item in the menu
        public int SelectIndex
        {
            get { return selectedIndex; }
            set
            {
                // Checks if index is within bounds
                if (value >= 0 && value < menuItems.Length)
                {
                    selectedIndex = value;
                }
            }
        }
        // Retrieve or change the output printed above the menu
        public string Output
        {
            get { return output; }
            set { output = value; }
        }

        // Allows user to change and set new menus - if needed
        public string[] MenuItems
        {
            get { return menuItems; }
            set { menuItems = value; }
        }

        // Returns the menu array
        public string[] GetMenu()
        {
            return menuItems;
        }
        // Change a item in the menu array at given index
        public void SetMenuItem(string item, int index)
        {
            menuItems[index] = item;
        }
        public string GetMenuItem()
        {
            return menuItems[selectedIndex];
        }

        // Change color of the menu 
        public ConsoleColor SetColor
        {
            get { return color; }
            set { color = value; }
        }
        public char SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; }
        }
        public char Item
        {
            get { return item; }
            set { item = value; }
        }
        #endregion
        // Moves the cursor to the right of currently selected menu item
        public void MoveCursorRight()
        {
            if (!string.IsNullOrWhiteSpace(output))
            {
                Console.SetCursorPosition(menuItems[selectedIndex].Length + 2, selectedIndex + 1);
            }
            else
            {
                Console.SetCursorPosition(menuItems[selectedIndex].Length + 2, selectedIndex);
            }
        }
        // Moves the cursor to the bottom of current menu
        public void MoveCursorBottom()
        {
            Console.SetCursorPosition(0, menuItems.Length);
        }
        // Moves the cursor to the top of the current menu
        public void MoveCursorTop()
        {
            Console.SetCursorPosition(0, 0);
        }
        // Creates a menu from the personmodel outputting only person names
        public void CreatePersonMenu(List<PersonModel> items)
        {
            MenuItems = new string[items.Count];
            for(int i = 0; i < items.Count; i++)
            {
                MenuItems[i] = items[i].person_name;
            }
        }
        // Creates a menu from the projectmodel and some basic formatting to show project name and hours spent on a project
        public void CreateProjectMenu(List<ProjectModel> items)
        {
            MenuItems = new string[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                MenuItems[i] = String.Format("|{0,-15}|{1,0}|", items[i].project_name, items[i].project_time + "h");
            }
        }
        // Compares all the projects in the database with the selected persons projects
        // And only adds the unused ones to the menu
        public List<ProjectModel> ChooseProjectMenu(List<ProjectModel> projects, List<ProjectModel> personProjects)
        {
            var result = new List<ProjectModel>();
            int match;
            for(int i = 0; i < projects.Count; i++)
            {
                match = 0; // Resets the match counter
                for(int j = 0; j < personProjects.Count; j++)
                {
                    if (projects[i].project_name == personProjects[j].project_name)
                    {
                        match++; // For each match found
                    }
                }
                if(match == 0) // Only adds a project if its unique
                {
                    result.Add(projects[i]);
                }
            }
            MenuItems = new string[result.Count];
            for(int i = 0; i < result.Count; i++)
            {
                MenuItems[i] = result[i].project_name;
            }
            return result;
        }

        // A method that prints the menu when called
        public void PrintMenu()
        {
            Console.Clear();
            // Prints out the menu items in the console, and puts brackets around the selected item.
            if (!String.IsNullOrWhiteSpace(output))
            {
                Console.ForegroundColor = color;
                Console.WriteLine(output);
                Console.ResetColor();
            }
            for (int i = 0; i < menuItems.Length; i++)
            {
                // Checks if the current menu choice is the selected item 
                // Gives the text a given color and puts brackets around it
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine("{0} {1}", selectedItem, menuItems[i]);
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine("{0} {1}", item, menuItems[i]);
                }
                Console.ResetColor();
            }
        }

        // A method that allows the user to orientate around the menu
        public int UseMenu()
        {
            // Calls for a class that deals with user input
            // Handles validation of input and only returns valid keyinput
            InputHandler menuInput = new InputHandler();

            bool usingMenu = true;
            do
            {
                PrintMenu();
                ConsoleKey userInput = menuInput.ReadInput(menuItems, selectedIndex); // Returns keyinput if valid
                // Moves up and down in the array, depending on the input
                // If user presses enter, breaks the loop and returns currenty selected index
                if (userInput == ConsoleKey.UpArrow)
                {
                    SelectIndex--;
                }
                else if (userInput == ConsoleKey.DownArrow)
                {
                    SelectIndex++;
                }
                else if (userInput == ConsoleKey.Enter || userInput == ConsoleKey.Spacebar)
                {
                    break; // Will break the loop and return index of currently selected item in the menu
                }
                else
                {
                    SelectIndex = menuInput.GetIndex();
                }
                PrintMenu(); // Prints the newly updated menu
            } while (usingMenu);
            return selectedIndex;
        }
    }
}
