using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CashierApp.Model
{
    public static class Database
    {
        #region Static Path Files

        private readonly static string FolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\CashierApp";

        private readonly static string ProductPath = $"{FolderPath}\\products";

        #endregion

        #region Log Files

        public static async Task CreateLog()
        {
            DateTime date = DateTime.Now;
            string dateDirectory = $"{FolderPath}\\logs\\{date.Date.ToString("dd-MM-yyyy")}";
            string dateLog = $"{dateDirectory}\\{date.Date.ToString("dd-MM-yyyy")}-log.txt";

            if (!Directory.Exists(dateDirectory))
            {
                Directory.CreateDirectory(dateDirectory);
            }

            if (!File.Exists(dateLog))
            {
                await using FileStream stream = File.Create(dateLog);
                stream.Close();
                
                await WriteLogLine(new("Created log file.", "MainWindowViewModel.cs", LogType.INFO));
            }
        }

        public static async Task WriteLogLine(LogEntry logEntry)
        {
            DateTime date = DateTime.Now;
            string dateDirectory = $"{FolderPath}\\logs\\{date.Date.ToString("dd-MM-yyyy")}";
            string dateLog = $"{dateDirectory}\\{date.Date.ToString("dd-MM-yyyy")}-log.txt";

            await File.AppendAllLinesAsync(dateLog, new string[] { logEntry.ToString() });
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

            await File.AppendAllLinesAsync(dateLog, new string[] { logEntry.ToString() });
        }

        #endregion

        #region Json Handling

        /// <summary>
        /// Asynchronously creates the JSON files required by the project.
        /// </summary>
        public static async Task CreateJson()
        {
            string[] filePaths = new string[] { "food-products.json", "drink-products.json", "dessert-products.json" };

            if (!Directory.Exists(ProductPath)) 
            {
                Directory.CreateDirectory(ProductPath);
            }

            foreach (string filePath in filePaths)
            {
                string tempPath = $"{ProductPath}\\{filePath}";

                if (!File.Exists(tempPath)) 
                {
                    await using var stream = File.Create(tempPath);
                }
            }
        }

        /// <summary>
        /// Asynchronously reads data from product data files.
        /// </summary>
        /// <param name="productType">The specific products file to access. Takes ProductType enum.</param>
        /// <returns>Returns an observable collection of Products.</returns>
        public static async Task<ObservableCollection<Product>> ReadJson(ProductType productType)
        {
            string filePath = string.Empty;

            switch (productType)
            {
                case ProductType.Dessert:
                    filePath = $"{ProductPath}\\dessert-products.json";
                    //string[] allLines = await File.ReadAllLinesAsync(filePath);
                    return new ObservableCollection<Product>();
                case ProductType.Food:
                    filePath = $"{ProductPath}\\food-products.json";
                    FoodProducts tempList = await ReadJsonFood(filePath);

                    // Generics are not covariant (learn more about this!!!)
                    if (tempList != null) return new ObservableCollection<Product>(tempList.FoodProductsList);
                    else 
                    { 
                        await WriteLogLine($"File: {filePath} was empty no values have been fetched", "Database.cs", LogType.WARNING); 
                        return new ObservableCollection<Product>(); 
                    } 
                case ProductType.Drink:
                    filePath = $"{ProductPath}\\drink-products.json";
                    return new ObservableCollection<Product>();
                default:
                    return new ObservableCollection<Product>();
            }
        }

        private static async Task<FoodProducts> ReadJsonFood(string filePath)
        {
            FoodProducts result = new FoodProducts();

            try
            {
                using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using StreamReader reader = new StreamReader(stream);

                string json = await reader.ReadToEndAsync();

                if (json != null)
                {
                    result = JsonConvert.DeserializeObject<FoodProducts>(json);
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

            return result;
        }

        public static async Task WriteFile(ObservableCollection<FoodProduct> writeItems)
        {
            using var stream = new FileStream($"{ProductPath}\\food-products.json", FileMode.Open, FileAccess.Write);
            using var writer = new StreamWriter(stream);

            FoodProducts tempFoodProducts = new();
            tempFoodProducts.FoodProductsList = writeItems;

            await writer.WriteAsync(JsonConvert.SerializeObject(tempFoodProducts, Formatting.Indented));

        }

        #endregion
    }
}
