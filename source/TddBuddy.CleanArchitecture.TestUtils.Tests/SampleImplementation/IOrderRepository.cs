namespace TddBuddy.CleanArchitecture.TestUtils.Tests.SampleImplementation
{
    public interface IOrderRepository
    {
        bool CreateOrder(CreateOrderInputTo inputTo);
    }
}