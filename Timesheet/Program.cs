using System.Diagnostics.Metrics;

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
                        SelectUser(personList);
                        break;
                    case 1:
                        if (selectedPerson >= 0) 
                        {
                            //ProjectTest(personList[selectedPerson].projects); 
                            Projects(personList[selectedPerson].projects);
                        }
                        else 
                        { 
                            Console.WriteLine("Vänligen välj en användare först!"); Console.ReadKey(); 
                        }
                        break;
                    case 2:
                        break;
                }
            }
        }

        private static List<PersonModel> LoadAll(Menu MainMenu)
        {
            List<PersonModel> personList = DataAccess.LoadPersons();
            // Fills the projects list in the PersonModel with corresponding projects
            for (int i = 0; i < personList.Count; i++)
            {
                personList[i].projects = DataAccess.LoadProjects(personList[i].person_id);
            }
            return personList;
        }

        private static void SelectUser(List<PersonModel> personList)
        {
            Menu PersonMenu = new Menu(); // Initiates our Menu
            PersonMenu.ListToArray(personList); // Creates a menu from our List
            PersonMenu.Output = "Välj en person från listan";
            selectedPerson = PersonMenu.UseMenu(); // Uses menu and returns selected index from list
            Console.WriteLine(personList[selectedPerson].person_name);
            Console.ReadKey();
        }

        private static void Projects(List<ProjectModel> ProjectList)
        {
            Menu ProjectMenu = new Menu(new string[] {"Visa projekt", "Redigera projekt", "Lägg till projekt", "Ta bort projekt", "Gå tillbaka"});
            bool showMenu = true;
            while(showMenu)
            {
                int selectedOption = ProjectMenu.UseMenu();
                if(selectedOption == ProjectMenu.MenuItems.Length) 
                {
                    int selectedProject = SelectProject(ProjectList);
                    switch (selectedOption)
                    {
                        case 0:
                            break;
                        case 1:
                            EditProject(ProjectList[selectedProject]);
                            break;
                        case 2:
                            AddProject(ProjectList[selectedProject]);
                            break;
                        case 3:
                            RemoveProject(ProjectList[selectedProject]);
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
            ProjectMenu.ListToArray(ProjectList); // Creates a menu from our List
            ProjectMenu.Output = "Välj ett projekt från listan";
            int selectedProject = ProjectMenu.UseMenu(); // Uses menu and returns selected index from list
            return selectedProject;
        }

        private static void EditProject(ProjectModel project)
        {
            // Redigera projekt
            Console.WriteLine(project.project_name + " - " + project.project_time);
            Console.ReadKey();
        }

        private static void AddProject(ProjectModel project)
        {
            // Lägg till projekt
            Console.WriteLine(project.project_name + " - " + project.project_time);
            Console.ReadKey();
        }

        private static void RemoveProject(ProjectModel project)
        {
            // Ta bort projekt
            Console.WriteLine(project.project_name + " - " + project.project_time);
            Console.ReadKey();
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
