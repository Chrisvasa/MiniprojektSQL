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
                // Checks if a person is selected, and changes the menu title
                if (selectedPerson >= 0)
                {
                    MainMenu.Output = $"Projekthanterare för {personList[selectedPerson].person_name}";
                }
                switch (MainMenu.UseMenu())
                {
                    case 0:
                        UsersMenu(); // Menu with options to choose a user, edit, create and delete users
                        break;
                    case 1:
                        ProjectsMenu(); // Menu with options to show projects, edit, create and delete projects
                        break;
                    case 2:
                        Console.WriteLine("Du har avslutat programmet. Hejdå!");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        // Method that loads all the data about each person from the database
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

        // Menu for all the different user menu options (Choose, edit, create and remove users)
        private static void UsersMenu()
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

        // Outputs all the users into a menu and returns the selected persons index in the list.
        private static void SelectUser()
        {
            Menu PersonMenu = new Menu(); // Initiates our Menu
            PersonMenu.CreatePersonMenu(personList); // Creates a menu from our List
            PersonMenu.Output = "Välj en person från listan";
            selectedPerson = PersonMenu.UseMenu(); // Uses menu and returns selected index from list
        }

        // Method that allows for creation of new users
        private static void CreateUser()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Var vänlig och skapa en ny användare.");
                Console.Write("Namn: ");
                string userName = Helper.FormatString(Console.ReadLine().Trim());
                if (Helper.isValid(userName)) // Checks if all chars in the name are valid
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

        // Allows for changing of names on current users in the database
        private static void EditUser()
        {
            // If a user has not been selected, this will allow you to do so
            if (selectedPerson == -1)
            {
                SelectUser();
            }
            Console.WriteLine($"Vill du ändra på namnet {personList[selectedPerson].person_name}? Y/N");
            if(Helper.Confirm()) 
            {
                Console.Write("Skriv in det nya namnet: ");
                string userName = Helper.FormatString(Console.ReadLine().Trim()); ;
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

        // A method that allows you to delete a user from the database
        private static void DeleteUser()
        {
            // If a user has not been selected, this will allow you to do so
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

        // Calls for the projects menu, if you have selected a user or tells you to select a user first
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
            List<ProjectModel> AllProjects = DataAccess.LoadProjects();
            Menu ProjectMenu = new Menu(new string[] { "Visa projekt [" + ProjectList.Count + "]", "Redigera projekt", "Lägg till projekt", "Ta bort projekt", "Gå tillbaka" });
            bool showMenu = true;
            while (showMenu)
            {
                int selectedOption = ProjectMenu.UseMenu(); // Returns index of the selected menu item 
                if (selectedOption == 0)
                {
                    ShowProjects(ProjectList);
                }
                else if (selectedOption != ProjectMenu.MenuItems.Length - 1) // Checks if user pressed go back
                {
                    switch (selectedOption)
                    {
                        case 1:
                            int selectedProject = SelectProject(ProjectList);
                            if(selectedProject >= 0) 
                            {
                                EditProject(ProjectList[selectedProject]);
                            }
                            else
                            {
                                Console.WriteLine("Du har för nävarande inga projekt");
                                Console.ReadKey();
                            }
                            break;
                        case 2:
                            AddProject(AllProjects);
                            break;
                        case 3:
                            selectedProject = SelectProject(ProjectList);
                            if(selectedProject >= 0)
                            {
                                RemoveProject(ProjectList[selectedProject]);
                            }
                            else
                            {
                                Console.WriteLine("Du har för nävarande inga projekt");
                                Console.ReadKey();
                            }
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

        // A method that prompts the user with a list of project and returns the selected project
        // If there are no projects for the user, returns a -1
        private static int SelectProject(List<ProjectModel> ProjectList)
        {
            Menu ProjectMenu = new Menu(); // Initiates our Menu
            ProjectMenu.CreateProjectMenu(ProjectList); // Creates a menu from our List
            ProjectMenu.Output = "Välj ett projekt från listan";
            if(ProjectList.Count > 0) // If user has projects
            {
                int selectedProject = ProjectMenu.UseMenu(); // Uses menu and returns selected index from list
                return selectedProject;
            }
            return -1;
        }

        // Prints out the users projects
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

        // Method to allow editing a users hours on different projects
        private static void EditProject(ProjectModel project)
        {
            Menu EditMenu = new Menu(new string[] { "Titel: " + project.project_name, "Arbetstimmar: " + project.project_time, "Spara ändringar" });
            bool showMenu = true;
            bool confirmation = false;
            string input = string.Empty; 
            string hours = string.Empty;
            while(showMenu)
            {
                EditMenu.PrintMenu();
                // Allows the user to edit both the project name and hour spent on a project
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
                    project.project_name = Helper.FormatString(input); // Formats the project name then updates it in the ProjectModel
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
        // Method to add a project to the user and set the hours spent
        private static void AddProject(List<ProjectModel> projects)
        {
            Menu AddMenu = new Menu(new string[] { "Titel: ", "Arbetstimmar: ", "Spara ändringar" ,"Gå tillbaka" });
            bool showMenu = true;
            string selected = string.Empty;
            string hours = string.Empty;
            while (showMenu)
            {
                switch (AddMenu.UseMenu())
                {
                    case 0:
                        selected = ChooseProject(projects);
                        AddMenu.SetMenuItem("Titel: " + selected, 0);
                        AddMenu.PrintMenu();
                        break;
                    case 1:
                        AddMenu.SetMenuItem("Arbetstimmar: ", 1);
                        AddMenu.PrintMenu();
                        AddMenu.MoveCursorRight();
                        hours = Console.ReadLine();
                        AddMenu.SetMenuItem("Arbetstimmar: " + hours, 1);
                        break;
                    case 2:
                        SaveProject(selected, hours);
                        break;
                    case 3:
                        showMenu = false;
                        break;
                }
            }
        }
        // Creates a menu that outputs the projects that are not already assigned to the selected user
        // Then returns the selected project name
        private static string ChooseProject(List<ProjectModel> projects)
        {
            Menu ChoiceMenu = new Menu();
            projects = ChoiceMenu.ChooseProjectMenu(projects, personList[selectedPerson].projects); 
            int selectedProject = ChoiceMenu.UseMenu();
            return projects[selectedProject].project_name;
        }
        // Checks that both the project name and hours are correct and then prompts the user for confirmation to save the project to the database
        private static void SaveProject(string projectName, string hours)
        {
            if(Helper.isValid(projectName) && int.TryParse(hours, out int result))
            {
                Console.WriteLine($"Vill du skapa projektet med namnet {projectName} och timmarna {hours}? Y/N");
                if(Helper.Confirm())
                {
                    DataAccess.AddProject(projectName, result, personList[selectedPerson].person_id);
                    personList = LoadAll();
                    Console.WriteLine("Projektet har nu lagts till.");
                    Console.ReadKey();
                }
            }
        }
        // Prompts the user for confirmation before removing the selected project from the users list
        private static void RemoveProject(ProjectModel project)
        {
            Console.WriteLine("Är du säker på att du vill ta bort projektet? Y/N");
            if(Helper.Confirm())
            {
                Console.WriteLine($"Denna ändring är permanent. Vill du fortsätta med borttagningen av projektet {project.project_name}? Y/N");
                if(Helper.Confirm()) 
                {
                    DataAccess.RemoveProject(project);
                    Console.WriteLine($"{project.project_name} har nu tagits bort.");
                    Console.ReadKey();
                    personList = LoadAll();
                }
            }
        }
    }
}
