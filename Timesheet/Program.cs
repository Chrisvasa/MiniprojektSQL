using System.Diagnostics.Metrics;

namespace Timesheet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Timesheet.Run();
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
