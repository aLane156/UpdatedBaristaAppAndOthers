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
using System.IO;

namespace TaskList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string curTaskName = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TaskName.Text = ReadLineOfFile(0);
        }

        private string ReadLineOfFile(int lineNo)
        {
            string line = File.ReadLines("C:\\Users\\mb153367\\Documents\\testing-file.txt").ElementAt(lineNo);
            return line;
        }

        private void WriteLineOfFile(int lineNo, string changeTo)
        {
            string[] lines = File.ReadAllLines("C:\\Users\\mb153367\\Documents\\testing-file.txt");
            lines[lineNo] = changeTo;
            File.WriteAllLines("C:\\Users\\mb153367\\Documents\\testing-file.txt", lines);
        }

        private void SaveTaskName_Click(object sender, RoutedEventArgs e)
        {
            WriteLineOfFile(0, Task_Name_Input.Text);
        }
    }
}
