using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Xml.Linq;
using System.Net.Sockets;


namespace C_RayFingerNetwork
{    
    public class DatabaseHandler
    {
        private string connectionString;

        public DatabaseHandler(string dbFilePath)
        {
            connectionString = $"Data Source={dbFilePath};";
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(
                    "CREATE TABLE IF NOT EXISTS Subjects (Id INTEGER PRIMARY KEY, Name TEXT, Template TEXT)", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public void SaveDB(SubjectProbe subject)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SQLiteCommand("INSERT INTO Subjects (Name, Template) VALUES (@Name, @Template)", connection))
                        {
                            command.Parameters.AddWithValue("@Id", subject.Id);
                            command.Parameters.AddWithValue("@Name", subject.Name);
                            // Convert the FingerprintTemplate to a string representation for storage in the database.
                            command.Parameters.AddWithValue("@Template", Convert.ToBase64String(subject.serializedFingerTemplate));

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public List<Subject> LoadDB()
        {
            List<Subject> subjects = new List<Subject>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT Id, Name, Template FROM Subjects", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        //Console.WriteLine("name: " + name);
                        byte[] templateAsBytes = Convert.FromBase64String((string)reader["Template"]); // Read the template as a byte array

                        // Deserialize the byte array back into a FingerprintTemplate
                        //FingerprintTemplate template = DeserializeFingerprintTemplate(templateAsBytes);
                        var newSubject = new Subject(id, name, templateAsBytes);
                        subjects.Add(newSubject);
                    }
                }
            }

            return subjects;
        }
        private void DeserializeFingerprintTemplate(byte[] templateAsBytes)
        {
            // implementation to deserialize the byte array into a FingerprintTemplate goes here.
            // https://sourceafis.machinezoo.com/net

            //var template = new FingerprintTemplate(templateAsBytes);
            //return template;
        }
    }
}
