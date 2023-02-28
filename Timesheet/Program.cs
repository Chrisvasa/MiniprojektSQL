using System.Diagnostics.Metrics;

namespace Timesheet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<PersonModel> PersonList = DataAccess.LoadPersons();
            List<ProjectModel> ProjectList = DataAccess.LoadProjects();
            Menu MainMenu = new Menu(new string[] {"Person", "Projekt", "Stäng ned programmet"});
            while(true)
            {
                switch (MainMenu.UseMenu())
                {
                    case 0:
                        PersonTest(PersonList);
                        break;
                    case 1:
                        ProjectTest(ProjectList);
                        break;
                    case 2:
                        break;
                }
            }
        }

        private static void PersonTest(List<PersonModel> personList)
        {
            Menu PersonMenu = new Menu();
            PersonMenu.ListToArray(personList);
            PersonMenu.UseMenu();
        }

        private static void ProjectTest(List<ProjectModel> projectList)
        {
            Menu ProjectMenu = new Menu();
            ProjectMenu.ListToArray(projectList);
            ProjectMenu.UseMenu();
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
