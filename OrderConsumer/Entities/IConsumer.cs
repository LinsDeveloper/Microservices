namespace OrderConsumer.Entities
{
    public interface IConsumer
    {
        Task<TicketConsumer> Consume();
    }
}
