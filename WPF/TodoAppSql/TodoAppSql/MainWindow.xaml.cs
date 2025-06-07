using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TodoAppSql.Models;
using TodoAppSql.Services;

namespace TodoAppSql
{
    public partial class MainWindow : Window
    {
        private BindingList<TodoModel> _TodoDataList;
        private DatabaseService _dbService; // ✅ заменили FileIOService на DatabaseService

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _dbService = new DatabaseService(); // ✅ подключение к БД

            try
            {
                _TodoDataList = _dbService.LoadData(); // ✅ загрузка из БД
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных из базы: " + ex.Message);
                Close();
            }

            dgTodoList.ItemsSource = _TodoDataList;

            _TodoDataList.ListChanged += _TodoDataList_ListChanged;
        }

        private void _TodoDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded ||
                e.ListChangedType == ListChangedType.ItemDeleted ||
                e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _dbService.SaveData(_TodoDataList); // ✅ сохраняем в БД
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении в базу: " + ex.Message);
                    Close();
                }
            }
        }

        private void DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is TodoModel todo)
            {
                var result = MessageBox.Show("Delete this task?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _dbService.DeleteById(todo.Id); // ✅ удаление из базы данных
                        _TodoDataList.Remove(todo);      // ✅ удаление из UI
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении из базы: " + ex.Message);
                    }
                }
            }
        }

    }
}