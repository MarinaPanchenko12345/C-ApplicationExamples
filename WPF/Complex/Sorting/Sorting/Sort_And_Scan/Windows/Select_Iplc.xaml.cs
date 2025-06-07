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
using System.Windows.Shapes; // Import shape drawing // Импорт рисования фигур

namespace Sort_And_Scan.Windows // Namespace for organizing windows // Пространство имён для организации окон
{
    public partial class Select_Iplc : Window // Partial class for Select_Iplc window // Частичный класс для окна Select_Iplc
    {
        // Constructor for Select_Iplc window // Конструктор для окна Select_Iplc
        public Select_Iplc()
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            CB.ItemsSource = new List<string>() { "Left", "Right" }; // Set ComboBox items to "Left" and "Right" // Устанавливаем элементы ComboBox: "Left" и "Right"
        }

        // Method to show dialog and return selected side // Метод для показа диалога и возврата выбранной стороны
        public bool? ShowDialog(ref string Setup)
        {
            bool? result = this.ShowDialog(); // Show the dialog and get result // Показываем диалог и получаем результат
            Setup = CB.Text; // Assign selected ComboBox value to Setup // Присваиваем выбранное значение ComboBox переменной Setup
            return result; // Return dialog result (true if OK, false if Cancel) // Возвращаем результат диалога (true если OK, false если Cancel)
        }

        // Event handler for OK button click // Обработчик нажатия кнопки OK
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true; // Set dialog result to true (OK pressed) // Устанавливаем результат диалога в true (нажата OK)
            Close(); // Close the dialog window // Закрываем окно диалога
        }
    }
}