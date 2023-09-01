using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace WPFApp.Model
{
    public class Database
    {
        public static string FolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/jwTodoApp";

        public string FilePath = $"{FolderPath}/todo.json";

        private readonly JsonSerializerOptions JsonSerializeOptions = new JsonSerializerOptions() { WriteIndented = true };

        public void CreateJson()
        {
            if (Directory.Exists(FolderPath))
            {
                return;
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

        public async void WriteJson(TodoItems TodoList)
        {
            using (FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                using StreamWriter writer = new StreamWriter(stream);
                await writer.WriteAsync(System.Text.Json.JsonSerializer.Serialize<TodoItems>(TodoList, JsonSerializeOptions));
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
                    Debug.WriteLine(json);

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

        public static string GenUUID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}
