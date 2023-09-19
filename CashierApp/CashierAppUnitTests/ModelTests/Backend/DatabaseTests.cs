namespace CashierApp.Model.Backend
{
    public class DatabaseTests
    {
        [Fact]
        public async void TestDatabaseCreateJson()
        {
            var mockFileSystem = new MockFileSystem();
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\CashierApp";

            Database._fileSystem = mockFileSystem;

            await Database.CreateJson();

            Assert.Equal(mockFileSystem, Database._fileSystem);

            Assert.True(mockFileSystem.FileExists($"{path}\\products\\food-products.json"));
            Assert.True(mockFileSystem.FileExists($"{path}\\products\\drink-products.json"));
            Assert.True(mockFileSystem.FileExists($"{path}\\products\\dessert-products.json"));
        }

        [Fact]
        public async void TestDatabaseCreateLog()
        {
            var mockFileSystem = new MockFileSystem();
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\CashierApp";
            string date = $"{DateTime.Now.Date.ToString("dd-MM-yyyy")}";

            Database._fileSystem = mockFileSystem;

            await Database.CreateLog();

            Assert.Equal(mockFileSystem, Database._fileSystem);

            Assert.True(mockFileSystem.FileExists($"{path}\\logs\\{date}\\{date}-log.txt"));
        }
    }
}
