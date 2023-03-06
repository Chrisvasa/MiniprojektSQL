﻿using System.Diagnostics.Metrics;

namespace Timesheet
{
    internal class Program
    {
        public static int selectedPerson = -1;
        static void Main(string[] args)
        {
            Menu MainMenu = new Menu(new string[] {"Användare", "Projekt", "Stäng ned programmet"}); // Creates the main menu
            MainMenu.Output = "Projekthanterare";
            List<PersonModel> personList = LoadAll(MainMenu);
            while (true)
            {
                if (selectedPerson >= 0)
                {
                    MainMenu.Output = $"Projekthanterare för {personList[selectedPerson].person_name}";
                }
                switch (MainMenu.UseMenu())
                {
                    case 0:
                        PersonTest(personList);
                        break;
                    case 1:
                        ProjectTest(personList[selectedPerson].projects);
                        break;
                    case 2:
                        break;
                }
            }
        }

        private static List<PersonModel> LoadAll(Menu MainMenu)
        {
            List<PersonModel> personList = DataAccess.LoadPersons();
            for (int i = 0; i < personList.Count; i++)
            {
                personList[i].projects = DataAccess.LoadProjects(personList[i].person_id);
            }
            return personList;
        }

        private static void PersonTest(List<PersonModel> personList)
        {
            Menu PersonMenu = new Menu(); // Initiates our Menu
            PersonMenu.ListToArray(personList); // Creates a menu from our List
            PersonMenu.Output = "Välj en person från listan";
            selectedPerson = PersonMenu.UseMenu(); // Uses menu and returns selected index from list
            Console.WriteLine(personList[selectedPerson].person_name);
            Console.ReadKey();
        }

        private static void ProjectTest(List<ProjectModel> ProjectList)
        {
            Menu ProjectMenu = new Menu(); // Initiates our Menu
            ProjectMenu.ListToArray(ProjectList); // Creates a menu from our List
            ProjectMenu.Output = "Välj ett projekt från listan";
            int selectedProject = ProjectMenu.UseMenu(); // Uses menu and returns selected index from list
        }
    }
}

/* Load in persons and projects
Print menu 
> Person
> Projects <-- Need to choose person before selecting this
> Exit

If person selected:
> Billy
  Leif
return id

Get projects matching person_id
*/
