using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using CashierApp.Model.Types;
using Newtonsoft.Json;

namespace CashierApp.Model.Backend
{
    public static class Database
    {
        #region Static Path Files

        public static IFileSystem _fileSystem = new FileSystem();

        private readonly static string FolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\CashierApp";

        private readonly static string ProductPath = $"{FolderPath}\\products";

        #endregion

        #region Log Files

        public static async Task CreateLog()
        {
            DateTime date = DateTime.Now;
            string dateDirectory = $"{FolderPath}\\logs\\{date.Date.ToString("dd-MM-yyyy")}";
            string dateLog = $"{dateDirectory}\\{date.Date.ToString("dd-MM-yyyy")}-log.txt";

            if (!_fileSystem.Directory.Exists(dateDirectory))
            {
                _fileSystem.Directory.CreateDirectory(dateDirectory);
            }

            if (!_fileSystem.File.Exists(dateLog))
            {
                await using var stream = _fileSystem.File.Create(dateLog);
                stream.Close();

                await WriteLogLine(new("Created log file.", "MainWindowViewModel.cs", LogType.INFO));
            }
        }

        public static async Task WriteLogLine(LogEntry logEntry)
        {
            DateTime date = DateTime.Now;
            string dateDirectory = $"{FolderPath}\\logs\\{date.Date.ToString("dd-MM-yyyy")}";
            string dateLog = $"{dateDirectory}\\{date.Date.ToString("dd-MM-yyyy")}-log.txt";

            await _fileSystem.File.AppendAllLinesAsync(dateLog, new string[] { logEntry.ToString() });
        }

        /// <summary>
        /// Writing to log file with direct log object parameters.
        /// </summary>
        /// <param name="logText">The text of the log line.</param>
        /// <param name="logFile">The place where the log was issued from.</param>
        /// <param name="logType">The type of log. LogType enum.</param>
        public static async Task WriteLogLine(string logText, string logFile, LogType logType)
        {
            LogEntry logEntry = new(logText, logFile, logType);

            DateTime date = DateTime.Now;
            string dateDirectory = $"{FolderPath}\\{date.Date.ToString("dd-MM-yyyy")}";
            string dateLog = $"{dateDirectory}\\{date.Date.ToString("dd-MM-yyyy")}-log.txt";

            await _fileSystem.File.AppendAllLinesAsync(dateLog, new string[] { logEntry.ToString() });
        }

        #endregion

        #region Json Handling

        /// <summary>
        /// Asynchronously creates the JSON files required by the project.
        /// </summary>
        public static async Task CreateJson()
        {
            string[] filePaths = new string[] { "food-products.json", "drink-products.json", "dessert-products.json" };

            if (!_fileSystem.Directory.Exists(ProductPath)) 
            {
                _fileSystem.Directory.CreateDirectory(ProductPath);
            }

            foreach (string filePath in filePaths)
            {
                string tempPath = $"{ProductPath}\\{filePath}";

                if (!_fileSystem.File.Exists(tempPath)) 
                {
                    await using var stream = _fileSystem.File.Create(tempPath);
                }
            }
        }

        public static async Task<ObservableCollection<FoodProduct>> ReadJsonFood()
        {
            FoodProducts result = new();
            ObservableCollection<FoodProduct> results = new();
            string filePath = $"{ProductPath}\\food-products.json";

            try
            {
                using StreamReader reader = _fileSystem.File.OpenText(filePath);

                string json = await reader.ReadToEndAsync();

                if (json != null)
                {
                    result = JsonConvert.DeserializeObject<FoodProducts>(json);

                    foreach (FoodProduct item in result.FoodProductsList)
                    {
                        results.Add(item);
                    }
                }
                else
                {
                    await WriteLogLine("Failed to deserialize FoodProducts object", "Database.cs", LogType.ERROR);
                    throw new FileLoadException($"Failed to load file: {filePath}");
                }
            } 
            catch (Exception ex)
            {
                await WriteLogLine($"Failed to retrieve information from food-products.json, exception: {ex.Message}", "Database.cs", LogType.ERROR);
            }

            //await Task.Delay(10000);

            return results;
        }

        public static async Task<ObservableCollection<DrinkProduct>> ReadJsonDrink()
        {
            DrinkProducts result = new();
            ObservableCollection<DrinkProduct> results = new();
            string filePath = $"{ProductPath}\\drink-products.json";

            try
            {
                using StreamReader reader = _fileSystem.File.OpenText(filePath);

                string json = await reader.ReadToEndAsync();

                if (json != null)
                {
                    result = JsonConvert.DeserializeObject<DrinkProducts>(json);

                    foreach (DrinkProduct item in result.DrinkProductsList)
                    {
                        results.Add(item);
                    }
                }
                else
                {
                    await WriteLogLine("Failed to deserialize DrinkProducts object", "Database.cs", LogType.ERROR);
                    throw new FileLoadException($"Failed to load file: {filePath}");
                }
            }
            catch (Exception ex)
            {
                await WriteLogLine($"Failed to retrieve information from drink-products.json, exception: {ex.Message}", "Database.cs", LogType.ERROR);
            }

            return results;
        }

        public static async Task WriteFile(ObservableCollection<DrinkProduct> writeItems)
        {
            using var stream = new FileStream($"{ProductPath}\\drink-products.json", FileMode.Open, FileAccess.Write);
            using var writer = new StreamWriter(stream);

            DrinkProducts tempProducts = new();
            tempProducts.DrinkProductsList = writeItems;

            await writer.WriteAsync(JsonConvert.SerializeObject(tempProducts, Formatting.Indented));
        }

        #endregion
    }
}
