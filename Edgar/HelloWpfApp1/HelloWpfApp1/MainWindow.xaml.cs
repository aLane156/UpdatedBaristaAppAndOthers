using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

namespace HelloWpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonAddName_Click(object sender, RoutedEventArgs e)
        {
            /* Nested if statements to throw correct error */
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtDescription.Text))
                {
                    if (!taskNames.Items.Contains(txtName.Text))
                    {
                        taskNames.Items.Add(txtName.Text);
                        taskDescription.Items.Add(txtDescription.Text);
                        txtName.Clear();
                    }
                    else throw new InvalidOperationException("Name already used");
                }
                else throw new InvalidOperationException("Description not filled");
            }
            else throw new InvalidOperationException("Name not filled");
        }

        private void taskNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
