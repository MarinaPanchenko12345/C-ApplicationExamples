using Sort_And_Scan.Classes; // Import project classes // Импорт классов проекта
using Sort_And_Scan.Views; // Import view controls // Импорт контролов представлений
using System; // Import base .NET types // Импорт базовых типов .NET
using System.Collections.Generic; // Import collections // Импорт коллекций
using System.Data.Odbc; // Import ODBC for database access // Импорт ODBC для доступа к базе данных
using System.Linq; // Import LINQ // Импорт LINQ
using System.Text; // Import text utilities // Импорт утилит для работы с текстом
using System.Threading.Tasks; // Import async utilities // Импорт утилит для асинхронности
using System.Windows; // Import WPF UI elements // Импорт элементов WPF UI
using System.Windows.Controls; // Import WPF controls // Импорт WPF контролов
using System.Windows.Data; // Import data binding // Импорт привязки данных
using System.Windows.Documents; // Import document elements // Импорт элементов документов
using System.Windows.Input; // Import input handling // Импорт обработки ввода
using System.Windows.Media; // Import drawing utilities // Импорт утилит для рисования
using System.Windows.Media.Imaging; // Import image handling // Импорт работы с изображениями
using System.Windows.Shapes; // Import shape drawing // Импорт рисования фигур

namespace Sort_And_Scan.Windows // Namespace for organizing windows // Пространство имён для организации окон
{
    /// <summary>
    /// Interaction logic for PL.xaml // Логика взаимодействия для PL.xaml
    /// </summary>
    public partial class PL : Window // Partial class for PL window // Частичный класс для окна PL
    {
        public string pl { get; set; } // Property to store selected PL // Свойство для хранения выбранного PL
        public static MainWindow mn = new MainWindow(); // Static reference to MainWindow // Статическая ссылка на MainWindow
        List<Item> Items = new List<Item>(); // List to store Item objects // Список для хранения объектов Item

        // Constructor for PL window // Конструктор для окна PL
        public PL()
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
        }

