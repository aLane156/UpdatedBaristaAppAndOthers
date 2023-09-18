using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Task_Manager;
using static System.Net.Mime.MediaTypeNames;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            ReadFile();
        }

        // Button event catchers
        private void AddTask(object sender, RoutedEventArgs e)
        {
            AddTaskWindow AddTaskWindow = new();
            AddTaskWindow.Show();
            this.Close();

            //Adding a new task will be processed on a different window.
        }

        private void EditTask(object sender, RoutedEventArgs e)
        {
            Console.Write("Select a task to edit");
        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            //A message appears prompting the user to click on the task they want to delete. For now, its key will be entered instead.

            DeleteTaskWIndow DeleteTaskWindow = new();
            DeleteTaskWindow.Show();
            this.Close();
        }

        private void ExitProgram(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public void ReadFile()
        {
            // file reader here

            // Currently only works with one object

            if (File.Exists("Tasks.json"))
            {
                try
                {
                    using StreamReader r = new("Tasks.json");

                    string json = File.ReadAllText("Tasks.json");
                    TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

                    Grid TaskGrid = new()
                    {
                        Name = "TaskGrid",
                        Height = 795,
                        Margin = new Thickness(0, 20, 0, 0)
                    };

                    ScrollBar ScrollBar = new()
                    {
                        Name = "TaskScrollbar",
                        Height = 795,
                        Width = 20,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(1900, 0, 0, 0)
                    };

                    TaskGrid.Children.Add(ScrollBar);

                    for (int i = 1; i < tasks.Count + 1; i++)
                    {
                        Grid grid = new()
                        {
                            Name = "TaskGrid" + (i).ToString(),
                            Height = 100,
                            Width = 1900,
                            Margin = new Thickness(0, 0, 0, 900 - (200 * i))
                        };


                        ColumnDefinition ColumnT = new()
                        {
                            Width = new GridLength(500)
                        };
                        grid.ColumnDefinitions.Add(ColumnT);

                        ColumnDefinition ColumnD = new()
                        {
                            Width = new GridLength(1000)
                        };
                        grid.ColumnDefinitions.Add(ColumnD);

                        ColumnDefinition ColumnDT = new()
                        {
                            Width = new GridLength(400)
                        };
                        grid.ColumnDefinitions.Add(ColumnDT);

                        Task task = tasks[(i).ToString()];

                        TextBlock Title = new()
                        {
                            Name = "TitleText" + (i).ToString(),
                            Text = tasks[i.ToString()].Title.ToString(),
                            Height = 30,
                            Margin = new Thickness(1),
                            FontSize = 20,
                        };


                        TextBlock Description = new()
                        {
                            Name = "DescriptionText" + (i).ToString(),
                            Text = tasks[i.ToString()].Description.ToString(),
                            Height = 30,
                            Width = 600,
                            Margin = new Thickness(1),
                            FontSize = 20,
                        };

                        TextBlock DateAndTime = new()
                        {
                            Name = "DateAndTime" + (i).ToString(),
                            Text = tasks[i.ToString()].Date.ToString() + "  " + tasks[i.ToString()].Time.ToString(),
                            Height = 30,
                            Margin = new Thickness(1),
                            FontSize = 20,
                        };

                        Grid.SetColumn(Title, 0);
                        Grid.SetColumn(Description, 1);
                        Grid.SetColumn(DateAndTime, 2);

                        grid.Children.Add(Title);
                        grid.Children.Add(Description);
                        grid.Children.Add(DateAndTime);

                        // Change to comment when showing program.
                        //this.Content = grid;

                    }

                    r.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message.ToString());
                }
            }

            else
            {
                File.Create("Tasks.json");
            }

            
        }
    }
}

/* <Window x:Class="Task_Manager.MainWindow"
xmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns: x = "http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns: d = "http://schemas.microsoft.com/expression/blend/2008"
        xmlns: mc = "http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns: local = "clr-namespace:Task_Manager"
        mc: Ignorable = "d"
        Height = "1080" Width = "1920"
        WindowState = "Maximized" WindowStyle = "None" >
    < Window.Resources >
        < DataTemplate x: Key = "myTaskTemplate" >
            < StackPanel Orientation = "Horizontal" >
                < TextBlock Name = "TitleText" FontSize = "60" Text = "{Binding Path=Title}" />
                < TextBlock Name = "DescriptionText" FontSize = "40" Text = "{Binding Path=Description}" />
                < TextBlock Name = "DateAndTimeText" FontSize = "40" Text = "" />
            </ StackPanel >
        </ DataTemplate >
    </ Window.Resources >
    < Grid >
        < Grid Height = "135" VerticalAlignment = "Bottom" Name = "ButtonBar" >
            < Image Source = "/Images/BarBackground.jpg" Stretch = "UniformToFill" />
            < Button Height = "80" Width = "500" VerticalAlignment = "Bottom" Margin = "-1200,0,0,25" Content = "Add Task"  Name = "AddTaskButton" Click = "AddTask" FontSize = "48" />
            < Button Height = "80" Width = "500" VerticalAlignment = "Bottom" Margin = "0,0,0,25" Content = "Edit Task" Name = "EditTaskButton" Click = "EditTask" FontSize = "48" />
            < Button Height = "80" Width = "500" VerticalAlignment = "Bottom" Margin = "1200,0,0,25" Content = "Delete Task" Name = "DeleteDaskButton" Click = "DeleteTask" FontSize = "48" />
        </ Grid >

        < Image Height = "945" VerticalAlignment = "Top" Grid.ColumnSpan = "2" Source = "/Images/MainBackground.jpg" Stretch = "UniformToFill" />

        < Grid Name = "TopScreen" VerticalAlignment = "Top" Height = "200" >
            < Button Height = "100" Width = "200" Margin = "0,0,1600,0" Name = "ExitProgramButton" Content = "Exit" FontSize = "52" Click = "ExitProgram" />
        </ Grid >

        < Grid Height = "795" Margin = "0,20,0,0" >
            < ScrollBar Height = "795" Width = "20" VerticalAlignment = "Top" Margin = "1900,0,0,0" />
            < ListBox Width = "1900" Height = "200" HorizontalAlignment = "Left"
                     ItemsSource = "{Binding Tasks}"
                     ItemTemplate = "{StaticResource myTaskTemplate}" />
        </ Grid >
    </ Grid >
</ Window >
*/
