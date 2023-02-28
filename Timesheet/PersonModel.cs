using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet
{
    internal class PersonModel
    {
        public int id { get; set; }
        public int person_id { get; set; }
        public string person_name { get; set; }
        public List<ProjectModel> projects { get; set; }
    }
}
