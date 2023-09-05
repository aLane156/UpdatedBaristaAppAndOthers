using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WPFApp.Model
{
    public class Database
    {
        public Database() { }

        private static readonly string FolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/jwTodoApp";
        private string FilePath = $"{FolderPath}/todo.json";

        public void CreateJson()
        {
            if (Directory.Exists(FolderPath))
            {
                if (File.Exists(FilePath))
                {
                    return;
                }
                else
                {
                    try
                    {
                        File.Create(FilePath);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    File.Create($"{FolderPath}/todo.json");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public void WriteJson(TodoItems TodoList)
        {
            try
            {
                using FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                using StreamWriter writer = new StreamWriter(stream);
                writer.Write(JsonConvert.SerializeObject(TodoList, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Database.cs] Writing Json Failed: '{ex.Message}'");
                throw new Exception("Failed to write to Json file.");
            }
        }

        public TodoItems ReadJson()
        {
            try
            {
                using (FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    using StreamReader reader = new StreamReader(stream);

                    var json = reader.ReadToEnd();

                    TodoItems TempList = JsonConvert.DeserializeObject<TodoItems>(json);

                    return TempList;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new TodoItems();
            }
        }

        public string GenUUID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}