        // Event handler for OK button click // Обработчик нажатия кнопки OK
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed; // Hide the window // Скрываем окно
            this.DialogResult = true; // Set dialog result to true // Устанавливаем результат диалога в true
            this.pl = PLTxt.Text; // Store the entered PL value // Сохраняем введённое значение PL
        }

        // Event handler for PL textbox losing focus // Обработчик потери фокуса полем PL
        private void PLTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait; // Set cursor to wait // Устанавливаем курсор ожидания
            GetItems(PLTxt.Text.Trim()); // Load items for the entered PL // Загружаем элементы для введённого PL
            CheckIfFirstOrBackup(); // Check if items are First or Backup // Проверяем, являются ли элементы First или Backup
            Mouse.OverrideCursor = null; // Reset cursor // Сбрасываем курсор
        }

        // Method to check if items are First or Backup // Метод для проверки, являются ли элементы First или Backup
        private void CheckIfFirstOrBackup()
        {
            try
            {
                int countFirst = 0; // Counter for First items // Счётчик для First элементов
                int countBackup = 0; // Counter for Backup items // Счётчик для Backup элементов
                foreach (var item in Items) // Loop through each item // Проходим по каждому элементу
                {
                    int qtyCount = 0; // Counter for accumulated quantity // Счётчик накопленного количества
                    List<LotPerPL> Lots = GetLots(item.PN); // Get lots for the item // Получаем лоты для элемента

                    for (int i = 0; i < Lots.Count; i++) // Loop through each lot // Проходим по каждому лоту
                    {
                        qtyCount += Lots[i].Qty; // Add lot quantity to counter // Прибавляем количество лота к счётчику

                        if (i == 0) // If this is the first lot // Если это первый лот
                        {
                            countFirst++; // Increment First counter // Увеличиваем счётчик First
                        }
                        if (qtyCount >= item.Qty) // If accumulated quantity covers item requirement // Если накопленное количество покрывает требуемое для элемента
                        {
                            countBackup++; // Increment Backup counter // Увеличиваем счётчик Backup
                            break; // Stop checking further lots // Прекращаем проверку следующих лотов
                        }
                        if (qtyCount < item.Qty) // If still not enough quantity // Если всё ещё недостаточно количества
                        {
                            countBackup++; // Increment Backup counter // Увеличиваем счётчик Backup
                        }
                    }
                }
                // Display the counts in the UI // Отображаем количество в интерфейсе
                FirstAndBackup.Text = "First: " + countFirst + " " + "Backup: " + countBackup;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Show error message // Показываем сообщение об ошибке
            }
        }

        // Method to get items for a given PL // Метод для получения элементов по PL
        public void GetItems(string pl)
        {
            Items.Clear(); // Clear the items list // Очищаем список элементов
            try
            {
                OdbcConnection DbConnection = new OdbcConnection("DSN=Baan"); // Create ODBC connection to BAAN // Создаём ODBC соединение с BAAN
                DbConnection.Open(); // Open the connection // Открываем соединение
                OdbcCommand DbCommand = DbConnection.CreateCommand(); // Create command object // Создаём объект команды
                DbCommand.CommandTimeout = 180; // Set command timeout // Устанавливаем тайм-аут команды

                // Prepare SQL query to get items for the PL // Готовим SQL-запрос для получения элементов по PL
                DbCommand.CommandText = string.Format(@"SELECT tticst001400.t_sitm, tticst910400.t_pino, sum(tticst001400.t_qucs+tticst001400.t_issu+tticst001400.t_subd) as Estimate_Qty
                                                        FROM tticst910400
                                                        LEFT JOIN  ttisfc001400 ON tticst910400.t_pdno=ttisfc001400.t_pdno
                                                        LEFT JOIN  tticst001400 ON tticst910400.t_pdno=tticst001400.t_pdno
                                                        INNER JOIN ttiitm001400 ON ttiitm001400.t_item = tticst001400.t_sitm
                                                        WHERE tticst001400.t_opno=10 AND tticst910400.t_pino = '{0}' AND ttiitm001400.t_ctyp!='LBL' AND ttiitm001400.t_csgp NOT Like '%PCB%'
                                                        GROUP BY tticst001400.t_sitm, tticst910400.t_pino
                                                        ORDER BY tticst001400.t_sitm", pl);
                OdbcDataReader DbReader = DbCommand.ExecuteReader(); // Execute query and get reader // Выполняем запрос и получаем ридер

                while (DbReader.Read() && DbReader.GetValue(0) != DBNull.Value) // Read each row // Читаем каждую строку
                {
                    Item I = new Item(); // Create new Item object // Создаём новый объект Item
                    I.PN = DbReader.IsDBNull(0) ? "" : DbReader.GetString(0).Trim(); // Get part number // Получаем номер детали
                    I.PL = DbReader.IsDBNull(1) ? "" : DbReader.GetValue(1).ToString().Trim(); // Get PL // Получаем PL
                    I.Qty = DbReader.IsDBNull(2) ? 0 : Convert.ToInt32(DbReader.GetValue(2)); // Get quantity // Получаем количество
                    Items.Add(I); // Add item to list // Добавляем элемент в список
                }

                DbReader.Close(); // Close reader // Закрываем ридер
                DbCommand.Dispose(); // Dispose command // Освобождаем команду
                DbConnection.Close(); // Close connection // Закрываем соединение
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); // Show error message // Показываем сообщение об ошибке
            }
        }

        // Method to get lots for a given part number // Метод для получения лотов по номеру детали
        public List<LotPerPL> GetLots(string pn)
        {
            List<LotPerPL> Lots_List = new List<LotPerPL>(); // List to store lots // Список для хранения лотов

            string _pn = ""; // Variable for padded part number // Переменная для дополненного номера детали
            int n = 16 - pn.Length; // Calculate padding // Вычисляем количество пробелов для дополнения
            while (n > 0)
            {
                _pn += " "; // Add space // Добавляем пробел
                n--;
            }
            _pn += pn; // Append original part number // Добавляем исходный номер детали

            try
            {
                OdbcConnection DbConnection = new OdbcConnection("DSN=Baan"); // Create ODBC connection // Создаём ODBC соединение
                DbConnection.Open(); // Open connection // Открываем соединение
                OdbcCommand DbCommand = DbConnection.CreateCommand(); // Create command object // Создаём объект команды
                DbCommand.CommandTimeout = 180; // Set command timeout // Устанавливаем тайм-аут команды

                // Prepare SQL query to get lots for the part number // Готовим SQL-запрос для получения лотов по номеру детали
                DbCommand.CommandText = string.Format(@"SELECT INV.t_cwar, 
                                                       INV.t_loca, 
                                                       LOT.t_clot,
                                                       LOT.t_daco, 
                                                       INV.t_strs,
                                                       LOT.t_umid_c, 
                                                       ttcmcs830400.t_ptyp, 
                                                       LOT.t_lsup, 
                                                       LOT.t_suno, 
                                                       ttccom020400.t_nama,
                                                       ttdawh043400.t_slot,
                                                       ttdawh043400.t_cnum
                                                       FROM baandb.ttdilc101400 INV 
                                                       Left join baandb.ttdltc001400 LOT on LOT.t_clot = INV.t_clot and LOT.t_item= INV.t_item
                                                       Left join baandb.ttiitm200400 ttiitm200400 on ttiitm200400.t_item = INV.t_item	
                                                       Left join baandb.ttcmcs950400 ttcmcs950400 on ttcmcs950400.t_mnum = LOT.t_mnum		
                                                       Left join baandb.ttccom010400 ttccom010400 on ttccom010400.t_cuno = ttiitm200400.t_cuno 
                                                       Left join baandb.ttccom020400 ttccom020400 on ttccom020400.t_suno = LOT.t_suno
                                                       Left join baandb.ttcmcs830400 ttcmcs830400 on ttcmcs830400.t_pcod = LOT.t_pack_c 
                                                       LEFT JOIN baandb.ttdawh043400 ttdawh043400 ON ttdawh043400.t_clot =  LOT.t_clot AND ttdawh043400.t_cwar = INV.t_cwar
                                                       WHERE  INV.t_item = '{0}' AND INV.t_cwar!='MSC' AND INV.t_cwar!='RCH' AND INV.t_loca!=' INSPECT'
                                                       ORDER BY LOT.t_daco, LOT.t_clot", _pn);
                OdbcDataReader DbReader = DbCommand.ExecuteReader(); // Execute query and get reader // Выполняем запрос и получаем ридер

                while (DbReader.Read() && DbReader.GetValue(0) != DBNull.Value) // Read each row // Читаем каждую строку
                {
                    LotPerPL L = new LotPerPL(); // Create new LotPerPL object // Создаём новый объект LotPerPL
                    L.WH = DbReader.IsDBNull(0) ? "" : DbReader.GetString(0).Trim(); // Get warehouse // Получаем склад
                    L.Sec_WH = DbReader.IsDBNull(1) ? "" : DbReader.GetString(1).Trim(); // Get secondary warehouse // Получаем второй склад
                    L.PickEnabled = L.Sec_WH == "AUTOWH" ? true : false; // Enable pick if secondary warehouse is AUTOWH // Включаем отбор, если второй склад AUTOWH
                    L.Lot = DbReader.IsDBNull(2) ? "" : DbReader.GetValue(2).ToString().Trim(); // Get lot number // Получаем номер лота
                    L.DateCode = DbReader.IsDBNull(3) ? "" : DbReader.GetString(3).Trim(); // Get date code // Получаем код даты

                    // If date code is valid, extract month and year // Если код даты валиден, извлекаем месяц и год
                    if (L.DateCode.Length >= 4 && !L.DateCode.Contains(";"))
                    {
                        L.DC_Month = Convert.ToInt32(L.DateCode.Substring(0, 2)); // Get month // Получаем месяц
                        L.DC_Year = Convert.ToInt32(L.DateCode.Substring(2, 2)); // Get year // Получаем год
                    }
                    L.Qty = DbReader.IsDBNull(4) ? 0 : Convert.ToInt32(DbReader.GetValue(4)); // Get quantity // Получаем количество
                    L.Size = DbReader.IsDBNull(6) ? "" : DbReader.GetString(6).Trim(); // Get size // Получаем размер
                    L.Slot = DbReader.IsDBNull(10) ? "" : DbReader.GetValue(10).ToString().Trim(); // Get slot // Получаем слот
                    L.Cart = DbReader.IsDBNull(11) ? "" : DbReader.GetValue(11).ToString().Trim(); // Get cart // Получаем тележку

                    if (L.WH == "MH") // Only add lots from warehouse "MH" // Добавляем только лоты со склада "MH"
                        Lots_List.Add(L); // Add to the list // Добавляем в список
                }
                return Lots_List; // Return the list of lots // Возвращаем список лотов

                // The following lines are unreachable, but should close resources // Следующие строки недостижимы, но должны закрывать ресурсы
                DbReader.Close(); // Close reader // Закрываем ридер
                DbCommand.Dispose(); // Dispose command // Освобождаем команду
                DbConnection.Close(); // Close connection // Закрываем соединение
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); // Show error message // Показываем сообщение об ошибке
                return Lots_List; // Return the list (possibly empty) // Возвращаем список (возможно пустой)
            }
        }
    }
}