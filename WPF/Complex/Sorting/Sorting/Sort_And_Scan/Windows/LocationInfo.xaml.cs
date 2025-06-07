using Sort_And_Scan.Classes; // Import classes from the Classes namespace // Импорт классов из пространства имён Classes
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
    public partial class LocationInfo : Window // Partial class for LocationInfo window // Частичный класс для окна LocationInfo
    {
        List<DBRow> SkidList = new List<DBRow>(); // List to store DBRow objects (SKIDs) // Список для хранения объектов DBRow (SKID'ов)

        // Constructor for LocationInfo window // Конструктор для окна LocationInfo
        public LocationInfo(List<DBRow> list)
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            SkidList = list; // Assign the provided list to SkidList // Присваиваем переданный список переменной SkidList
        }

        // Event handler for Lot_Txt text change // Обработчик изменения текста в поле Lot_Txt
        private void Lot_Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Lot_Txt.Text.Length == 10) // If the entered lot number is 10 characters // Если введённый номер лота состоит из 10 символов
            {
                if (SkidList.Exists(x => x.Lot == Lot_Txt.Text)) // If such a lot exists in the list // Если такой лот есть в списке
                {
                    // Set PickLocation_Txt to the pick location of the found lot // Устанавливаем PickLocation_Txt на место отбора найденного лота
                    PickLocation_Txt.Text = SkidList.FirstOrDefault(x => x.Lot == Lot_Txt.Text).PickLocation_One;
                    FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus back to Lot_Txt // Возвращаем фокус на Lot_Txt
                }
                else
                {
                    Lot_Txt.Text = ""; // Clear Lot_Txt if not found // Очищаем Lot_Txt, если не найдено
                    PickLocation_Txt.Text = "Skid: " + Lot_Txt.Text + " Not Exist"; // Show not exist message // Показываем сообщение о том, что не существует
                    FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus back to Lot_Txt // Возвращаем фокус на Lot_Txt
                    return; // Exit method // Выходим из метода
                }
            }
        }

        // Event handler for Clear button click // Обработчик нажатия кнопки очистки
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Lot_Txt.Text = ""; // Clear Lot_Txt textbox // Очищаем поле Lot_Txt
            PickLocation_Txt.Text = ""; // Clear PickLocation_Txt textbox // Очищаем поле PickLocation_Txt
            FocusManager.SetFocusedElement(this, Lot_Txt); // Set focus back to Lot_Txt // Возвращаем фокус на Lot_Txt
        }
    }
}