using Sort_And_Scan.Classes; // Import classes from the Classes namespace // Импорт классов из пространства имён Classes
using System; // Import base .NET types // Импорт базовых типов .NET
using System.Collections.Generic; // Import collections // Импорт коллекций
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
    public partial class SortAndScan : UserControl // Partial class for SortAndScan user control // Частичный класс для пользовательского контрола SortAndScan
    {
        List<DBRow> MainList = new List<DBRow>(); // Main list of DBRow objects to display in the grid // Основной список объектов DBRow для отображения в таблице

        // Constructor for SortAndScan user control // Конструктор для пользовательского контрола SortAndScan
        public SortAndScan(List<DBRow> list)
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            MainList = list; // Assign the provided list to MainList // Присваиваем переданный список переменной MainList
            new Thread(() => FillGrid(MainList)).Start(); // Start a new thread to fill the grid // Запускаем новый поток для заполнения таблицы
            Grid1.ItemsSource = MainList; // Set the data source of the grid to MainList // Устанавливаем источник данных таблицы на MainList
            Rows_Lbl.Content = "Num Of Rows: " + MainList.Count; // Display the number of rows // Отображаем количество строк
        }

        // Method to fill the grid with data (runs on UI thread) // Метод для заполнения таблицы данными (выполняется в UI-потоке)
        private void FillGrid(List<DBRow> list)
        {
            this.Dispatcher.Invoke(() => // Use Dispatcher to update UI from another thread // Используем Dispatcher для обновления UI из другого потока
            {
                Grid1.ItemsSource = null; // Clear the grid's data source // Очищаем источник данных таблицы
                Grid1.ItemsSource = list; // Set the grid's data source to the provided list // Устанавливаем источник данных таблицы на переданный список
            });
        }

        // Event handler for Clear button click // Обработчик нажатия кнопки очистки
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            SearchItem_Txt.Text = ""; // Clear the search textbox // Очищаем текстовое поле поиска
            Grid1.ItemsSource = MainList; // Reset the grid to show all items // Сбрасываем таблицу для отображения всех элементов
            Rows_Lbl.Content = "Num Of Rows: " + MainList.Count; // Update the row count label // Обновляем метку количества строк
        }

        // Event handler for search textbox text change // Обработчик изменения текста в поле поиска
        private void SearchItem_Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender; // Cast sender to TextBox // Приводим sender к типу TextBox
            // Filter MainList by items that start with the entered text (case-insensitive) // Фильтруем MainList по элементам, начинающимся с введённого текста (без учёта регистра)
            var list = MainList.Where(x => x.Item.ToLower().StartsWith(t.Text.ToLower().Trim())).ToList();
            Grid1.ItemsSource = list; // Set the filtered list as the grid's data source // Устанавливаем отфильтрованный список как источник данных таблицы
            Rows_Lbl.Content = "Num Of Rows: " + list.Count; // Update the row count label // Обновляем метку количества строк
        }
    }
}