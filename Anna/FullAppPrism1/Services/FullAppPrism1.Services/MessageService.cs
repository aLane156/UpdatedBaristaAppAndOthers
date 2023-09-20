using FullAppPrism1.Services.Interfaces;

namespace FullAppPrism1.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
