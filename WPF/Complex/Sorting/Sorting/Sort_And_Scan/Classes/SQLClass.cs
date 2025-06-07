using System; // Importing base .NET types // Импорт базовых типов .NET
using System.Collections.Generic; // Importing collections // Импорт коллекций
using System.Data; // Importing ADO.NET data types // Импорт типов данных ADO.NET
using System.Data.SqlClient; // Importing SQL Server client // Импорт клиента SQL Server
using System.Linq; // Importing LINQ // Импорт LINQ
using System.Text; // Importing text utilities // Импорт утилит для работы с текстом
using System.Threading.Tasks; // Importing async utilities // Импорт утилит для асинхронности
using System.Windows; // Importing WPF UI elements // Импорт элементов WPF UI

namespace Sort_And_Scan.Classes // Namespace for organizing code // Пространство имён для организации кода
{
    public class SQLClass // Class for SQL Server database operations // Класс для работы с базой данных SQL Server
    {
        private readonly string connectionString = ""; // Connection string for SQL Server // Строка подключения к SQL Server

        // Constructor to initialize connection string // Конструктор для инициализации строки подключения
        public SQLClass(string ServerName, string DataBaseName, string UserName, string Secret)
        {
            // Build the connection string using provided parameters // Формируем строку подключения из переданных параметров
            connectionString =
           "Data Source=" + ServerName + ";" + // Set server name // Устанавливаем имя сервера
           "Initial Catalog=" + DataBaseName + ";" + // Set database name // Устанавливаем имя базы данных
           "User id=" + UserName + ";" + // Set user name // Устанавливаем имя пользователя
           "Password=" + Secret + ";"; // Set password // Устанавливаем пароль
        }

        // Executes a SQL query that does not return results (e.g., INSERT, UPDATE, DELETE) // Выполняет SQL-запрос, который не возвращает результат (например, INSERT, UPDATE, DELETE)
        public void Execute(string query)
        {
            try // Try to execute the query // Пытаемся выполнить запрос
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString)) // Create and open SQL connection // Создаём и открываем соединение с SQL
                {
                    using (SqlCommand cmd = new SqlCommand(query, sqlConn)) // Create SQL command with query // Создаём SQL-команду с запросом
                    {
                        sqlConn.Open(); // Open the connection // Открываем соединение
                        cmd.ExecuteNonQuery(); // Execute the command (no result expected) // Выполняем команду (результат не ожидается)
                    }
                }
            }
            catch (Exception ex) // Catch any exceptions // Ловим любые исключения
            {
                MessageBox.Show(ex.Message); // Show error message in a message box // Показываем сообщение об ошибке в окне сообщений
            }
        }

        // Executes a SQL query and returns the result as a DataTable // Выполняет SQL-запрос и возвращает результат в виде DataTable
        public DataTable GetDataTable(string sql)
        {
            DataTable dataTable = new DataTable(); // Create a new DataTable to hold results // Создаём новый DataTable для хранения результатов
            try // Try to execute the query // Пытаемся выполнить запрос
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString)) // Create and open SQL connection // Создаём и открываем соединение с SQL
                {
                    SqlCommand cmd = new SqlCommand(sql, sqlConn); // Create SQL command with query // Создаём SQL-команду с запросом
                    sqlConn.Open(); // Open the connection // Открываем соединение
                    SqlDataAdapter da = new SqlDataAdapter(cmd); // Create data adapter for filling DataTable // Создаём адаптер данных для заполнения DataTable
                    da.Fill(dataTable); // Fill DataTable with query results // Заполняем DataTable результатами запроса
                    sqlConn.Close(); // Close the connection // Закрываем соединение
                    da.Dispose(); // Dispose the data adapter // Освобождаем ресурсы адаптера данных
                }    
            }
            catch (Exception ex) // Catch any exceptions // Ловим любые исключения
            {
                MessageBox.Show(ex.Message); // Show error message in a message box // Показываем сообщение об ошибке в окне сообщений
            }
            return dataTable; // Return the filled DataTable // Возвращаем заполненный DataTable
        }
    }
}