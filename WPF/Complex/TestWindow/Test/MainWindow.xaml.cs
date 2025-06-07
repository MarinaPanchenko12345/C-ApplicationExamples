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

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> validLots = new List<string> { "1234567890", "9876543210" };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LotTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string lot = LotTextBox.Text.Trim();

            if (lot.Length == 10)
            {
                if (validLots.Contains(lot))
                {
                    MessageBox.Show("✅ Valid Lot: " + lot, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LotTextBox.Text = "";
                }
                else
                {
                    new WarningWindow("Skid: " + lot + " is not valid or already scanned")
                    {
                        Owner = this
                    }.ShowDialog();

                    LotTextBox.Text = "";
                    LotTextBox.Focus();
                }
            }
        }
    }
}