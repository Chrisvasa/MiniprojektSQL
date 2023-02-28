using System.Diagnostics.Metrics;

namespace Timesheet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu MainMenu = new Menu(new string[] {"Person", "Projekt", "Stäng ned programmet"});
            MainMenu.UseMenu();
            List<PersonModel> PersonList = new List<PersonModel>();
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
