using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for DeleteTaskWIndow.xaml
    /// </summary>
    public partial class DeleteTaskWIndow : Window
    {
        public DeleteTaskWIndow()
        {
            InitializeComponent();
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new();
            MainWindow.Show();
            this.Close();
        }

        private void EnterButtonClicked (object sender, RoutedEventArgs e)
        {
            string key = KeyText.Text;

            string json = File.ReadAllText("Tasks.json");
            TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

            try
            {
                if (tasks.ContainsKey(key))
                {
                    int k = tasks.Count;

                    tasks.Remove(key);

                    int DeletedTaskKey = int.Parse(key);

                    // foreach (var Key in KeysToUpdate)
                    for (int i = k; DeletedTaskKey < i + 1; i--)
                    {
                        var value = tasks[i.ToString()];
                        tasks.Remove(value.ToString());
                        int NewKey = int.Parse(key);
                        tasks[key.ToString()] = value;
                    }

                    var UpdatedTasks = JsonConvert.SerializeObject(tasks);
                    File.WriteAllText("Tasks.json", UpdatedTasks);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message.ToString());
            }
                
        }
    }
}
