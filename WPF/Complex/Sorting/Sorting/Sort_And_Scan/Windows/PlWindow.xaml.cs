using Sort_And_Scan.Classes; // Import project classes // Импорт классов проекта
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sort_And_Scan.Windows // Namespace for organizing windows // Пространство имён для организации окон
{
    /// <summary>
    /// Interaction logic for PlWindow.xaml // Логика взаимодействия для PlWindow.xaml
    /// </summary>
    public partial class PlWindow : Window // Partial class for PlWindow window // Частичный класс для окна PlWindow
    {
        List<WhKittingModel> ResultList = new List<WhKittingModel>(); // List to store result WhKittingModel objects // Список для хранения итоговых объектов WhKittingModel

        // Constructor for PlWindow // Конструктор для окна PlWindow
        public PlWindow()
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            GetData(); // Load initial data // Загружаем начальные данные
        }

        // Method to get all kitting data from the database // Метод для получения всех данных комплектации из базы данных
        private void GetData()
        {
            List<WhKittingModel> List = new List<WhKittingModel>(); // Temporary list for kitting data // Временный список для данных комплектации
            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", ""); // Create SQLClass instance for Setup DB // Создаём экземпляр SQLClass для базы Setup

            string sqlStr = string.Format(@"SELECT *  FROM Sort_And_Scan_Wh_Kitting ORDER BY Date DESC"); // SQL query to get all kitting records // SQL-запрос для получения всех записей комплектации
            DataTable DT1 = sql.GetDataTable(sqlStr); // Execute query and get results // Выполняем запрос и получаем результаты

            foreach (DataRow dr in DT1.Rows) // trim string data // Для каждой строки результата (обрезаем строки)
            {
                WhKittingModel m = new WhKittingModel(); // Create new WhKittingModel // Создаём новый WhKittingModel
                m.PL = dr["PL"].ToString().Trim(); // Get and trim PL // Получаем и обрезаем PL
                m.Employee_Name = dr["Employee_Name"].ToString() == null ? " " : dr["Employee_Name"].ToString(); // Get employee name or set to space if null // Получаем имя сотрудника или пробел, если null
                m.Date = dr["Date"] != DBNull.Value ? Convert.ToDateTime(dr["Date"]) : default; // Get date or default // Получаем дату или значение по умолчанию
                m.Status = dr["Status"].ToString().Trim(); // Get and trim status // Получаем и обрезаем статус
                List.Add(m); // Add to temporary list // Добавляем во временный список
            }

            CheckValues(List); // Process and summarize the data // Обрабатываем и суммируем данные
        }

        // Method to summarize kitting data by PL and fill the result list // Метод для суммирования данных комплектации по PL и заполнения итогового списка
        private void CheckValues(List<WhKittingModel> AllList)
        {
            var Pls = AllList.Select(item => item.PL).Distinct().ToList(); // Get distinct PLs // Получаем уникальные PL

            foreach (var item in Pls) // For each PL // Для каждого PL
            {
                if (!ResultList.Any(x => x.PL == item)) // If PL not already in result list // Если PL ещё нет в итоговом списке
                {
                    int totalFirst = AllList.Where(x => x.Status == "First" && x.PL == item).Count(); // Count First status // Считаем количество First
                    int totalBackup = AllList.Where(x => x.Status == "Backup" && x.PL == item).Count(); // Count Backup status // Считаем количество Backup
                    string EmployeeName = AllList.Where(x => x.PL == item).Select(y => y.Employee_Name).FirstOrDefault() == null ? "" : AllList.Where(x => x.PL == item).Select(y => y.Employee_Name).FirstOrDefault(); // Get employee name // Получаем имя сотрудника
                    DateTime Date = AllList.Where(x => x.PL == item).Select(y => y.Date).FirstOrDefault() == null ? default : AllList.Where(x => x.PL == item).Select(y => y.Date).FirstOrDefault(); // Get date // Получаем дату
                    WhKittingModel whKittingModel = new WhKittingModel(); // Create new WhKittingModel // Создаём новый WhKittingModel
                    whKittingModel.PL = item; // Set PL // Устанавливаем PL
                    whKittingModel.Total_First = totalFirst; // Set total First // Устанавливаем общее количество First
                    whKittingModel.Total_Backup = totalBackup; // Set total Backup // Устанавливаем общее количество Backup
                    whKittingModel.Employee_Name = EmployeeName; // Set employee name // Устанавливаем имя сотрудника
                    whKittingModel.Date = Date; // Set date // Устанавливаем дату
                    whKittingModel.Customer = GetCustomer(item); // Get and set customer // Получаем и устанавливаем заказчика
                    ResultList.Add(whKittingModel); // Add to result list // Добавляем в итоговый список
                }
            }
            // Set the data source for the grid, ordered by date descending // Устанавливаем источник данных для таблицы, сортируя по дате по убыванию
            Grid1.ItemsSource = ResultList
                .OrderByDescending(x => x.Date.Year)
                .ThenByDescending(x => x.Date.Month)
                .ThenByDescending(x => x.Date.Day);
        }

        // Method to get customer name for a given PL from BAAN // Метод для получения имени заказчика по PL из BAAN
        private string GetCustomer(string p)
        {
            string customer = string.Empty; // Variable to store customer name // Переменная для хранения имени заказчика
            using (OdbcConnection DbConnection = new OdbcConnection("DSN=BAAN")) // Create ODBC connection to BAAN // Создаём ODBC соединение с BAAN
            {
                try
                {
                    DbConnection.Open(); // Open the connection // Открываем соединение
                    OdbcCommand DbCommand = DbConnection.CreateCommand(); // Create command object // Создаём объект команды

                    // Prepare SQL query to get customer info for the given PL // Готовим SQL-запрос для получения информации о заказчике по PL
                    string q = string.Format(@"SELECT tticst910400.t_pdno AS WO,
                                                      ttiitm200400.t_prot AS Trace,
                                                      CASE WHEN ttdltc605400.t_fval IS NULL THEN 0 ELSE ttdltc605400.t_fval END AS TraceCustomer,
                                                      ttiitm001400.t_item AS PN,
                                                      ttisfc050400.t_dsca AS WO_Desc,
                                                      ttisfc001400.t_qrdr AS Qty,
                                                      ttisfc001400.t_revi AS Rev,
                                                      ttisfc001400.t_rohs AS Rohs,
                                                      ttiitm001400.t_dsca AS Desc,
                                                      ttccom010400.t_nama AS Customer,
                                                      ttiitm950400.t_mitm AS CustomerItem,
                                                      ttccom010400.t_fdad AS Medical,
                                                      tticst910400.t_prio AS Priority,
                                                      ttisfc001400.t_npif AS Type,
                                                      ttisfc001400.t_tchn AS Tech
                                            FROM baandb.tticst910400 tticst910400
                                            LEFT JOIN baandb.ttisfc001400 ttisfc001400 ON tticst910400.t_pdno = ttisfc001400.t_pdno
                                            LEFT JOIN baandb.ttiitm001400 ttiitm001400 ON ttiitm001400.t_item  = ttisfc001400.t_mitm 
                                            LEFT JOIN baandb.ttiitm200400 ttiitm200400 ON ttiitm200400.t_item = ttiitm001400.t_item 
                                            LEFT JOIN baandb.ttccom010400 ttccom010400 ON ttccom010400.t_cuno = ttiitm200400.t_cuno
                                            LEFT JOIN baandb.ttiitm950400 ttiitm950400 ON ttiitm001400.t_item = ttiitm950400.t_item
                                            LEFT JOIN baandb.ttisfc050400 ttisfc050400 ON ttisfc001400.t_npif = ttisfc050400.t_npif
                                            LEFT JOIN baandb.ttdltc605400 ttdltc605400 ON ttdltc605400.t_cuno = ttccom010400.t_cuno 
                                            WHERE 
                                            tticst910400.t_pino='{0}' AND 
                                            ttiitm950400.t_mnum='999' AND
                                            ttiitm950400.t_exdt <= Date('01/01/2001')
                                            ORDER BY tticst910400.t_pdno", p);

                    DbCommand.CommandText = q; // Set command text // Устанавливаем текст команды
                    OdbcDataReader DbReader = DbCommand.ExecuteReader(); // Execute query and get reader // Выполняем запрос и получаем ридер

                    while (DbReader.Read()) // Read each row // Читаем каждую строку
                    {
                        customer = DbReader.GetString(9).Trim(); // Get and trim customer name // Получаем и обрезаем имя заказчика
                    }
                    DbReader.Close(); // Close reader // Закрываем ридер
                }
                catch (OdbcException ex)
                {
                    MessageBox.Show("connection to the DSN '" + "BAAN" + "' failed." + ex.Message); // Show error message // Показываем сообщение об ошибке
                    return ""; // Return empty string on error // Возвращаем пустую строку при ошибке
                }
                return customer; // Return customer name // Возвращаем имя заказчика
            }
        }
    }
}