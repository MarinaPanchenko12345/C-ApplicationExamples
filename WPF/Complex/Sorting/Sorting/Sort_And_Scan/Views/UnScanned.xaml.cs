using Sort_And_Scan.Classes; // Import SQLClass and other classes // Импорт SQLClass и других классов
using Sort_And_Scan.Windows; // Import windows for dialogs // Импорт окон для диалогов
using System; // Import base .NET types // Импорт базовых типов .NET
using System.Collections.Generic; // Import collections // Импорт коллекций
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
    public partial class UnScanned : UserControl // Partial class for UnScanned user control // Частичный класс для пользовательского контрола UnScanned
    {
        List<DBRow> list; // List of DBRow objects representing unscanned items // Список объектов DBRow, представляющих неотсканированные элементы

        // Constructor for UnScanned user control // Конструктор для пользовательского контрола UnScanned
        public UnScanned(List<DBRow> list)
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            this.list = list; // Assign the provided list to the field // Присваиваем переданный список полю класса
            Grid1.ItemsSource = list; // Set the data source of the grid to the list // Устанавливаем источник данных таблицы на список

            // Enable or disable the Shortage button based on list content // Включаем или отключаем кнопку Shortage в зависимости от содержимого списка
            if (list == null || list.Count == 0)
                Shortage_Btn.IsEnabled = false; // Disable button if list is empty // Отключаем кнопку, если список пуст
            else
                Shortage_Btn.IsEnabled = true; // Enable button if list has items // Включаем кнопку, если в списке есть элементы
        }

        // Event handler for Shortage button click // Обработчик нажатия кнопки Shortage
        private void Shortage_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder str = new StringBuilder(); // StringBuilder to build the items string // StringBuilder для формирования строки элементов

            SQLClass sql = new SQLClassSQLClass("10\\S", "", "", ""); // Create SQLClass instance for DB operations // Создаём экземпляр SQLClass для работы с БД
            try
            {
                // Loop through each unscanned row and append its data // Проходим по каждому неотсканированному элементу и добавляем его данные
                foreach (DBRow row in list)
                {
                    str.Append(row.Item + "," + row.Lot + "|"); // Append item and lot, separated by comma and pipe // Добавляем элемент и лот, разделённые запятой и вертикальной чертой
                }

                // Remove the last pipe character to format the string correctly // Удаляем последний символ '|' для правильного форматирования строки
                string Items = str.ToString().Substring(0, str.ToString().Length - 1);

                // Prepare SQL request to insert a shortage message // Готовим SQL-запрос для вставки сообщения о нехватке
                string request = string.Format(@"INSERT INTO Sort_And_Scan_Msg (PL, WH, Customer, Items, Worker, Time, Send) 
                                                VALUES('{0}','{1}','{2}','{3}','{4}','{5}',0)",
                                                MainWindow.mn.PLTxt.Text.Trim(), // Get PL value // Получаем значение PL
                                                MainWindow.mn.WHTxt.Text, // Get WH value // Получаем значение WH
                                                MainWindow.mn.CustomerTxt.Text, // Get Customer value // Получаем значение Customer
                                                Items, // List of items and lots // Список элементов и лотов
                                                MainWindow.UserName, // Get current user name // Получаем имя пользователя
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); // Current date and time // Текущая дата и время

                sql.Execute(request); // Execute SQL command // Выполняем SQL-команду
                MainWindow.IsShortageBtnPressed = true; // Set shortage flag to true // Устанавливаем флаг нехватки в true
                MessageBox.Show("Reported Successfully", "", MessageBoxButton.OK, MessageBoxImage.Asterisk); // Show success message // Показываем сообщение об успешной отправке

            }
            catch (Exception ex)
            {
                // Show error message if exception occurs // Показываем сообщение об ошибке при возникновении исключения
                MessageBox.Show(ex.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
    }
}