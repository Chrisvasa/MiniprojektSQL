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
                        ProjectsMenu();
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
            SortList();
            return personList;
        }

        private static void SortList()
        {
            foreach (PersonModel person in personList)
            {
                personList.OrderBy(person => person.person_id);
            }
        }

        private static void Users()
        {
            SortList();
            Menu UserMenu = new Menu(new string[] {"Välj användare", "Redigera användare", "Skapa användare", "Ta bort användare", "Gå tillbaka"});
            bool showMenu = true;
            while (showMenu)
            {
                if(selectedPerson >= 0)
                {
                    UserMenu.SetMenuItem("Byt Användare", 0);
                }
                else
                {
                    UserMenu.SetMenuItem("Välj användare", 0);
                }
                switch(UserMenu.UseMenu())
                {
                    case 0:
                        SelectUser();
                        break;
                    case 1:
                        EditUser();
                        break;
                    case 2:
                        CreateUser();
                        break;
                    case 3:
                        DeleteUser();
                        break;
                    case 4:
                        showMenu = false;
                        break;
                }
            }
        }

        private static void SelectUser()
        {
            Menu PersonMenu = new Menu(); // Initiates our Menu
            PersonMenu.CreatePersonMenu(personList); // Creates a menu from our List
            PersonMenu.Output = "Välj en person från listan";
            selectedPerson = PersonMenu.UseMenu(); // Uses menu and returns selected index from list
        }

        private static void CreateUser()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Var vänlig och skapa en ny användare.");
                Console.Write("Namn: ");
                string userName = Helper.FormatString(Console.ReadLine());
                if (Helper.isValid(userName))
                {
                    Console.WriteLine($"Du skrev in {userName} - Ser detta rätt ut? Y/N");
                    if(Helper.Confirm())
                    {
                        DataAccess.CreateUser(userName);
                        Console.WriteLine($"Användaren med namnet {userName} har skapats.");
                        Console.ReadKey();
                        selectedPerson = -1;
                        personList = LoadAll();
                    }
                    else
                    {
                        Console.WriteLine("Användaren har inte skapats.");
                        Console.ReadKey();
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Vänligen ange endast bokstäver i ditt namn. Försök igen.");
                    Console.ReadKey();
                }
            } while (true);
        }

        private static void EditUser()
        {
            if(selectedPerson == -1)
            {
                SelectUser();
            }
            Console.WriteLine($"Vill du ändra på namnet {personList[selectedPerson].person_name}? Y/N");
            if(Helper.Confirm()) 
            {
                Console.Write("Skriv in det nya namnet: ");
                string userName = Helper.FormatString(Console.ReadLine());
                Console.WriteLine($"Ditt nya namn kommer bli {userName} - Ser detta bra ut? Y/N");
                if (Helper.isValid(userName) && Helper.Confirm())
                {
                    personList[selectedPerson].person_name = userName;
                    DataAccess.EditUser(personList[selectedPerson]);
                    Console.WriteLine("Namnet har ändrats.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Inga ändringar har skett.");
                    Console.ReadKey();
                }
            }
        }

        private static void DeleteUser()
        {
            if (selectedPerson == -1)
            {
                SelectUser();
            }
            Console.WriteLine($"Vill du ta bort {personList[selectedPerson].person_name} som användare? Y/N");
            if (Helper.Confirm())
            {
                Console.WriteLine("Är du helt säker? Detta sker permanent. Y/N");
                if(Helper.Confirm())
                {
                    DataAccess.DeleteUser(personList[selectedPerson]);
                    selectedPerson = -1;
                    personList = LoadAll();
                }
            }
        }

        private static void ProjectsMenu()
        {
            SortList();
            if (selectedPerson >= 0)
            {
                Projects(personList[selectedPerson].projects);
            }
            else
            {
                Console.WriteLine("Vänligen välj en användare först!");
                Console.ReadKey();
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
            Menu EditMenu = new Menu(new string[] { "Titel: " + project.project_name, "Arbetstimmar: " + project.project_time, "Spara ändringar" });
            bool showMenu = true;
            bool confirmation = false;
            string input = string.Empty; 
            string hours = string.Empty;
            while(showMenu)
            {
                EditMenu.PrintMenu();
                switch (EditMenu.UseMenu())
                {
                    case 0:
                        EditMenu.SetMenuItem("Titel: ", 0);
                        EditMenu.PrintMenu();
                        EditMenu.MoveCursorRight();
                        input = Console.ReadLine();
                        EditMenu.SetMenuItem("Titel: " + input, 0);
                        break;
                    case 1:
                        EditMenu.SetMenuItem("Arbetstimmar: ", 1);
                        EditMenu.PrintMenu();
                        EditMenu.MoveCursorRight();
                        hours = Console.ReadLine();
                        EditMenu.SetMenuItem("Arbetstimmar: " + hours, 1);
                        break;
                    case 2:
                        Console.WriteLine("Vill du spara dina ändringar? Y/N");
                        confirmation = Helper.Confirm();
                        showMenu = false;
                        break;
                }
            }
            if(confirmation == true)
            {
                if(input.Length > 0)
                {
                    project.project_name = Helper.FormatString(input);
                }
                if(hours.Length > 0)
                {
                    project.project_time = int.Parse(hours);
                }
                DataAccess.EditProject(project);
                Console.WriteLine("Dina ändringar har sparats.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Ändringarna har inte sparats.");
                Console.ReadKey();
            }
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
