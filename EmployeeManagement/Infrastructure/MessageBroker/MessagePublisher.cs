
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Azure.ServiceBus;

namespace EmployeeManagement.Infrastructure.MessageBroker
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IQueueClient _queueClient;

        public MessagePublisher(IQueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public Task Publish<T>(T obj)
        {
            var message = new Message(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj)));
            return _queueClient.SendAsync(message);
        }

        public Task Publish(string msg)
        {
            var message = new Message(Encoding.UTF8.GetBytes(msg));
            return _queueClient.SendAsync(message);
        }
    }
}
