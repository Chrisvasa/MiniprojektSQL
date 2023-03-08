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
        // Loads id and name from the person table
        public static List<PersonModel> LoadPersons()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PersonModel>($@"SELECT id AS person_id, person_name FROM cva_person");
                return output.ToList();
            }
        }
        // Loads id, project id, project name and the project time from the project table
        public static List<ProjectModel> LoadProjects(int personID)
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
                person_id='{personID}'");
                return output.ToList();
            }
        }

        public static void EditProject(ProjectModel project)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute($"UPDATE cva_project_person SET hours = @project_time WHERE id = @id", project);
                cnn.Execute($"UPDATE cva_project SET project_name = @project_name WHERE id = @project_id", project);
            }
        }

        public static void CreateUser(string user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute($"INSERT INTO cva_person (person_name) VALUES ('{user}')");
            }
        }

        internal static void EditUser(PersonModel person)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute($"UPDATE cva_person SET person_name = @person_name WHERE id = @person_id", person);
            }
        }

        internal static void DeleteUser(PersonModel person)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute($"DELETE FROM cva_project_person WHERE person_id = @person_id", person);
                cnn.Execute($"DELETE FROM cva_person WHERE id = @person_id", person);
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