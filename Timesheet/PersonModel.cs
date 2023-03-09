using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet
{
    public class PersonModel
    {
        //Getters and setters for the person fields from the database
        public int person_id { get; set; }
        public string person_name { get; set; }
        // A project model list so that each person has the correct projects linked to them
        public List<ProjectModel> projects { get; set; } 
    }
}
