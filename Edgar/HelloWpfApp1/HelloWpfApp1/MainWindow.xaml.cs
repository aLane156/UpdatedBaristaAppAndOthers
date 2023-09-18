using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Security.Cryptography;
using Microsoft.Win32;
using Newtonsoft.Json;


namespace HelloWpfApp1
{
    public partial class MainWindow : Window
    {
        static string AESKey = "abCdEfgHijKlmnOp";

        private ObservableCollection<TaskItem> tasks = new ObservableCollection<TaskItem>();

        public MainWindow()
        {
            InitializeComponent();
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            taskList.ItemsSource = tasks;
            taskDescription.ItemsSource = tasks;
            taskDate.ItemsSource = tasks;

        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string date = txtDate.Text.Trim();

            if (!string.IsNullOrEmpty(name))
            {
                if (!taskList.Items.Contains(txtName.Text))
                {
                    tasks.Add(new TaskItem { Name = name, Description = description, Date = date });
                    txtName.Clear();
                    txtDescription.Clear();
                    txtDate.Clear();
                }
                else throw new InvalidOperationException("Name already used");
            }
            else throw new InvalidOperationException("Name not filled");
        }


        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (taskList.SelectedIndex >= 0)
            {
                tasks.RemoveAt(taskList.SelectedIndex);
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string date = txtDate.Text.Trim();
            if (taskList.SelectedIndex >= 0)
            {
                tasks.RemoveAt(taskList.SelectedIndex);
                tasks.Add(new TaskItem { Name = name, Description = description, Date = date });
            }
        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            bool? result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == true) // Test result.
            {
                string file = openFileDialog1.FileName;
                LastFile.file = file;
                try
                {
                    string encryptedJson = File.ReadAllText(file);

                    string json = AesBody.Decrypt(encryptedJson);
                    List<TaskItem> importedTasks = JsonConvert.DeserializeObject<List<TaskItem>>(json);
                    tasks.Clear();
                    foreach (TaskItem task in importedTasks)
                    {
                        tasks.Add(task);
                    }
                }
                catch (IOException)
                {
                }
            }
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JSON files (*.json)|*.json";
            bool? result = saveFileDialog1.ShowDialog();
            if (result == true)
            {
                string file = saveFileDialog1.FileName;
                LastFile.file = file;
                try
                {
                    string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                    var encryptedJson = AesBody.Encrypt(json);
                    File.WriteAllText(file, encryptedJson);
                }
                catch (IOException)
                {
                }
            }
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine($"LastFile = {LastFile.file}");
            if (File.Exists(LastFile.file))
            {
                string file = LastFile.file;
                try
                {
                    string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                    var encryptedJson = AesBody.Encrypt(json);
                    File.WriteAllText(file, encryptedJson);
                }
                catch (IOException)
                {
                }
            }
        }

        private void TaskList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (taskList.SelectedItem is TaskItem selectedTask)
            {
                txtName.Text = selectedTask.Name;
                txtDescription.Text = selectedTask.Description;
                txtDate.Text = selectedTask.Date;
            }
        }

        private void ColorComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //if (selectedTask.Tags)
            //{

            //}
        }
    }
    public static class LastFile
    {
        public static string? file;
    }
    static class AESKey_IV
    {
        public static string Key = "12345678912345671234567891234565"; // field
        public static string IV = "1234567891234567";   // property
    }
    
    public class TaskItem
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Date { get; set; }
        public bool IsCompleted { get; set; }
    }
    class AesBody
    {
        // Simple function to not bother user with selecting Key or IV (Initialising Vector)
        public static string Encrypt(string plainText)
        {
            using (Aes myAes = Aes.Create())
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(AESKey_IV.IV);
                string base64IV = System.Convert.ToBase64String(buffer);
                string EncryptDataWithAesData = EncryptDataWithAes(plainText, AESKey_IV.Key, base64IV);
                return EncryptDataWithAesData;
            }
        }
        // Simple function to not bother user with selecting Key or IV (Initialising Vector)
        public static string Decrypt(string encryptText)
        {
            using (Aes myAes = Aes.Create())
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(AESKey_IV.IV);
                string base64IV = System.Convert.ToBase64String(buffer);
                return DecryptDataWithAes(encryptText, AESKey_IV.Key, base64IV);
            }
        }
        /// <summary>
        /// Encrypt with AES
        /// </summary>
        /// <param name="plainText">Enter the text to be encryped</param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static string EncryptDataWithAes(string plainText, string key, string iv)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                // Set the Key and IV to the shared one
                aesAlgorithm.Key = Convert.FromBase64String(key);
                aesAlgorithm.IV = Convert.FromBase64String(iv);

                // Create encryptor object
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();

                byte[] encryptedData;

                //Encryption will be done in a memory stream through a CryptoStream object
                /* 1. Substitution of the bytes
                 * 2. Shifting the rows
                 * 3. Mixing the columns
                 * 4. Adding the round key
                 * Repeats this 10-14 times based on key size  */
                 
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encryptedData = ms.ToArray();
                    }
                }
                

                return Convert.ToBase64String(encryptedData);
            }
        }
        private static string DecryptDataWithAes(string cipherText, string keyBase64, string vectorBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                aesAlgorithm.IV = Convert.FromBase64String(vectorBase64);

                // Create decryptor object
                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();

                byte[] cipher = Convert.FromBase64String(cipherText);

                //Decryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}