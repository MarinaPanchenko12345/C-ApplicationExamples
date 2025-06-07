using Sort_And_Scan.Classes; // Import SQLClass and other classes // Импорт SQLClass и других классов
using Sort_And_Scan.Windows; // Import windows for dialogs // Импорт окон для диалогов
using System; // Import base .NET types // Импорт базовых типов .NET
using System.Collections.Generic; // Import collections // Импорт коллекций
using System.Data; // Import ADO.NET data types // Импорт типов данных ADO.NET
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
using System.Windows.Navigation; // Import navigation // Импорт навигации
using System.Windows.Shapes; // Import shape drawing // Импорт рисования фигур

namespace Sort_And_Scan.Views // Namespace for organizing views // Пространство имён для организации представлений
{
    /// <summary>
    /// Interaction logic for Wh_Kitting.xaml // Логика взаимодействия для Wh_Kitting.xaml
    /// </summary>
    public partial class Wh_Kitting : UserControl // Partial class for Wh_Kitting user control // Частичный класс для пользовательского контрола Wh_Kitting
    {
        int SreelCounter = 0; // Counter for small reels // Счётчик для маленьких катушек
        int LreelCounter = 0; // Counter for large reels // Счётчик для больших катушек
        int tempSmallReel = 0; // Temporary counter for small reels // Временный счётчик для маленьких катушек
        int tempLargeReel = 0; // Temporary counter for large reels // Временный счётчик для больших катушек

        List<WhKittingModel> List = new List<WhKittingModel>(); // List to store WhKittingModel objects // Список для хранения объектов WhKittingModel

        // Default constructor for Wh_Kitting user control // Конструктор по умолчанию для пользовательского контрола Wh_Kitting
        public Wh_Kitting()
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus to Lot_Txt textbox // Устанавливаем фокус на текстовое поле Lot_Txt
        }

