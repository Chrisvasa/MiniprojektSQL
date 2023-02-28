using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet
{
    internal class DataAccess
    {

        public static void LoadProjects()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query($@"
            SELECT
                n.project_name as Projekt,
                p.hours as Tid
            FROM cva_project_person p
                JOIN cva_project n ON n.id = p.project_id
            WHERE person_id='{user_id}'");
            }
        }
        private static string LoadConnectionString(string id = "Default")
        {
            using (IDbConnection cnn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings[id].ConnectionString))
            {
                return ConfigurationManager.ConnectionStrings[id].ConnectionString;
            };
        }
    }
}
