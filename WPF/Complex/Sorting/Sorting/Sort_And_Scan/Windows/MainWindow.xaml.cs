using Sort_And_Scan.Classes; // Import project classes // Импорт классов проекта
using Sort_And_Scan.Views; // Import view controls // Импорт контролов представлений
using System; // Import base .NET types // Импорт базовых типов .NET
using System.Collections.Generic; // Import collections // Импорт коллекций
using System.Data; // Import ADO.NET data types // Импорт типов данных ADO.NET
using System.Data.Odbc; // Import ODBC for database access // Импорт ODBC для доступа к базе данных
using System.IO; // Import file IO // Импорт работы с файлами
using System.Linq; // Import LINQ // Импорт LINQ
using System.Threading.Tasks; // Import async utilities // Импорт утилит для асинхронности
using System.Windows; // Import WPF UI elements // Импорт элементов WPF UI
using System.Windows.Controls; // Import WPF controls // Импорт WPF контролов
using System.Windows.Input; // Import input handling // Импорт обработки ввода
using System.Windows.Media.Imaging; // Import image handling // Импорт работы с изображениями
using System.Windows.Threading; // Import dispatcher timer // Импорт таймера диспетчера

namespace Sort_And_Scan.Windows // Namespace for organizing windows // Пространство имён для организации окон
{
    public partial class MainWindow : Window // Main application window // Главное окно приложения
    {
        public static string UserName = ""; // Stores the current user's name // Хранит имя текущего пользователя
        public static string UserId = ""; // Stores the current user's ID // Хранит ID текущего пользователя
        public static MainWindow mn; // Static reference to the main window instance // Статическая ссылка на экземпляр главного окна
        public static List<DBRow> MainList = new List<DBRow>(); // Main list of DBRow objects // Основной список объектов DBRow
        List<DBRow> PCBList = new List<DBRow>(); // List for PCB rows // Список для строк PCB
        List<SiplaceRow> SiplaceList = new List<SiplaceRow>(); // List for Siplace rows // Список для строк Siplace
        List<string> MultipleSetupList = new List<string>(); // List for multiple setups // Список для множественных настроек
        DispatcherTimer Timer = new DispatcherTimer(); // Timer for background tasks // Таймер для фоновых задач
        DispatcherTimer Timer2 = new DispatcherTimer(); // Second timer for UI updates // Второй таймер для обновления UI
        Task t; // Background task for loading data // Фоновая задача для загрузки данных
        int counter = 1; // Counter for timer // Счётчик для таймера
        public static bool IsShortageBtnPressed = false; // Flag for shortage button // Флаг для кнопки нехватки
        public static PL PL = new PL(); // Static PL window instance // Статический экземпляр окна PL

        // Constructor for MainWindow // Конструктор для MainWindow
        public MainWindow()
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            mn = this; // Assign this instance to static reference // Присваиваем этот экземпляр статической ссылке
            Timer.Tick += Timer_Tick; // Attach event handler for Timer // Привязываем обработчик событий к Timer
            Timer.Interval = new TimeSpan(0, 0, 1); // Set timer interval to 1 second // Устанавливаем интервал таймера 1 секунда
            Timer2.Tick += Timer_Tick2; // Attach event handler for Timer2 // Привязываем обработчик событий к Timer2
            Timer2.Interval = new TimeSpan(0, 0, 1); // Set timer2 interval to 1 second // Устанавливаем интервал таймера 1 секунда
        }

