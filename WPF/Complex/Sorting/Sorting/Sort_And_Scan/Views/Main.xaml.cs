using Sort_And_Scan.Windows;
using System.Windows.Input;

namespace Sort_And_Scan.Views
{
    public partial class Main : UserControl // Main user control for the application // Основной пользовательский контрол приложения
    {
        int NumOfComp; // Stores the number of unique components // Хранит количество уникальных компонентов
        int NumOfSkid; // Stores the number of SKIDs // Хранит количество SKID'ов
        List<DBRow> SkidList; // List of all DBRow objects representing SKIDs // Список всех объектов DBRow, представляющих SKID'ы
        DBRow CurrentScan; // Currently scanned DBRow // Текущий сканируемый объект DBRow

        // Constructor for Main user control // Конструктор для пользовательского контрола Main
        public Main(List<DBRow> list)
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            this.Loaded += new RoutedEventHandler(MyWindow_Loaded); // Attach Loaded event handler // Привязываем обработчик события Loaded
            SkidList = list; // Assign the provided list to SkidList // Присваиваем переданный список переменной SkidList
            Set_Parametes(); // Set initial parameters and update UI // Устанавливаем начальные параметры и обновляем интерфейс
        }

        // Event handler for when the control is loaded // Обработчик события загрузки контрола
        void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Lot_Txt.Focus(); // Set focus to the Lot_Txt textbox // Устанавливаем фокус на текстовое поле Lot_Txt
        }

        // Method to calculate and display various parameters // Метод для вычисления и отображения различных параметров
        private void Set_Parametes()
        {
            NumOfComp = SkidList.Select(x => x.Item).Distinct().Count(); // Count unique components // Считаем уникальные компоненты
            NumOfSkid = SkidList.Where(x => x.Status != "" && !x.PickLocation_One.Contains("No Pick Location")).Count(); // Count valid SKIDs // Считаем действительные SKID'ы
            NumOfComp_Txt.Text = "Number Of Component: " + NumOfComp; // Display number of components // Отображаем количество компонентов
            FirstScanned_Txt.Text = "First Scanned: " + SkidList.Where(x => x.Classification.Contains("First") && x.Status == "Scanned" && !x.PickLocation_One.Contains("No Pick Location")).Count() + "/" + SkidList.Where(x => x.Classification.Contains("First") && !x.PickLocation_One.Contains("No Pick Location")).Count(); // Display first scanned count // Отображаем количество первых отсканированных
            TotalScanned_Txt.Text = "Total Scanned: " + SkidList.Where(x => x.Status == "Scanned" && !x.PickLocation_One.Contains("No Pick Location")).Count() + "/" + NumOfSkid; // Display total scanned // Отображаем общее количество отсканированных
            NumOfSkid_Txt.Text = "Number Of SKIDs: " + NumOfSkid; // Display number of SKIDs // Отображаем количество SKID'ов
            BackupScanned_Txt.Text = "Backup Scanned: " + SkidList.Where(x => x.Classification.Contains("Backup") && x.Status == "Scanned" && !x.PickLocation_One.Contains("No Pick Location")).Count() + "/" + SkidList.Where(x => x.Classification.Contains("Backup") && x.Status != "" && !x.PickLocation_One.Contains("No Pick Location")).Count(); // Display backup scanned // Отображаем количество резервных отсканированных

            // Show or hide AllFirst_Border based on whether all "First" are scanned // Показываем или скрываем AllFirst_Border в зависимости от того, все ли "First" отсканированы
            if (SkidList.Where(x => x.Classification.Contains("First") && x.Status == "Scanned" && !x.PickLocation_One.Contains("No Pick Location")).Count() == SkidList.Where(x => x.Classification.Contains("First") && !x.PickLocation_One.Contains("No Pick Location")).Count())
                AllFirst_Border.Visibility = Visibility.Visible; // Show border // Показываем границу
            else
                AllFirst_Border.Visibility = Visibility.Hidden; // Hide border // Скрываем границу

            // Show or hide Complete_Btn based on scan completion or shortage // Показываем или скрываем Complete_Btn в зависимости от завершения сканирования или нехватки
            if (SkidList.Where(x => x.Status == "Scanned" && !x.PickLocation_One.Contains("No Pick Location")).Count() == NumOfSkid || MainWindow.IsShortageBtnPressed)
                Complete_Btn.Visibility = Visibility.Visible; // Show button // Показываем кнопку
            else
                Complete_Btn.Visibility = Visibility.Hidden; // Hide button // Скрываем кнопку
        }

        // Event handler for Lot_Txt text change // Обработчик изменения текста в Lot_Txt
        private void Lot_Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Lot_Txt.Text.Length == 10) // If the entered lot number is 10 characters // Если введённый номер лота состоит из 10 символов
            {
                if (SkidList.Exists(x => x.Lot == Lot_Txt.Text && x.Status == "Not Scanned")) // If such a lot exists and is not scanned // Если такой лот существует и не отсканирован
                {
                    CurrentScan = SkidList.First(x => x.Lot == Lot_Txt.Text); // Get the current scan object // Получаем текущий объект сканирования
                    SkidList.FirstOrDefault(x => x.Lot == Lot_Txt.Text).Status = "Scanned"; // Mark as scanned // Отмечаем как отсканированный
                    Lot_Txt2.Text = CurrentScan.Lot; // Display lot number // Отображаем номер лота

                    // Set pick location based on classification // Устанавливаем место отбора в зависимости от классификации
                    if (CurrentScan.Classification == "First 1" || CurrentScan.Classification == "Backup 1")
                        PickLocation_Txt.Text = CurrentScan.PickLocation_One; // Use PickLocation_One // Используем PickLocation_One
                    else if (CurrentScan.Classification == "First 2" || CurrentScan.Classification == "Backup 2")
                        PickLocation_Txt.Text = CurrentScan.PickLocation_Tow; // Use PickLocation_Tow // Используем PickLocation_Tow
                    else if (CurrentScan.Classification == "First 3")
                        PickLocation_Txt.Text = CurrentScan.PickLocation_Three; // Use PickLocation_Three // Используем PickLocation_Three

                    First_Txt.Text = CurrentScan.Classification; // Display classification // Отображаем классификацию

                    // Set background color and button state based on classification // Устанавливаем цвет фона и состояние кнопки в зависимости от классификации
                    if (CurrentScan.Classification.Contains("First"))
                    {
                        PickLocation_Border.Background = PickLocation_Txt.Text.Contains("Tr:") ? Brushes.LightGreen : Brushes.LightGoldenrodYellow; // Set background for "First" // Устанавливаем фон для "First"
                        First_Border.Background = PickLocation_Txt.Text.Contains("Tr:") ? Brushes.LightGreen : Brushes.LightGoldenrodYellow; // Set border background // Устанавливаем фон границы
                        SetFirstBtn.IsEnabled = false; // Disable SetFirstBtn // Отключаем SetFirstBtn
                    }
                    else if (CurrentScan.Classification.Contains("Backup"))
                    {
                        PickLocation_Border.Background = PickLocation_Txt.Text.Contains("Tr:") ? Brushes.LightSkyBlue : Brushes.LightGoldenrodYellow; // Set background for "Backup" // Устанавливаем фон для "Backup"
                        First_Border.Background = PickLocation_Txt.Text.Contains("Tr:") ? Brushes.LightSkyBlue : Brushes.LightGoldenrodYellow; // Set border background // Устанавливаем фон границы
                        SetFirstBtn.IsEnabled = true; // Enable SetFirstBtn // Включаем SetFirstBtn
                    }

                    InfoSP.Visibility = Visibility.Visible; // Show info panel // Показываем панель информации
                    Item_Txt.Text = CurrentScan.Item; // Display item // Отображаем элемент
                    Grid1.ItemsSource = SkidList.Where(x => x.Item == CurrentScan.Item).ToList(); // Show all SKIDs for this item // Показываем все SKID'ы для этого элемента

                    Set_Parametes(); // Update parameters // Обновляем параметры
                    Lot_Txt.Text = ""; // Clear input // Очищаем ввод
                    FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus back to input // Возвращаем фокус на ввод
                }
                else
                {
                    new WarningWindow("Skid: " + Lot_Txt.Text + " is not valid or already scanned")
                    {
                        Owner = this
                    }.ShowDialog();

                    // show warning if lot is invalid or already scanned // показываем предупреждение, если лот недействителен или уже отсканирован
                    //messagebox.show("skid: " + lot_txt.text + " is not valid or you already scanned it", "warning", messageboxbutton.ok, messageboximage.warning);
                    Lot_Txt.Text = ""; // clear input // очищаем ввод
                    FocusManager.SetFocusedElement(this, lot_txt); // set focus back to input // возвращаем фокус на ввод
                    return; // exit method // выходим из метода
                }
            }
        }

        // Event handler for SetFirst button click // Обработчик нажатия кнопки SetFirst
        private void SetFirst_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentScan != null) // If there is a current scan // Если есть текущий скан
            {
                List<DBRow> list = SkidList.Where(x => x.ID == CurrentScan.ID).ToList(); // Get all rows with the same ID // Получаем все строки с тем же ID

                if (list.Exists(x => x.Date < CurrentScan.Date)) // Check FIFO rule // Проверяем правило FIFO
                {
                    MessageBox.Show("Does Not Follow FIFO Rules", "", MessageBoxButton.OK, MessageBoxImage.Warning); // Show warning // Показываем предупреждение
                    return; // Exit method // Выходим из метода
                }

                // If no other "First" scanned for this ID // Если нет другого "First" для этого ID
                if (!list.Exists(x => x.Lot != CurrentScan.Lot && x.Status == "Scanned" && x.Classification.Contains("First")))
                {
                    list.ForEach(x => { x.Classification = "Backup"; }); // Set all as Backup // Устанавливаем всем Classification = "Backup"
                    list.FirstOrDefault(x => x.Lot == CurrentScan.Lot).Classification = "First 1"; // Set current as First 1 // Устанавливаем текущему "First 1"
                    First_Txt.Text = CurrentScan.Classification; // Update classification display // Обновляем отображение классификации
                    PickLocation_Border.Background = Brushes.LightGreen; // Set background color // Устанавливаем цвет фона
                    First_Border.Background = Brushes.LightGreen; // Set border color // Устанавливаем цвет границы
                    Grid1.ItemsSource = list; // Update grid // Обновляем таблицу
                    Set_Parametes(); // Update parameters // Обновляем параметры
                }
                else
                {
                    // Show warning if another reel is already scanned as First // Показываем предупреждение, если другой ролик уже отсканирован как First
                    MessageBox.Show("Another reel already scanned as First. Please check", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Exit method // Выходим из метода
                }
            }

            FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus back to input // Возвращаем фокус на ввод
        }

        // Event handler for Location Info button click // Обработчик нажатия кнопки информации о местоположении
        private void LocInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            LocationInfo loc = new LocationInfo(SkidList); // Create new LocationInfo window // Создаём новое окно LocationInfo
            loc.Owner = MainWindow.mn; // Set owner window // Устанавливаем главное окно владельцем
            loc.ShowDialog(); // Show dialog // Показываем диалоговое окно
        }

        // Event handler for Completion button click // Обработчик нажатия кнопки завершения
        private void Completion_Click(object sender, RoutedEventArgs e)
        {
            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", ""); // Create SQLClass instance for DB operations // Создаём экземпляр SQLClass для работы с БД
            try
            {
                string total = SkidList.Where(x => x.Status == "Scanned").Count() + "/" + NumOfSkid; // Calculate total scanned // Вычисляем общее количество отсканированных
                // Prepare SQL request to insert or update record // Готовим SQL-запрос для вставки или обновления записи
                string request = string.Format(@"IF NOT EXISTS(SELECT 1 FROM Sort_And_Scan where PL='{0}')
                                                INSERT INTO Sort_And_Scan (PL, WH, Customer, Line, Worker, Total_Scanned, Time) 
                                                VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')
                                             ELSE
                                                UPDATE Sort_And_Scan SET Total_Scanned='{5}' WHERE PL='{0}'",
                                                MainWindow.mn.PLTxt.Text.Trim(), // Get PL value // Получаем значение PL
                                                MainWindow.mn.WHTxt.Text, // Get WH value // Получаем значение WH
                                                MainWindow.mn.CustomerTxt.Text, // Get Customer value // Получаем значение Customer
                                                MainWindow.mn.LineTxt.Text, // Get Line value // Получаем значение Line
                                                MainWindow.UserName, // Get current user name // Получаем имя пользователя
                                                total, // Total scanned value // Значение общего количества отсканированных
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); // Current date and time // Текущая дата и время
                sql.Execute(request); // Execute SQL command // Выполняем SQL-команду
                MessageBox.Show("Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Asterisk); // Show success message // Показываем сообщение об успешном сохранении

            }
            catch (Exception ex)
            {
                // Show error message if exception occurs // Показываем сообщение об ошибке при возникновении исключения
                MessageBox.Show(ex.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
    }
}