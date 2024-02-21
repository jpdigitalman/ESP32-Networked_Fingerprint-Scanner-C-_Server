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
        public int SaveDB(SubjectProbe subject)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                int rowCount = -1;

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SQLiteCommand("INSERT INTO Subjects (Name, Template) VALUES (@Name, @Template)", connection))
                        {
                            // Remove the @Id parameter if "Id" is auto-incremented in the database
                            // command.Parameters.AddWithValue("@Id", subject.Id);

                            command.Parameters.AddWithValue("@Name", subject.Name);
                            // Convert the FingerprintTemplate to a string representation for storage in the database.
                            command.Parameters.AddWithValue("@Template", subject.serializedFingerTemplate);

                            rowCount = command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error saving to the database: {ex.Message}");
                    }
                }

                return rowCount;
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
                        //byte[] templateAsBytes = (byte[])reader["Template"]; //Convert.FromBase64String((string)reader["Template"]); // Read the template as a byte array
                        byte[] templateAsBytes = null;

                        // Explicitly retrieve data as stream using GetStream
                        using (var stream = reader.GetStream(reader.GetOrdinal("Template")))
                        {
                            if (stream != null && stream.Length > 0)
                            {
                                templateAsBytes = new byte[stream.Length];
                                stream.Read(templateAsBytes, 0, templateAsBytes.Length);
                            }
                        }

                        var newSubject = new Subject(id, name, templateAsBytes);
                        subjects.Add(newSubject);
                    }
                }
            }

            return subjects;
        }

        public int GetNumberOfItems()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Count the number of rows in the table
                using (SQLiteCommand countCommand = new SQLiteCommand("SELECT COUNT(*) FROM Subjects", connection))
                {
                    // ExecuteScalar is used to retrieve a single value (in this case, the count)
                    int rowCount = Convert.ToInt32(countCommand.ExecuteScalar());

                    //Console.WriteLine($"Number of rows in the table: {rowCount}");
                    return rowCount;
                }                
            }            
        }
    }
}
