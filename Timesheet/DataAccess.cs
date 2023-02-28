using Dapper;
using Npgsql;
using Npgsql.Replication;
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
        public static List<PersonModel> LoadPersons()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PersonModel>($@"SELECT * FROM cva_person");
                return output.ToList();
            }
        }

        public static List<ProjectModel> LoadProjects()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<ProjectModel>($@"
            SELECT
                p.id,
                p.project_id,
                n.project_name,
                p.hours as project_time
            FROM cva_project_person p
                JOIN cva_project n ON n.id = p.project_id
            WHERE 
                person_id='{2}'");
                return output.ToList();
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