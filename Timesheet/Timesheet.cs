using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet
{
    internal class Timesheet
    {
        private static int selectedPerson = -1;
        private static List<PersonModel> personList = LoadAll();
        public static void Run()
        {
            Menu MainMenu = new Menu(new string[] { "Användarmeny", "Projektmeny", "Stäng ned programmet" }); // Creates the main menu
            MainMenu.Output = "Projekthanterare";
            while (true)
            {
                if (selectedPerson >= 0)
                {
                    MainMenu.Output = $"Projekthanterare för {personList[selectedPerson].person_name}";
                }
                switch (MainMenu.UseMenu())
                {
                    case 0:
                        Users();
                        break;
                    case 1:
                        if (selectedPerson >= 0)
                        {
                            Projects(personList[selectedPerson].projects);
                        }
                        else
                        {
                            Console.WriteLine("Vänligen välj en användare först!"); 
                            Console.ReadKey();
                        }
                        break;
                    case 2:
                        Console.WriteLine("Du har avslutat programmet. Hejdå!");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private static List<PersonModel> LoadAll()
        {
            personList = DataAccess.LoadPersons();
            // Fills the projects list in the PersonModel with corresponding projects
            for (int i = 0; i < personList.Count; i++)
            {
                personList[i].projects = DataAccess.LoadProjects(personList[i].person_id);
            }
            return personList;
        }

        private static void SelectUser()
        {
            Menu PersonMenu = new Menu(); // Initiates our Menu
            PersonMenu.CreatePersonMenu(personList); // Creates a menu from our List
            PersonMenu.Output = "Välj en person från listan";
            selectedPerson = PersonMenu.UseMenu(); // Uses menu and returns selected index from list
        }

        private static void Users()
        {
            Menu UserMenu = new Menu(new string[] {"Välj användare", "Redigera användare", "Skapa användare", "Ta bort användare", "Gå tillbaka"});
            bool showMenu = true;
            while (showMenu)
            {
                if(selectedPerson >= 0)
                {
                    UserMenu.SetMenuItem("Byt Användare", 0);
                }
                switch(UserMenu.UseMenu())
                {
                    case 0:
                        SelectUser();
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        showMenu = false;
                        break;
                }
            }
        }

        private static void Projects(List<ProjectModel> ProjectList)
        {
            Menu ProjectMenu = new Menu(new string[] { "Visa projekt", "Redigera projekt", "Lägg till projekt", "Ta bort projekt", "Gå tillbaka" });
            bool showMenu = true;
            while (showMenu)
            {
                int selectedOption = ProjectMenu.UseMenu();
                if (selectedOption == 0)
                {
                    ShowProjects(ProjectList);
                }
                else if (selectedOption != ProjectMenu.MenuItems.Length - 1)
                {
                    switch (selectedOption)
                    {
                        case 1:
                            int selectedProject = SelectProject(ProjectList);
                            EditProject(ProjectList[selectedProject]);
                            break;
                        case 2:
                            AddProject();
                            break;
                        case 3:
                            selectedProject = SelectProject(ProjectList);
                            RemoveProject(ProjectList[selectedProject]);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    showMenu = false;
                }
            }
        }

        private static int SelectProject(List<ProjectModel> ProjectList)
        {
            Menu ProjectMenu = new Menu(); // Initiates our Menu
            ProjectMenu.CreateProjectMenu(ProjectList); // Creates a menu from our List
            ProjectMenu.Output = "Välj ett projekt från listan";
            int selectedProject = ProjectMenu.UseMenu(); // Uses menu and returns selected index from list
            return selectedProject;
        }

        private static void ShowProjects(List<ProjectModel> items)
        {
            Console.Clear();
            string[] MenuItems = new string[items.Count];
            Console.WriteLine("Dina nuvarande projekt:");
            for (int i = 0; i < items.Count; i++)
            {
                MenuItems[i] = String.Format("|{0,-15}|{1,0}|", items[i].project_name, items[i].project_time + "h");
                Console.WriteLine(MenuItems[i]);
            }
            Console.ReadKey(true);
        }

        private static void EditProject(ProjectModel project)
        {
            // Redigera projekt
            // Låt användaren ändra ett befintligt projekt i DBn
            Menu EditMenu = new Menu(new string[] { "Titel: " + project.project_name, "Arbetstimmar: " + project.project_time });
            EditMenu.UseMenu();
            Console.WriteLine(project.project_name + " - " + project.project_time);
            Console.ReadKey();
        }

        private static void AddProject()
        {
            // Lägg till projekt
            // Ge användaren en lista med projekt att lägga till och be sedan om tiden
            // Kolla sedan i DataAccess ifall projektet redan existerar hos den användaren - OM inte? Lägg till. Annars skicka error till användaren
            Menu AddMenu = new Menu(new string[] { "Titel: ", "Arbetstimmar: " });
            AddMenu.UseMenu();
        }

        private static void RemoveProject(ProjectModel project)
        {
            // Ta bort projekt
            // Kolla om användaren är säker på att de valt rätt projekt att ta bort
            // Ifall svaret är ja, kalla på en metod i DataAccess som då tar bort fältet i DBn
            Console.WriteLine(project.project_name + " - " + project.project_time);
            Console.ReadKey();
        }
    }
}