        // Constructor with PL parameter // Конструктор с параметром PL
        public Wh_Kitting(string pl)
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            PlTxt.Text = pl; // Set PL textbox value // Устанавливаем значение поля PL
            FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus to Lot_Txt textbox // Устанавливаем фокус на текстовое поле Lot_Txt
        }

        // Event handler for Clear button click // Обработчик нажатия кнопки очистки
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            // No implementation yet // Пока не реализовано
        }

        // Method to get part number and type from BAAN database by lot // Метод для получения номера детали и типа из базы BAAN по лоту
        private BAAN GetPnFromBaan(string lot)
        {
            string type = string.Empty; // Variable for type // Переменная для типа
            string pn = string.Empty; // Variable for part number // Переменная для номера детали
            OdbcConnection DbConnection = new OdbcConnection("DSN=Baan"); // Create ODBC connection to BAAN // Создаём ODBC соединение с BAAN
            DbConnection.Open(); // Open the connection // Открываем соединение
            OdbcCommand DbCommand = DbConnection.CreateCommand(); // Create command object // Создаём объект команды

            // SQL query to get item info by lot // SQL-запрос для получения информации по лоту
            string sql = string.Format(@"SELECT LOT.t_item, t_clot, LOT.t_stoc, t_umid_c, t_pack_c,ttcmcs830400.t_ptyp, t_ltor_c, itm200DB.t_cuno, Customer.t_nama
FROM baandb.ttdltc001400 LOT
LEFT JOIN baandb.ttiitm200400 itm200DB ON LOT.t_item = itm200DB.t_item
LEFT JOIN baandb.ttccom010400 Customer ON itm200DB.t_cuno = Customer.t_cuno
LEFT JOIN baandb.ttcmcs830400 ttcmcs830400 on ttcmcs830400.t_pcod = LOT.t_pack_c
 
WHERE t_clot = '      {0}'
 ", lot.Replace("\t","")); // Replace tabs in lot // Удаляем табуляции из лота
            DbCommand.CommandText = sql; // Set command text // Устанавливаем текст команды
            OdbcDataReader DbReader = DbCommand.ExecuteReader(); // Execute query and get reader // Выполняем запрос и получаем reader

            BAAN b = new BAAN(); // Create BAAN object // Создаём объект BAAN
            while (DbReader.Read()) // Read each row // Читаем каждую строку
            {
                b.Pn = DbReader.GetString(0).ToString().Trim(); // Get part number and trim // Получаем номер детали и обрезаем пробелы
                b.Type = DbReader.GetString(5).ToString().Trim(); // Get type and trim // Получаем тип и обрезаем пробелы
            }
            return b; // Return BAAN object // Возвращаем объект BAAN
        }

        // Event handler for PL textbox text change // Обработчик изменения текста в поле PL
        private void PlTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            // No implementation yet // Пока не реализовано
        }

        // Event handler for Save button click // Обработчик нажатия кнопки сохранения
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", ""); // Create SQLClass instance for DB operations // Создаём экземпляр SQLClass для работы с БД
            try
            {
                foreach (var item in List) // Loop through each item in the list // Проходим по каждому элементу в списке
                {
                    // Prepare SQL query to insert kitting data // Готовим SQL-запрос для вставки данных комплектации
                    string qry = string.Format(@"INSERT INTO Sort_And_Scan_Wh_Kitting (ID,item,Status,Pl ,Lot,Package,Date,Employee_Name) 
                                        VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                              item.Id, item.Item, item.Status, PlTxt.Text.Trim(),item.Lot,item.Package, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), MainWindow.UserName);
                    sql.Execute(qry); // Execute SQL command // Выполняем SQL-команду
                }
                MessageBox.Show("Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Asterisk); // Show success message // Показываем сообщение об успешном сохранении
                PlTxt.Text = ""; // Clear PL textbox // Очищаем поле PL
                PN_Txt.Text = ""; // Clear PN textbox // Очищаем поле PN
                Lot_Txt.Text = ""; // Clear Lot textbox // Очищаем поле Lot
                First_Txt.Text = ""; // Clear First textbox // Очищаем поле First
                Lot_Txt2.Text = ""; // Clear Lot_Txt2 textbox // Очищаем поле Lot_Txt2
                //PickLocation_Txt.Text = ""; // (Commented) Clear PickLocation textbox // (Закомментировано) Очищаем поле PickLocation
                PickLocation_Border.Background = Brushes.White; // Reset PickLocation border color // Сбрасываем цвет границы PickLocation
                First_Border.Background = Brushes.White; // Reset First border color // Сбрасываем цвет границы First
                LotBrd.Background = Brushes.White; // Reset Lot border color // Сбрасываем цвет границы Lot
                FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus to Lot_Txt // Устанавливаем фокус на Lot_Txt
                Grid5.ItemsSource = null; // Clear grid data source // Очищаем источник данных таблицы
                List.Clear(); // Clear the list // Очищаем список
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Asterisk); // Show error message // Показываем сообщение об ошибке
            }
        }

        // Event handler for Lot_Txt key up event // Обработчик события отпускания клавиши в Lot_Txt
        private void Lot_Txt_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                    if (!(e.Key == Key.Return))// Change
                    if (!(e.Key == Key.Tab || e.Key == Key.Space)) // Only process if Tab or Space is pressed // Обрабатываем только если нажаты Tab или Space
                {
                    return; // Exit if other key // Выходим, если другая клавиша
                }
          
                string lot = Lot_Txt.Text.Replace("\t", ""); // Remove tabs from lot // Удаляем табуляции из лота
                BAAN pn = GetPnFromBaan(lot); // Get part number and type from BAAN // Получаем номер детали и тип из BAAN
                if (pn.Pn == null) // If part number not found // Если номер детали не найден
                {
                    PN_Txt.Text = ""; // Clear PN textbox // Очищаем поле PN
                    Lot_Txt.Text = ""; // Clear Lot textbox // Очищаем поле Lot
                    First_Txt.Text = ""; // Clear First textbox // Очищаем поле First
                    Lot_Txt2.Text = ""; // Clear Lot_Txt2 textbox // Очищаем поле Lot_Txt2
                    //PickLocation_Txt.Text = ""; // (Commented) Clear PickLocation textbox // (Закомментировано) Очищаем поле PickLocation
                    PickLocation_Border.Background = Brushes.White; // Reset PickLocation border color // Сбрасываем цвет границы PickLocation
                    First_Border.Background = Brushes.White; // Reset First border color // Сбрасываем цвет границы First
                    LotBrd.Background = Brushes.White; // Reset Lot border color // Сбрасываем цвет границы Lot
                    FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus to Lot_Txt // Устанавливаем фокус на Lot_Txt
                    return; // Exit method // Выходим из метода
                }

                if (List.Any(x => x.Lot == lot)) // If lot already exists in the list // Если лот уже есть в списке
                {
                    MessageBox.Show("Lot Already Exist", "", MessageBoxButton.OK, MessageBoxImage.Warning); // Show warning // Показываем предупреждение
                    Lot_Txt.Text = ""; // Clear Lot textbox // Очищаем поле Lot
                    return; // Exit method // Выходим из метода
                }
                if (pn.Type.Contains("S")) // If type is small reel // Если тип маленькая катушка
                {
                    WhKittingModel w = new WhKittingModel(); // Create new WhKittingModel // Создаём новый WhKittingModel

                    if (List.Any(x => x.Item == pn.Pn)) // If item already exists in the list // Если элемент уже есть в списке
                    {
                        if(!List.Any(x=>x.Status== "Backup")) // If no backup exists // Если нет резервного
                        {
                            SreelCounter = 1; // Set counter to 1 // Устанавливаем счётчик в 1
                        }
                        else
                        {
                            SreelCounter++; // Increment counter // Увеличиваем счётчик
                        }

                        Lot_Txt2.Text = lot; // Set Lot_Txt2 textbox // Устанавливаем поле Lot_Txt2
                        PN_Txt.Text = pn.Pn; // Set PN textbox // Устанавливаем поле PN
                        w.Id = SreelCounter; // Set ID // Устанавливаем ID
                        w.Lot = lot; // Set lot // Устанавливаем лот
                        w.Status = "Backup"; // Set status as Backup // Устанавливаем статус как Backup
                        First_Txt.Text = "Backup"; // Set First textbox // Устанавливаем поле First
                        PickLocation_Border.Background = Brushes.LightSkyBlue; // Set PickLocation border color // Устанавливаем цвет границы PickLocation
                        LotBrd.Background = Brushes.LightSkyBlue; // Set Lot border color // Устанавливаем цвет границы Lot
                        First_Border.Background = Brushes.LightSkyBlue; // Set First border color // Устанавливаем цвет границы First
                    }
                    else
                    {
                        tempSmallReel++; // Increment temporary small reel counter // Увеличиваем временный счётчик маленьких катушек
                        Lot_Txt2.Text = lot; // Set Lot_Txt2 textbox // Устанавливаем поле Lot_Txt2
                        PN_Txt.Text = pn.Pn; // Set PN textbox // Устанавливаем поле PN
                        w.Id = tempSmallReel; // Set ID // Устанавливаем ID
                        w.Lot = lot; // Set lot // Устанавливаем лот
                        w.Status = "First"; // Set status as First // Устанавливаем статус как First
                        First_Txt.Text = "First"; // Set First textbox // Устанавливаем поле First
                        LotBrd.Background = Brushes.LightGreen; // Set Lot border color // Устанавливаем цвет границы Lot
                        PickLocation_Border.Background = Brushes.LightGreen; // Set PickLocation border color // Устанавливаем цвет границы PickLocation
                        First_Border.Background = Brushes.LightGreen; // Set First border color // Устанавливаем цвет границы First
                    }
                    w.Item = pn.Pn; // Set item // Устанавливаем элемент
                    w.Package = pn.Type; // Set package type // Устанавливаем тип упаковки
                    List.Add(w); // Add to list // Добавляем в список
                  
                    Lot_Txt.Text = ""; // Clear Lot textbox // Очищаем поле Lot
                    FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus to Lot_Txt // Устанавливаем фокус на Lot_Txt
                    Grid5.ItemsSource = List.ToList(); // Update grid data source // Обновляем источник данных таблицы
                }
                else if (pn.Type.Contains("L")) // If type is large reel // Если тип большая катушка
                {
                    WhKittingModel w = new WhKittingModel(); // Create new WhKittingModel // Создаём новый WhKittingModel          
                    if (List.Any(x => x.Item == pn.Pn)) // If item already exists in the list // Если элемент уже есть в списке
                    {
                        if (!List.Any(x => x.Status == "Backup" )) // If no backup exists // Если нет резервного
                        {
                            LreelCounter = 1; // Set counter to 1 // Устанавливаем счётчик в 1
                        }
                        else
                        {
                            LreelCounter++; // Increment counter // Увеличиваем счётчик
                        }
                        PN_Txt.Text = pn.Pn; // Set PN textbox // Устанавливаем поле PN
                        Lot_Txt2.Text = lot; // Set Lot_Txt2 textbox // Устанавливаем поле Lot_Txt2
                        w.Id = LreelCounter; // Set ID // Устанавливаем ID
                        w.Lot = lot; // Set lot // Устанавливаем лот
                        w.Status = "Backup"; // Set status as Backup // Устанавливаем статус как Backup
                        First_Txt.Text = "Backup"; // Set First textbox // Устанавливаем поле First
                        LotBrd.Background = Brushes.LightSkyBlue; // Set Lot border color // Устанавливаем цвет границы Lot
                        PickLocation_Border.Background = Brushes.LightSkyBlue; // Set PickLocation border color // Устанавливаем цвет границы PickLocation
                        First_Border.Background = Brushes.LightSkyBlue; // Set First border color // Устанавливаем цвет границы First
                    }
                    else
                    {
                        tempLargeReel++; // Increment temporary large reel counter // Увеличиваем временный счётчик больших катушек
                        PN_Txt.Text = pn.Pn; // Set PN textbox // Устанавливаем поле PN
                        Lot_Txt2.Text = lot; // Set Lot_Txt2 textbox // Устанавливаем поле Lot_Txt2
                        w.Id = tempLargeReel; // Set ID // Устанавливаем ID
                        w.Lot = lot; // Set lot // Устанавливаем лот
                        w.Status = "First"; // Set status as First // Устанавливаем статус как First
                        First_Txt.Text = "First"; // Set First textbox // Устанавливаем поле First
                        LotBrd.Background = Brushes.LightGreen; // Set Lot border color // Устанавливаем цвет границы Lot
                        PickLocation_Border.Background = Brushes.LightGreen; // Set PickLocation border color // Устанавливаем цвет границы PickLocation
                        First_Border.Background = Brushes.LightGreen; // Set First border color // Устанавливаем цвет границы First
                    }
                    w.Item = pn.Pn; // Set item // Устанавливаем элемент
                    w.Package = pn.Type; // Set package type // Устанавливаем тип упаковки
                    List.Add(w); // Add to list // Добавляем в список
                   
                    Lot_Txt.Text = ""; // Clear Lot textbox // Очищаем поле Lot
                    FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus to Lot_Txt // Устанавливаем фокус на Lot_Txt
                    Grid5.ItemsSource = List.ToList(); // Update grid data source // Обновляем источник данных таблицы
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message); // Show error message // Показываем сообщение об ошибке
            }
        }

        // Event handler for Component_Txt key up event // Обработчик события отпускания клавиши в Component_Txt
        private void Component_Txt_KeyUp(object sender, KeyEventArgs e)
        {
            List<WhKittingModel> whKittingsList = new List<WhKittingModel>(); // List to store results // Список для хранения результатов
            string pn = Component_Txt.Text.Replace("\t","").Trim().ToString(); // Get part number from textbox // Получаем номер детали из текстового поля
            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", ""); // Create SQLClass instance for DB operations // Создаём экземпляр SQLClass для работы с БД

            string qry = string.Format(@"SELECT * FROM Sort_And_Scan_Wh_Kitting WHERE item='{0}'", pn); // Prepare SQL query // Готовим SQL-запрос
            DataTable DT = sql.GetDataTable(qry); // Execute query and get results // Выполняем запрос и получаем результаты

            foreach (DataRow row in DT.Rows) // Loop through each row in result // Проходим по каждой строке результата
            {
                WhKittingModel k = new WhKittingModel(); // Create new WhKittingModel // Создаём новый WhKittingModel
                k.Id = Convert.ToInt32(row["ID"].ToString()); // Set ID // Устанавливаем ID
                k.Item = row["Item"].ToString(); // Set item // Устанавливаем элемент
                k.Status = row["Status"].ToString(); // Set status // Устанавливаем статус
                k.PL = row["PL"].ToString(); // Set PL // Устанавливаем PL
                k.Lot = row["LOT"].ToString(); // Set lot // Устанавливаем лот
                whKittingsList.Add(k); // Add to result list // Добавляем в список результатов
            }

            Grid5.ItemsSource = whKittingsList.ToList(); // Update grid data source // Обновляем источник данных таблицы
        }

        // Event handler for PL button click // Обработчик нажатия кнопки PL
        private void plBtn_Click(object sender, RoutedEventArgs e)
        {
            PlWindow plWindow = new PlWindow(); // Create new PL window // Создаём новое окно PL
            plWindow.Show(); // Show PL window // Показываем окно PL
        }
    }
}