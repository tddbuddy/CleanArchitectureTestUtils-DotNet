namespace TddBuddy.CleanArchitecture.TestUtils.Tests.SampleImplementation
{
    public interface ICreateOrderUseCase
    {
        void Execute(CreateOrderInputTo inputTo, DummyPresenter<string, ErrorTo> presenter);
    }
}