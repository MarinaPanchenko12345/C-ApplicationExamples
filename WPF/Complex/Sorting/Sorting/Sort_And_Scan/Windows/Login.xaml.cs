using Sort_And_Scan.Classes; // Import SQLClass and other classes // Импорт SQLClass и других классов
using System; // Import base .NET types // Импорт базовых типов .NET
using System.Collections.Generic; // Import collections // Импорт коллекций
using System.Data; // Import ADO.NET data types // Импорт типов данных ADO.NET
using System.Linq; // Import LINQ // Импорт LINQ
using System.Runtime.Remoting.Lifetime;
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
    public partial class Login : Window // Partial class for Login window // Частичный класс для окна Login
    {
        // Constructor for Login window // Конструктор для окна Login
        public Login()
        {
            InitializeComponent(); // Initialize UI components // Инициализация компонентов интерфейса
            FocusManager.SetFocusedElement(this, textBoxWorkNumber); // Set focus to work number textbox // Устанавливаем фокус на поле ввода номера сотрудника
        }

        // Event handler for OK button click (WinForms signature) // Обработчик нажатия кнопки OK (WinForms сигнатура)
        private void BtnOK_Click(object sender, EventArgs e)
        {
            GetUser(); // Call method to check user credentials // Вызываем метод для проверки учетных данных пользователя
        }

        // Event handler for key down event in textboxes // Обработчик события нажатия клавиши в текстовых полях
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) // If Enter key is pressed // Если нажата клавиша Enter
            {
                GetUser(); // Call method to check user credentials // Вызываем метод для проверки учетных данных пользователя
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //Auto Focus for Inputs
        //private void OnKeyDownHandler(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Return)
        //    {
        //        // If currently focused on work number input
        //        if (textBoxWorkNumber.IsFocused)
        //        {
        //            // Move focus to password field
        //            textBoxPassword.Focus(); // 👉 Set focus to password input
        //            return; // Skip login until password is entered
        //        }

        //        // If already in password field, try to log in
        //        GetUser(); // Проверка логина/пароля
        //    }
        //}


        // Method to check user credentials and log in // Метод для проверки учетных данных пользователя и входа
        private void GetUser()
        {
            SQLClass sql = new SQLClass("10\\S", "", "", ""); // Create SQLClass instance for DB operations // Создаём экземпляр SQLClass для работы с БД
            string qry = string.Format(@"SELECT * FROM employers WHERE work_number='{0}'", textBoxWorkNumber.Text.Trim()); // Prepare SQL query to find user by work number // Готовим SQL-запрос для поиска пользователя по номеру
            DataTable dt = sql.GetDataTable(qry); // Execute query and get results // Выполняем запрос и получаем результаты

            if (dt.Rows.Count > 0) // If user exists in database // Если пользователь найден в базе данных
            {
                // Check if entered password matches the database password // Проверяем, совпадает ли введённый пароль с паролем из базы
                if (dt.Rows[0]["password"].ToString()?.Trim() == textBoxPassword.Password.Trim())
                {
                    MainWindow.UserId = dt.Rows[0]["work_number"].ToString().Trim(); // Set global user ID // Устанавливаем глобальный ID пользователя
                    MainWindow.UserName = dt.Rows[0]["name"].ToString().Trim(); // Set global user name // Устанавливаем глобальное имя пользователя
                    DialogResult = true; // Set dialog result to true (login successful) // Устанавливаем результат диалога в true (успешный вход)
                    Close(); // Close the login window // Закрываем окно входа
                }
                else
                {
                    MessageBox.Show("Worng Password!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk); // Show wrong password message // Показываем сообщение о неверном пароле
                    textBoxPassword.Password = ""; // Clear password field // Очищаем поле пароля
                    textBoxWorkNumber.Text = ""; // Clear work number field // Очищаем поле номера сотрудника
                    return; // Exit method // Выходим из метода
                }
            }
            else
            {
                MessageBox.Show("User Number Not Exist!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk); // Show user not found message // Показываем сообщение о том, что пользователь не найден
                textBoxWorkNumber.Text = ""; // Clear work number field // Очищаем поле номера сотрудника
                return; // Exit method // Выходим из метода
            }
        }

        // Event handler for WPF OK button click // Обработчик нажатия кнопки OK (WPF)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetUser(); // Call method to check user credentials // Вызываем метод для проверки учетных данных пользователя
        }
    }
}

//Please add detailed inline comments in English and Russian to each part of this code, including:
//-Each function and its purpose,
//- Each line inside functions,
//- Each class and property.

//Add comments directly in the code. Use line-by-line explanation. First write the comment in English, then add the Russian translation.