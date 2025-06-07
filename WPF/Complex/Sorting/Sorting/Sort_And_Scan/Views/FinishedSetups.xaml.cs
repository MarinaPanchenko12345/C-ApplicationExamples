using Sort_And_Scan.Classes; // Import SQLClass and other classes from the Classes namespace // Импорт SQLClass и других классов из пространства имён Classes
using System; // Import base .NET types // Импорт базовых типов .NET
using System.Collections.Generic; // Import collections // Импорт коллекций
using System.Data; // Import ADO.NET data types // Импорт типов данных ADO.NET
using System.Linq; // Import LINQ // Импорт LINQ
using System.Text; // Import text utilities // Импорт утилит для работы с текстом
using System.Threading; // Import threading utilities // Импорт утилит для многопоточности
using System.Threading.Tasks; // Import async utilities // Импорт утилит для асинхронности
using System.Windows; // Import WPF UI elements // Импорт элементов WPF UI
using System.Windows.Controls; // Import WPF controls // Импорт WPF контролов
using System.Windows.Data; // Import data binding // Импорт привязки данных
using System.Windows.Documents; // Import document elements // Импорт элементов документов
using System.Windows.Input; // Import input handling // Импорт обработки ввода
using System.Windows.Media; // Import drawing utilities // Импорт утилит для рисования
using System.Windows.Media.Imaging; // Import image handling // Импорт работы с изображениями
using System.Windows.Navigation; // Import navigation // Импорт навигации
using System.Windows.Shapes; // Import shape drawing // Импорт рисования фигур

namespace Sort_And_Scan.Views // Namespace for organizing views // Пространство имён для организации представлений
{
    public partial class FinishedSetups : UserControl // Partial class for FinishedSetups user control // Частичный класс для пользовательского контрола FinishedSetups
    {
        // Constructor for FinishedSetups user control // Конструктор для пользовательского контрола FinishedSetups
        public FinishedSetups()
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            GetData(); // Load and display data when control is created // Загрузка и отображение данных при создании контрола
        }

        // Method to load data from the database and display it in the grid // Метод для загрузки данных из базы данных и отображения их в таблице
        private void GetData()
        {
            // Create a new SQLClass instance with connection parameters // Создаём новый экземпляр SQLClass с параметрами подключения
            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", "");

            // Prepare SQL query to select all records ordered by Time descending // Готовим SQL-запрос для выбора всех записей, отсортированных по времени по убыванию
            string sqlStr = string.Format(@"SELECT * FROM Sort_And_Scan ORDER BY Time DESC");

            // Execute the query and get the results as a DataTable // Выполняем запрос и получаем результаты в виде DataTable
            DataTable DT1 = sql.GetDataTable(sqlStr);

            // Convert each DataRow to a FinishedRow object using LINQ // Преобразуем каждую строку DataRow в объект FinishedRow с помощью LINQ
            List<FinishedRow> list = (from rw in DT1.AsEnumerable()
                                      select new FinishedRow()
                                      {
                                          PL = rw["PL"].ToString()?.Trim(), // Get and trim PL column // Получаем и обрезаем столбец PL
                                          WH = rw["WH"].ToString()?.Trim(), // Get and trim WH column // Получаем и обрезаем столбец WH
                                          Customer = rw["Customer"].ToString()?.Trim(), // Get and trim Customer column // Получаем и обрезаем столбец Customer
                                          Line = rw["Line"].ToString()?.Trim(), // Get and trim Line column // Получаем и обрезаем столбец Line
                                          Worker = rw["Worker"].ToString()?.Trim(), // Get and trim Worker column // Получаем и обрезаем столбец Worker
                                          Total_Scanned = rw["Total_Scanned"].ToString()?.Trim(), // Get and trim Total_Scanned column // Получаем и обрезаем столбец Total_Scanned
                                          Time = Convert.ToDateTime(rw["Time"]), // Convert Time column to DateTime // Преобразуем столбец Time в DateTime
                                      }).ToList(); // Convert the query result to a list // Преобразуем результат запроса в список

            Grid1.ItemsSource = list; // Set the data source of the grid to the list // Устанавливаем источник данных для таблицы в список
        }
    }
}