        // Timer tick event handler for background data loading // Обработчик тика таймера для фоновой загрузки данных
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (t.IsCompleted) // If background task is completed // Если фоновая задача завершена
            {
                ViewProgressBar.Visibility = Visibility.Collapsed; // Hide progress bar // Скрываем индикатор выполнения
                Timer.Stop(); // Stop the timer // Останавливаем таймер
                if (MainList.Count > 0) // If there are items in the main list // Если в основном списке есть элементы
                {
                    Menu.IsEnabled = true; // Enable the menu // Включаем меню
                    ViewSP.Visibility = Visibility.Visible; // Show main view stack panel // Показываем основной стек-панель
                    WelcomeGrid.Visibility = Visibility.Collapsed; // Hide welcome grid // Скрываем приветственный экран
                    PLDetailsGrid.Visibility = Visibility.Visible; // Show PL details grid // Показываем сетку деталей PL
                    LineTxt.Text = SiplaceList.Count > 0 ? SiplaceList[0].Station.Substring(SiplaceList[0].Station.Length - 1) : ""; // Set line text // Устанавливаем текст линии
                    CustomerTxt.Text = MainList[0].Customer; // Set customer text // Устанавливаем текст заказчика
                    WHTxt.Text = MainList[0].WH; // Set warehouse text // Устанавливаем текст склада
                    MainList = MainList.OrderBy(x => x.ID).ThenBy(x => x.Classification.Contains("First") ? 0 : 1).ThenBy(x => x.Date).ThenBy(x => x.Lot).ToList(); // Sort main list // Сортируем основной список
                    MainList.AddRange(PCBList); // Add PCB rows to main list // Добавляем строки PCB в основной список
                    ViewSP.Children.Clear(); // Clear view stack panel // Очищаем стек-панель
                    ViewSP.Children.Add(new Main(MainList)); // Add Main user control with main list // Добавляем контрол Main с основным списком
                    GridCursor.Margin = new Thickness(10 + (180 * 0), 40, 0, 0); // Set grid cursor position // Устанавливаем позицию курсора сетки
                }
                else
                {
                    MessageBox.Show("No Items", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); // Show warning if no items // Показываем предупреждение, если нет элементов
                }
            }
        }

        // Timer2 tick event handler for UI transitions // Обработчик тика второго таймера для переходов UI
        private void Timer_Tick2(object sender, EventArgs e)
        {
            counter--; // Decrement counter // Уменьшаем счётчик
            if (counter == 0) // If counter reaches zero // Если счётчик достиг нуля
            {
                ViewProgressBar.Visibility = Visibility.Collapsed; // Hide progress bar // Скрываем индикатор выполнения
                Timer2.Stop(); // Stop timer2 // Останавливаем второй таймер
                counter = 1; // Reset counter // Сбрасываем счётчик
                ViewSP.Visibility = Visibility.Visible; // Show main view stack panel // Показываем основной стек-панель
                Mouse.OverrideCursor = null; // Reset mouse cursor // Сбрасываем курсор мыши
            }
        }

        // Event handler for menu button clicks // Обработчик нажатия кнопок меню
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait; // Set mouse cursor to wait // Устанавливаем курсор мыши в режим ожидания
            int index = int.Parse(((Button)e.Source).Uid); // Get index from button UID // Получаем индекс из UID кнопки
            GridCursor.Margin = new Thickness(10 + (180 * index), 40, 0, 0); // Move grid cursor // Перемещаем курсор сетки
            ViewSP.Children.Clear(); // Clear view stack panel // Очищаем стек-панель
            ViewProgressBar.Visibility = Visibility.Visible; // Show progress bar // Показываем индикатор выполнения
            ViewSP.Visibility = Visibility.Collapsed; // Hide main view stack panel // Скрываем основной стек-панель

            Timer2.Start(); // Start timer2 for UI transition // Запускаем второй таймер для перехода UI

            switch (index) // Switch by menu index // Переключаемся по индексу меню
            {
                case 0:
                    ViewSP.Children.Add(new Main(MainList)); // Show Main view // Показываем Main
                    break;
                case 1:
                    ViewSP.Children.Add(new SortAndScan(MainList.OrderBy(x => x.ID).ThenBy(x => x.Classification.Contains("First") ? 0 : 1).ThenBy(x => x.Date).ThenBy(x => x.Lot).ToList())); // Show SortAndScan view // Показываем SortAndScan
                    break;
                case 2:
                    ViewSP.Children.Add(new UnScanned(MainList.Where(x => x.Status == "Not Scanned").OrderBy(x => x.ID).ThenBy(x => x.Classification.Contains("First") ? 0 : 1).ThenBy(x => x.Date).ThenBy(x => x.Lot).ToList())); // Show UnScanned view // Показываем UnScanned
                    break;
                case 3:
                    break; // Reserved for future use // Зарезервировано для будущего использования
                case 4:
                    break; // Reserved for future use // Зарезервировано для будущего использования
                case 5:
                    ViewSP.Children.Add(new FinishedSetups()); // Show FinishedSetups view // Показываем FinishedSetups
                    break;
                case 6:
                    PL = new PL(); // Create new PL window // Создаём новое окно PL
                    bool? result = PL.ShowDialog(); // Show PL dialog // Показываем диалог PL
                    if (result.HasValue && result.Value)
                    {
                        mn.ViewSP.Children.Add(new Wh_Kitting(PL.pl)); // Show Wh_Kitting view if dialog result is OK // Показываем Wh_Kitting если результат диалога OK
                    }
                    break;
            }
        }

        // Event handler for login button click // Обработчик нажатия кнопки входа
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (UserName != "") // If user is already logged in // Если пользователь уже вошёл
            {
                Logout(); // Log out current user // Выходим из системы
            }
            else
            {
                Login log = new Login(); // Create login window // Создаём окно входа
                log.Owner = this; // Set owner window // Устанавливаем владельца окна
                bool? result = log.ShowDialog(); // Show login dialog // Показываем диалог входа
                if ((result != null) && (result == true)) // If login successful // Если вход успешен
                {
                    DetailsSP.Visibility = Visibility.Visible; // Show details panel // Показываем панель деталей
                    User_Details_Panel.Visibility = Visibility.Visible; // Show user details panel // Показываем панель данных пользователя
                    LoginBtn.Visibility = Visibility.Collapsed; // Hide login button // Скрываем кнопку входа
                    NameTxt.Text = UserName; // Display user name // Отображаем имя пользователя
                    BitmapImage img = GetPhoto(UserId); // Get user photo // Получаем фото пользователя
                    ProfileImg.ImageSource = img; // Set profile image // Устанавливаем изображение профиля
                    WelcomeGrid.Visibility = Visibility.Collapsed; // Hide welcome grid // Скрываем приветственный экран
                    ViewSP.Visibility = Visibility.Visible; // Show main view // Показываем основной вид
                    FinishedBtn.IsEnabled = true; // Enable finished button // Включаем кнопку завершения
                    WhKittingBtn.IsEnabled = true; // Enable WhKitting button // Включаем кнопку WhKitting
                }
                FocusManager.SetFocusedElement(this, PLTxt); // Set focus to PL textbox // Устанавливаем фокус на поле PL
            }
        }

        // Method to log out the current user // Метод для выхода текущего пользователя
        private void Logout()
        {
            UserName = ""; // Clear user name // Очищаем имя пользователя
            UserId = ""; // Clear user ID // Очищаем ID пользователя
            ProfileImg.ImageSource = null; // Clear profile image // Очищаем изображение профиля
            User_Details_Panel.Visibility = Visibility.Collapsed; // Hide user details panel // Скрываем панель данных пользователя
            LoginBtn.Visibility = Visibility.Visible; // Show login button // Показываем кнопку входа
            DetailsSP.Visibility = Visibility.Collapsed; // Hide details panel // Скрываем панель деталей
            Menu.IsEnabled = false; // Disable menu // Отключаем меню
            FinishedBtn.IsEnabled = false; // Disable finished button // Отключаем кнопку завершения
            ViewSP.Visibility = Visibility.Collapsed; // Hide main view // Скрываем основной вид
            ViewProgressBar.Visibility = Visibility.Collapsed; // Hide progress bar // Скрываем индикатор выполнения
            WelcomeGrid.Visibility = Visibility.Visible; // Show welcome grid // Показываем приветственный экран
        }

        // Event handler for exit button click // Обработчик нажатия кнопки выхода
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Close the application // Закрываем приложение
        }

        // Method to get the photo of the operator by user ID // Метод для получения фото оператора по ID пользователя
        private BitmapImage GetPhoto(string p)
        {
            string path = @"\\mignt002\Private\falcon\" + p + ".jpg"; // Path to user photo // Путь к фото пользователя
            BitmapImage bitmap = new BitmapImage(); // Create bitmap image // Создаём изображение bitmap
            if (File.Exists(path)) // If photo file exists // Если файл фото существует
            {
                bitmap.BeginInit(); // Begin image initialization // Начинаем инициализацию изображения
                bitmap.UriSource = new Uri(path); // Set image URI // Устанавливаем URI изображения
                bitmap.EndInit(); // End image initialization // Завершаем инициализацию изображения
            }
            else
            {
                bitmap.BeginInit(); // Begin image initialization // Начинаем инициализацию изображения
                bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/profile.jpg", UriKind.Absolute); // Use default profile image // Используем изображение профиля по умолчанию
                bitmap.EndInit(); // End image initialization // Завершаем инициализацию изображения
            }
            return bitmap; // Return bitmap image // Возвращаем изображение bitmap
        }

        // Event handler for Go button click // Обработчик нажатия кнопки Go
        private void Go_Click(object sender, RoutedEventArgs e)
        {
            GO(); // Call GO method // Вызываем метод GO
        }

        // Event handler for PL textbox key down // Обработчик нажатия клавиши в поле PL
        private void PLTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Tab) // If Enter or Tab is pressed // Если нажата Enter или Tab
            {
                GO(); // Call GO method // Вызываем метод GO
            }
        }

        // Main method to start loading data for a PL // Основной метод для загрузки данных по PL
        private void GO()
        {
            if (PLTxt.Text.Trim().Length != 6) // If PL is not 6 characters // Если PL не 6 символов
            {
                MessageBox.Show("Insert Valid PL", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); // Show warning // Показываем предупреждение
                return; // Exit method // Выходим из метода
            }

            string PL = PLTxt.Text.Trim(); // Get PL value // Получаем значение PL
            List<DBRow> IPlc_list = CheckIPLC(PL); // Check for IPLC // Проверяем IPLC
            string Setup = CheckMultipleSetup(PL); // Check for multiple setups // Проверяем множественные настройки
            if (Setup == "") // If setup not found // Если настройка не найдена
            {
                MessageBox.Show("PL Not Exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); // Show warning // Показываем предупреждение
                return; // Exit method // Выходим из метода
            }
            SetupTxt.Text = Setup; // Set setup text // Устанавливаем текст настройки

            MainList.Clear(); // Clear main list // Очищаем основной список
            SiplaceList.Clear(); // Clear Siplace list // Очищаем список Siplace
            PCBList.Clear(); // Clear PCB list // Очищаем список PCB
            ViewProgressBar.Visibility = Visibility.Visible; // Show progress bar // Показываем индикатор выполнения
            ViewSP.Visibility = Visibility.Collapsed; // Hide main view // Скрываем основной вид

            // Start background task to load data // Запускаем фоновую задачу для загрузки данных
            t = Task.Factory.StartNew(() =>
            {
                SiplaceList = GetDataFromSiplace(Setup); // Load Siplace data // Загружаем данные Siplace
                GetDataFromBaan(PL, Setup, IPlc_list); // Load BAAN data // Загружаем данные BAAN
                SetClassification(); // Set classification for rows // Устанавливаем классификацию для строк
            });
            Timer.Start(); // Start timer to monitor task // Запускаем таймер для отслеживания задачи
        }

        private List<SiplaceRow> GetDataFromSiplace(string Setup)
        {
            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", "");
            List<SiplaceRow> list = new List<SiplaceRow>();

            string sqlStr = string.Format(@"SELECT DISTINCT 
                         TOP (100) PERCENT AliasName_2.ObjectName AS PN, AliasName_1.ObjectName AS Station, dbo.CHeadSchedule.lHeadIndex AS Location, dbo.CPickupLink.lTrack AS Track, dbo.CPickupLink.lReserve AS Division, 
                         dbo.CPickupLink.lTower AS Tower, dbo.CPickupLink.lLevel AS [Level], AliasName_3.ObjectName AS Setup, dbo.CPickupLink.nLocation
                         FROM  dbo.CRecipe INNER JOIN
                         dbo.AliasName ON dbo.CRecipe.OID = dbo.AliasName.PID INNER JOIN
                         dbo.CHeadSchedule ON dbo.CRecipe.OID = dbo.CHeadSchedule.PID INNER JOIN
                         dbo.AliasName AS AliasName_1 ON dbo.CHeadSchedule.spStation = AliasName_1.PID INNER JOIN
                         dbo.CHeadStep ON dbo.CHeadSchedule.OID = dbo.CHeadStep.PID INNER JOIN
                         dbo.CPickupLink ON dbo.CRecipe.OID = dbo.CPickupLink.PID AND dbo.CHeadStep.lPickupLink = dbo.CPickupLink.lIndex INNER JOIN
                         dbo.AliasName AS AliasName_2 ON dbo.CPickupLink.spComponentRef = AliasName_2.PID INNER JOIN
                         dbo.CPlacementLink ON dbo.CRecipe.OID = dbo.CPlacementLink.PID AND dbo.CHeadStep.lPlacementLink = dbo.CPlacementLink.lIndex INNER JOIN
                         dbo.CComponentPlacement ON dbo.CPlacementLink.spComponentPlacement = dbo.CComponentPlacement.OID INNER JOIN
                         dbo.AliasName AS AliasName_3 ON dbo.CRecipe.spSetupRef = AliasName_3.PID
                         WHERE (AliasName_3.ObjectName LIKE '%{0}%') 
                         ORDER BY dbo.CPickupLink.lTrack ASC, dbo.CPickupLink.lReserve ASC", Setup);
            DataTable DT1 = sql.GetDataTable(sqlStr);
            list = (from rw in DT1.AsEnumerable()
                    select new SiplaceRow()
                    {
                        Item = rw["PN"].ToString()?.Trim(),
                        Station = rw["Station"].ToString()?.Trim(),
                        StationNumber = rw["Station"].ToString()?.Trim().Substring(4, 1),
                        Location = CheckLocBySiplace(rw["Station"].ToString()?.Trim(), rw["nLocation"].ToString()?.Trim(), Convert.ToInt32(rw["Division"].ToString()?.Trim()), out int num),
                        numOfStation = num,
                        Track = rw["Track"].ToString()?.Trim(),
                        Division = Convert.ToInt32(rw["Division"].ToString()?.Trim()) + 1,
                        Tower = rw["Tower"].ToString()?.Trim(),
                        Level = rw["Level"].ToString()?.Trim(),
                        Setup = rw["Setup"].ToString()?.Trim(),
                    }).ToList();
            foreach (SiplaceRow row in list)
            {
                if (string.IsNullOrWhiteSpace(row.PickLocation_One))
                {
                    List<SiplaceRow> temp = list.Where(x => x.Item == row.Item).ToList();
                    string loc1 = "";
                    string loc2 = "";
                    string loc3 = "";

                    int i = 1;

                    foreach (SiplaceRow t in temp)
                    {
                        if (i == 1)
                        {
                            if (t.Tower == "0")
                                loc1 = "S:" + t.StationNumber + "/" + t.Location + " Tr:" + t.Track + " D:" + t.Division;
                            else
                                loc1 = "S:" + t.StationNumber + "/" + t.Location + " T:" + t.Tower + " L:" + t.Level;
                        }
                        else if (i == 2)
                        {
                            if (t.Tower == "0")
                                loc2 = "S:" + t.StationNumber + "/" + t.Location + " Tr:" + t.Track + " D:" + t.Division;
                            else
                                loc2 = "S:" + t.StationNumber + "/" + t.Location + " T:" + t.Tower + " L:" + t.Level;
                        }
                        else if (i == 3)
                        {
                            if (t.Tower == "0")
                                loc3 = "S:" + t.StationNumber + "/" + t.Location + " Tr:" + t.Track + " D:" + t.Division;
                            else
                                loc3 = "S:" + t.StationNumber + "/" + t.Location + " T:" + t.Tower + " L:" + t.Level;
                        }
                        i++;
                    }

                    temp.ForEach(x => { x.PickLocation_One = loc1; x.PickLocation_Tow = loc2; x.PickLocation_Three = loc3; });
                }
            }
            return list;
        }

        private string CheckLocBySiplace(string Station, string Loc, int div, out int num)
        {
            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", "");
            string NewLoc = "1";
            string sqlStr = string.Format(@"SELECT distinct AliasName_1.ObjectName AS Station, dbo.CHeadSchedule.lHeadIndex AS Location
                                   FROM dbo.CRecipe INNER JOIN
                                   dbo.AliasName ON dbo.CRecipe.OID = dbo.AliasName.PID INNER JOIN
                                   dbo.CHeadSchedule ON dbo.CRecipe.OID = dbo.CHeadSchedule.PID INNER JOIN
                                   dbo.AliasName AS AliasName_1 ON dbo.CHeadSchedule.spStation = AliasName_1.PID INNER JOIN
                                   dbo.CHeadStep ON dbo.CHeadSchedule.OID = dbo.CHeadStep.PID 
								   where AliasName_1.ObjectName = '{0}'
								   order by AliasName_1.ObjectName asc", Station);
            DataTable DT = sql.GetDataTable(sqlStr);
            num = DT.Rows.Count;

            if (num == 1)
                NewLoc = Loc == "1" ? "1" : Loc == "0" ? "2" : Loc;
            else if (num == 2)
                NewLoc = Loc == "1" ? "1" : Loc == "0" ? "2" : Loc;
            else if (num == 4 || num == 3)
                NewLoc = Loc == "1" ? "1" : Loc == "2" ? "3" : Loc == "3" ? "2" : Loc == "0" ? "4" : Loc;

            return NewLoc;
        }

        private void GetDataFromBaan(string PL, string Setup, List<DBRow> IPlc_list)
        {
            MainList.Clear();
            PCBList.Clear();
            List<SiplaceRow> SiplaceList2 = new List<SiplaceRow>();

            if (MultipleSetupList.Count > 1 && Setup != MultipleSetupList[0])//only for setup 2
                SiplaceList2 = GetDataFromSiplace(MultipleSetupList[0]);

            using (OdbcConnection DbConnection = new OdbcConnection("DSN=BAAN"))
            {
                DbConnection.Open();
                OdbcCommand DbCommand = DbConnection.CreateCommand();
                string q = string.Format(@"
SELECT DISTINCT ttdilc101400.t_item, ttdilc101400.t_clot, ttdilc101400.t_cwar,
                                            ttcmcs830400.t_ptyp, ttdltc001400.t_daco, ttdltc001400.t_oudt, ttccom010400.t_nama,ttdilc101400.t_strs
                                            FROM baandb.ttdilc101400 ttdilc101400
                                            LEFT JOIN baandb.ttdltc001400 ttdltc001400 ON ttdltc001400.t_clot = ttdilc101400.t_clot
                                            LEFT JOIN baandb.ttcmcs830400 ttcmcs830400 ON ttcmcs830400.t_pcod = ttdltc001400.t_pack_c
                                            LEFT JOIN baandb.ttdinv001400 ttdinv001400 ON ttdinv001400.t_item = ttdilc101400.t_item AND ttdinv001400.t_cwar = ttdilc101400.t_cwar 
                                            LEFT JOIN baandb.ttdawh043400 ttdawh043400 ON ttdawh043400.t_clot = ttdilc101400.t_clot AND ttdawh043400.t_cwar = ttdilc101400.t_cwar 
                                            LEFT JOIN baandb.ttiitm001400 ttiitm001400 ON ttdilc101400.t_item = ttiitm001400.t_item
                                            LEFT JOIN baandb.tticst910400 tticst910400 ON tticst910400.t_twar = ttdilc101400.t_cwar
                                            LEFT JOIN baandb.ttiitm200400 ttiitm200400 ON ttiitm200400.t_item = ttiitm001400.t_item
                                            LEFT JOIN baandb.ttccom010400 ttccom010400 ON ttccom010400.t_cuno = ttiitm200400.t_cuno                                            
                                            WHERE tticst910400.t_pino = '{0}' 
                                            ORDER BY ttdilc101400.t_item,ttdltc001400.t_oudt , ttdilc101400.t_clot ASC", PL);

                DbCommand.CommandText = q;
                try
                {
                    OdbcDataReader DbReader = DbCommand.ExecuteReader();

                    int i = 0;
                    string item = "";
                    string bcolor = "White";

                    while (DbReader.Read() && DbReader.GetValue(0) != DBNull.Value)
                    {
                        DBRow row = new DBRow();
                        row.Item = DbReader.IsDBNull(0) ? "" : DbReader.GetValue(0).ToString()?.Trim();
                        row.Lot = DbReader.IsDBNull(1) ? "" : DbReader.GetValue(1).ToString()?.Trim();
                        row.WH = DbReader.IsDBNull(2) ? "" : DbReader.GetValue(2).ToString()?.Trim();
                        row.Package_Type = DbReader.IsDBNull(3) ? "" : DbReader.GetValue(3).ToString()?.Trim();

                        row.DateCode = DbReader.IsDBNull(4) ? "0" : DbReader.GetValue(4).ToString()=="" ? "0" : DbReader.GetValue(4).ToString().Substring(0, 3);
                        if (row.DateCode == "0")
                        {
                            row.DateCode = row.Lot.Substring(2, 3) + row.Lot.Substring(0, 1);
                        }
                        row.Date = DbReader.GetDateTime(5);
                        row.Customer = DbReader.IsDBNull(6) ? "" : DbReader.GetValue(6).ToString()?.Trim();
                        row.Qty = DbReader.IsDBNull(7) ? 0 : double.Parse(DbReader.GetValue(7).ToString());
                        row.Status = "";
                        row.Classification = "";
                     


                        string PN = row.Item;
                        if (PN.Equals("241614077I"))
                        {
                            Console.Write(PN);
                        }
                        if (SiplaceList.Exists(x => x.Item == PN ||
                        ((row.Customer.ToLower().Contains("rafael") || row.Customer.ToLower().Contains("c4")) && PN.Substring(PN.IndexOf("-") + 1) == x.Item.Substring(x.Item.IndexOf("-") + 1))))
                        {
                            row.PickLocation_One = SiplaceList.FirstOrDefault(x => x.Item == PN || ((row.Customer.ToLower().Contains("rafael") || row.Customer.ToLower().Contains("c4")) && PN.Substring(PN.IndexOf("-") + 1) == x.Item.Substring(x.Item.IndexOf("-") + 1))).PickLocation_One;
                            row.PickLocation_Tow = SiplaceList.FirstOrDefault(x => x.Item == PN || ((row.Customer.ToLower().Contains("rafael") || row.Customer.ToLower().Contains("c4")) && PN.Substring(PN.IndexOf("-") + 1) == x.Item.Substring(x.Item.IndexOf("-") + 1))).PickLocation_Tow;
                            row.PickLocation_Three = SiplaceList.FirstOrDefault(x => x.Item == PN || ((row.Customer.ToLower().Contains("rafael") || row.Customer.ToLower().Contains("c4")) && PN.Substring(PN.IndexOf("-") + 1) == x.Item.Substring(x.Item.IndexOf("-") + 1))).PickLocation_Three;

                            if ((SiplaceList2.Count > 0 && SiplaceList2.Exists(x => x.Item == PN && x.PickLocation_One == row.PickLocation_One)) ||
                                (IPlc_list.Count > 0 && !IPlc_list.Exists(x => x.Lot == row.Lot)))
                            {
                                row.PickLocation_One = "No Pick Location in Setup, please check";
                                row.PickLocation_Tow = "";
                                row.PickLocation_Three = "";
                            }

                            if (IPlc_list.Count > 0 && !IPlc_list.Exists(x => x.Lot == row.Lot))
                            {

                            }
                        }
                        else
                            row.PickLocation_One = "No Pick Location in Setup, please check";

                        if (!row.Package_Type.Contains("Reel"))
                        {
                            PCBList.Add(row);
                        }
                        else
                        {
                            if (row.Item == item)
                                row.ID = i;
                            else
                            {
                                row.ID = ++i;
                                bcolor = bcolor == "White" ? "LightGray" : "White";
                            }
                            row.Merge_Status = CheckMerge(row.Lot);

                            if (row.Merge_Status != "Spliced")
                                row.Status = "Not Scanned";

                            row.RowColor = bcolor;

                            //if (row.Merge_Status != "Spliced")
                            MainList.Add(row);
                            item = row.Item;
                        }
                    }

                    PCBList.ForEach(x => { x.ID = i + 1; x.RowColor = bcolor == "White" ? "LightGray" : "White"; });

                    DbReader.Close();
                    DbCommand.Dispose();
                    DbConnection.Close();
                }
                catch (OdbcException ex)
                {
                    MessageBox.Show("Executing the query failed." + ex.Message);
                }
            }
        }

        private string CheckMerge(string Lot)
        {
            string LotTemp = Lot;


            if (Lot.Contains("2425004680"))
            {
                Console.WriteLine("");
            }
          


            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", "");
            string sqlStr = string.Format(@"SELECT *
                                           FROM RefillOffLine
                                            WHERE (IdPackiging_1='{0}' or IdPackiging_2='{0}') and Date>='{1}'
                                           order by Date desc", LotTemp, DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss"));
            DataTable DT = sql.GetDataTable(sqlStr);
            if (DT.Rows.Count > 0)
            {
                int qty1 = Convert.ToInt32(DT.Rows[0]["Quantity_1"].ToString());
                int qty2 = Convert.ToInt32(DT.Rows[0]["Quntity_2"].ToString());
              
                long Lot1 = Convert.ToInt64(DT.Rows[0]["IdPackiging_1"].ToString().Contains("I") ? Convert.ToInt64(DT.Rows[0]["IdPackiging_1"].ToString().Replace("I", "")) : Convert.ToInt64(DT.Rows[0]["IdPackiging_1"].ToString()));
                long Lot2 = Convert.ToInt64(DT.Rows[0]["IdPackiging_2"].ToString().Contains("I") ? Convert.ToInt64(DT.Rows[0]["IdPackiging_2"].ToString().Replace("I","")) : Convert.ToInt64(DT.Rows[0]["IdPackiging_2"].ToString()))  ;


                if (LotTemp.Contains("I"))
                {
                    LotTemp = Lot.Replace("I", " ").ToString().Trim();

                }

                if (Lot1== 2234011378)
                {
                    Console.Write("");
                }

               

                if (qty1 == qty2)
                {
                    if (Lot1.ToString() == LotTemp && Lot1 > Lot2)
                        return "Merge";
                    else if (Lot2.ToString() == LotTemp && Lot2 > Lot1)
                        return "Merge";
                    else
                        return "Spliced";
                }
                else
                {

                    if (Lot1.ToString() == LotTemp && qty1 > qty2)
                        return "Merge";

                  
                   else if (Lot2.ToString() == LotTemp.Trim() && qty2 > qty1)
                        return "Merge";
                   
                    else
                        return "Spliced";
                }
            }
            else
            {
                sqlStr = string.Format(@"SELECT *
                                        FROM RefillOffLine3
                                        WHERE (IdPackiging_1='{0}' or IdPackiging_2='{0}' or IdPackiging_3='{0}') AND Date>='{1}'
                                        order by Date desc", LotTemp, DateTime.Now.AddDays(-8).ToString("yyyy-MM-dd HH:mm:ss"));
                DT = sql.GetDataTable(sqlStr);
                if (DT.Rows.Count > 0)
                {
                    int qty1 = Convert.ToInt32(DT.Rows[0]["Quantity_1"].ToString());
                    int qty2 = Convert.ToInt32(DT.Rows[0]["Quantity_2"].ToString());
                    int qty3 = Convert.ToInt32(DT.Rows[0]["Quantity_3"].ToString());
                    long Lot1 = Convert.ToInt64(DT.Rows[0]["IdPackiging_1"].ToString());
                    long Lot2 = Convert.ToInt64(DT.Rows[0]["IdPackiging_2"].ToString());
                    long Lot3 = Convert.ToInt64(DT.Rows[0]["IdPackiging_3"].ToString());

                    if (qty1 == qty2 && qty2 == qty3)//all Qty equal
                    {
                        if (Lot1.ToString() == LotTemp && Lot1 > Lot2 && Lot1 > Lot3)
                            return "Merge";
                        else if (Lot2.ToString() == LotTemp && Lot2 > Lot1 && Lot2 > Lot3)
                            return "Merge";
                        else if (Lot3.ToString() == LotTemp && Lot3 > Lot1 && Lot3 > Lot2)
                            return "Merge";
                        else
                            return "Spliced";
                    }
                    else if (qty1 == qty2 && qty2 != qty3)//Lot1 & Lot2 Qty is equal
                    {
                        if (qty3 > qty1)
                        {
                            if (Lot3.ToString() == LotTemp)
                                return "Merge";
                            else
                                return "Spliced";
                        }
                        else
                        {
                            if (Lot1.ToString() == LotTemp && Lot1 > Lot2)
                                return "Merge";
                            else if (Lot2.ToString() == LotTemp && Lot2 > Lot1)
                                return "Merge";
                            else
                                return "Spliced";
                        }
                    }
                    else if (qty2 == qty3 && qty2 != qty1)//Lot2 & Lot3 Qty is equal
                    {
                        if (qty1 > qty2)
                        {
                            if (Lot1.ToString() == LotTemp)
                                return "Merge";
                            else
                                return "Spliced";
                        }
                        else
                        {
                            if (Lot2.ToString() == LotTemp && Lot2 > Lot3)
                                return "Merge";
                            else if (Lot3.ToString() == LotTemp && Lot3 > Lot2)
                                return "Merge";
                            else
                                return "Spliced";
                        }
                    }
                    else if (qty1 == qty3 && qty1 != qty2)//Lot1 & Lot3 Qty is equal
                    {
                        if (qty2 > qty1)
                        {
                            if (Lot2.ToString() == LotTemp)
                                return "Merge";
                            else
                                return "Spliced";
                        }
                        else
                        {
                            if (Lot1.ToString() == LotTemp && Lot1 > Lot3)
                                return "Merge";
                            else if (Lot3.ToString() == LotTemp && Lot3 > Lot1)
                                return "Merge";
                            else
                                return "Spliced";
                        }
                    }
                    else//None Qty is equal
                    {
                        if (Lot1.ToString() == LotTemp && qty1 > qty2 && qty1 > qty3)
                            return "Merge";
                        else if (Lot2.ToString() == LotTemp && qty2 > qty1 && qty2 > qty3)
                            return "Merge";
                        else if (Lot3.ToString() == LotTemp && qty3 > qty1 && qty3 > qty2)
                            return "Merge";
                        else
                            return "Spliced";
                    }
                }
                else
                    return "";
            }
        }

        private void SetClassification()
        {
            foreach (DBRow row in MainList)
            {
                if (string.IsNullOrWhiteSpace(row.Classification))
                {
                    List<DBRow> temp = MainList.Where(x => x.Item == row.Item).ToList();
                    temp = temp.OrderBy(x => x.Merge_Status == "Merge" ? 0 : 1).ThenBy(x => x.Date).ThenBy(x => x.Qty).ToList();

                    int n = string.IsNullOrWhiteSpace(temp[0].PickLocation_One) ? 0 :
                            string.IsNullOrWhiteSpace(temp[0].PickLocation_Tow) ? 1 :
                            string.IsNullOrWhiteSpace(temp[0].PickLocation_Three) ? 2 : 3;

                    int i = 1;
                    foreach (DBRow t in temp)
                    {
                        if (n == 0)
                            t.Classification = "First 1";
                        else if (n == 1)
                        {
                            if (i == 1)
                                t.Classification = "First 1";
                            else
                                t.Classification = "Backup 1";
                        }
                        else if (n == 2)
                        {
                            if (i == 1)
                                t.Classification = "First 1";
                            else if (i == 2)
                                t.Classification = "First 2";
                            else if (i % 2 == 0)
                                t.Classification = "Backup 1";
                            else
                                t.Classification = "Backup 2";
                        }
                        else
                        {
                            if (i == 1)
                                t.Classification = "First 1";
                            else if (i == 2)
                                t.Classification = "First 2";
                            else if (i == 3)
                                t.Classification = "First 3";
                            else if (i % 2 == 0)
                                t.Classification = "Backup 1";
                            else
                                t.Classification = "Backup 2";
                        }
                        i++;
                    }
                }
            }
        }

        private string CheckMultipleSetup(string PL)
        {
            MultipleSetupList.Clear();
            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", "");
            string Setup = "";
            string query = string.Format(@"SELECT count( Distinct  convert(varchar(15),dbo.AliasName.ObjectName) + 
                                            ' ' + convert(varchar(15),AliasName_2.ObjectName) + 
                                            ' ' +convert(varchar(15),dbo.CPickupLink.nLocation)+ 
                                            ' ' +convert(varchar(15),dbo.CPickupLink.lTrack)+ 
                                            ' ' +convert(varchar(15),dbo.CpickupLink.lReserve)) as cnt, dbo.AliasName.ObjectName as setup
                                            FROM dbo.CSetup 
                                            INNER JOIN dbo.AliasName ON dbo.CSetup.OID = dbo.AliasName.PID 
                                            INNER JOIN dbo.CRecipe ON dbo.CSetup.OID = dbo.CRecipe.spSetupRef 
                                            INNER JOIN dbo.CPickupLink ON dbo.CRecipe.OID = dbo.CPickupLink.PID 
                                            INNER JOIN dbo.AliasName AS AliasName_2 ON dbo.CPickupLink.spComponentRef = AliasName_2.PID 
                                            INNER JOIN dbo.AliasName AS AliasName_1 ON dbo.CPickupLink.spStation = AliasName_1.PID
                                            WHERE dbo.AliasName.ObjectName like '%{0}%' AND dbo.CPickupLink.lTower = 0
                                            group by dbo.AliasName.ObjectName", PL);

            DataTable dt = sql.GetDataTable(query);
            if (dt.Rows.Count > 1)//if the setup is split to 2 lines
            {
                MultipleSetupList = new List<string>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MultipleSetupList.Add(dt.Rows[i]["setup"].ToString());
                }

                Select_Setup S = new Select_Setup(MultipleSetupList)
                {
                    Owner = this
                };

                S.ShowDialog(ref Setup);
            }
            else if (dt.Rows.Count > 0)
            {
                Setup = dt.Rows[0]["setup"].ToString();
            }
            return Setup;
        }

        private List<DBRow> CheckIPLC(string PL)
        {
            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", "");
            string Side = "";
            string query = string.Format(@"SELECT * FROM WH_Kit WHERE WH2!='' and PL='{0}'", PL);
            List<DBRow> list = new List<DBRow>();

            DataTable dt = sql.GetDataTable(query);
            if (dt.Rows.Count > 0)//if its Iplc
            {
                string WH1 = dt.Rows[0]["WH"].ToString().Trim();
                string WH2 = dt.Rows[0]["WH2"].ToString().Trim();

                Select_Iplc S = new Select_Iplc()
                {
                    Owner = this
                };

                S.ShowDialog(ref Side);
                sql = new SQLClassSQLClass("10\\S", "", "", "");
                query = string.Format(@"SELECT * FROM Sort_And_Scan_Iplc WHERE PL='{0}' AND WH='{1}'", PL, Side == "Left" ? WH1 : WH2);
                DataTable dt2 = sql.GetDataTable(query);
                if (dt2.Rows.Count > 0)
                {


                    list = (from rw in dt2.AsEnumerable()
                            select new DBRow()
                            {
                                Item = rw["PN"].ToString()?.Trim(),
                                Lot = rw["Lot"].ToString()?.Trim(),
                                WH = rw["WH"].ToString()?.Trim()
                            }).ToList();
                    return list;

                }
            }
            return list;
        }

        // Event handler for Clear button click // Обработчик нажатия кнопки очистки
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            PLTxt.Text = ""; // Clear PL textbox // Очищаем поле PL
            FocusManager.SetFocusedElement(this, PLTxt); // Set focus to PL textbox // Устанавливаем фокус на поле PL
        }
    }
}
