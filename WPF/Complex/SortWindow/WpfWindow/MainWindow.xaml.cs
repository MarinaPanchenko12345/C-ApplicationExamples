using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using WpfWindow.View;

namespace WpfWindow
{
    public partial class MainWindow : Window
    {






        private void Txt_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (!(e.Key == Key.Return))
                {
                    return; // Only respond to the Enter key
                }

                // Get the TextBox from the sender
                TextBox txtBox = sender as TextBox;

                if (txtBox == null || txtBox.Text.Length != 10)
                {
                    // Create an error message if the input is less than 10 characters
                    string errorMessage = "Invalid number or already exist in data base.";
                    ModalWindow modalWindow = new ModalWindow(errorMessage);
                    modalWindow.Owner = this; // Set owner to make it modal
                    modalWindow.ShowDialog(); // Display the modal window here
                }
            }
            catch (Exception ex)
            {
                // Create and show ModalWindow instead of MessageBox for exception errors
                string errorMessage = "An error occurred: " + ex.Message;
                ModalWindow modalWindow = new ModalWindow(errorMessage);
                modalWindow.Owner = this; // Set owner to make it modal
                modalWindow.ShowDialog(); // Display the error in the custom modal
            }
        }
    }
}