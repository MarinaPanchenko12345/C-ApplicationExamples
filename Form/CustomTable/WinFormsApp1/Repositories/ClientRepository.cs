using System; // Importing System namespace for basic types and exceptions
using System.Collections.Generic; // Importing generic collections
using System.Data.SqlClient; // Importing SQL Server data provider
using System.Linq; // Importing LINQ (not used in this file)
using System.Text; // Importing text utilities (not used in this file)
using System.Threading.Tasks; // Importing threading tasks (not used in this file)
using WinFormsApp1.Models; // Importing models from the application

namespace WinFormsApp1.Repositories // Declaring the namespace for repositories
{
    public class ClientRepository // Declaring the ClientRepository class
    {
        private readonly string connectionString =
    "Data Source=MARINAIGOR\\SQLEXPRESS;Initial Catalog=MyTrainingDB;Integrated Security=True;TrustServerCertificate=True;";

        // Connection string for the database

        /// <summary>
        /// Retrieves all clients from the database, ordered by id descending.
        /// Returns a list of Client objects.
        /// </summary>
        public List<Client> GetClients() // Method to get all clients
        {
            var clients = new List<Client>(); // Create a list to store clients

            try // Start try block to catch exceptions
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) // Create and open a SQL connection
                {
                    connection.Open(); // Open the database connection

                    string sql = "SELECT * FROM clients ORDER BY id DESC"; // SQL query to select all clients ordered by id descending
                    using (SqlCommand command = new SqlCommand(sql, connection)) // Create a SQL command with the query
                    {
                        using (SqlDataReader reader = command.ExecuteReader()) // Execute the command and get a data reader
                        {
                            while (reader.Read()) // Loop through each row in the result
                            {
                                Client client = new Client(); // Create a new Client object

                                client.id = reader.GetInt32(0); // Set client id from the first column
                                client.firstName = reader.GetString(1); // Set client first name from the second column
                                client.lastName = reader.GetString(2); // Set client last name from the third column
                                client.email = reader.GetString(3); // Set client email from the fourth column
                                client.phone = reader.GetString(4); // Set client phone from the fifth column
                                client.address = reader.GetString(5); // Set client address from the sixth column
                                client.createAt = reader.GetDateTime(6).ToString(); // Set client creation date from the seventh column

                                clients.Add(client); // Add the client to the list
                            }
                        }
                    }
                }
            }
            catch (Exception ex) // Catch any exceptions
            {

                Console.WriteLine("Exception : " + ex.ToString()); // Print the exception to the console
            }

            return clients; // Return the list of clients
        }

        /// <summary>
        /// Retrieves a single client by id from the database.
        /// Returns a Client object if found, otherwise null.
        /// </summary>
        /// <param name="id">The id of the client to retrieve.</param>
        public Client? GetClient(int id) // Method to get a client by id
        {
            try // Start try block to catch exceptions
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) // Create and open a SQL connection
                {
                    connection.Open(); // Open the database connection
                    string sql = "SELECT * FROM clients WHERE id=@id"; // SQL query to select a client by id
                    using (SqlCommand command = new SqlCommand(sql, connection)) // Create a SQL command with the query
                    {
                        command.Parameters.AddWithValue("@id", id); // Add the id parameter to the command
                        using (SqlDataReader reader = command.ExecuteReader()) // Execute the command and get a data reader
                        {
                            if (reader.Read()) // If a row is found
                            {
                                Client client = new Client(); // Create a new Client object

                                client.id = reader.GetInt32(0); // Set client id from the first column
                                client.firstName = reader.GetString(1); // Set client first name from the second column
                                client.lastName = reader.GetString(2); // Set client last name from the third column
                                client.email = reader.GetString(3); // Set client email from the fourth column
                                client.phone = reader.GetString(4); // Set client phone from the fifth column
                                client.address = reader.GetString(5); // Set client address from the sixth column
                                client.createAt = reader.GetDateTime(6).ToString(); // Set client creation date from the seventh column

                                return client; // Return the found client

                            }
                        }
                    }

                }
            }
            catch (Exception ex) // Catch any exceptions
            {
                Console.WriteLine("Exception : " + ex.ToString()); // Print the exception to the console
            }
            return null; // Return null if not found or on error
        }

        /// <summary>
        /// Inserts a new client into the database.
        /// </summary>
        /// <param name="client">The Client object to insert.</param>
        public void CreateClient(Client client) // Method to create a new client
        {
            try // Start try block to catch exceptions
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) // Create and open a SQL connection
                {
                    connection.Open(); // Open the database connection
                    string sql = "INSERT INTO clients " + // SQL query to insert a new client (note: typo in table name)
                        "(firstName,lastName,email,phone,address) VALUES " + // Specify columns to insert
                        "(@firstName,@lastName,@email,@phone,@address);"; // Specify parameter placeholders

                    using (SqlCommand command = new SqlCommand(sql, connection)) // Create a SQL command with the query
                    {
                        command.Parameters.AddWithValue("@firstName", client.firstName); // Add first name parameter
                        command.Parameters.AddWithValue("@lastName", client.lastName); // Add last name parameter
                        command.Parameters.AddWithValue("@email", client.email); // Add email parameter
                        command.Parameters.AddWithValue("@phone", client.phone); // Add phone parameter
                        command.Parameters.AddWithValue("@address", client.address); // Add address parameter

                        command.ExecuteNonQuery(); // Execute the insert command
                    }
                }
            }
            catch (Exception ex) // Catch any exceptions
            {

                Console.WriteLine("Exception : " + ex.ToString()); // Print the exception to the console
            }
        }


        public void UpdateClient(Client client)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE clients " +
                        "SET " +
                        "firstName=firstName,lastName=@lastName, " +
                        "email=@email,phone=@phone,address=@address " +
                        "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", client.firstName); // Add first name parameter
                        command.Parameters.AddWithValue("@lastName", client.lastName); // Add last name parameter
                        command.Parameters.AddWithValue("@email", client.email); // Add email parameter
                        command.Parameters.AddWithValue("@phone", client.phone); // Add phone parameter
                        command.Parameters.AddWithValue("@address", client.address); // Add address parameter

                        command.ExecuteNonQuery(); // Execute the insert command
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception : " + ex.ToString());
            }
        }


        public void DeleteClient(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM clients WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id); 

                        command.ExecuteNonQuery(); // Execute the insert command
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception : " + ex.ToString());
            }
        }


    }
}