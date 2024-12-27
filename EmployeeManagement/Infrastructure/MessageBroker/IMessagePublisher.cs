namespace EmployeeManagement.Infrastructure.MessageBroker
{
    public interface IMessagePublisher
    {
        Task Publish<T>(T obj);
        Task Publish(string message);
    }
}
