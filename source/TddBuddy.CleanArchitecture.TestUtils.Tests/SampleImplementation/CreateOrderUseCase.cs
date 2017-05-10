namespace TddBuddy.CleanArchitecture.TestUtils.Tests.SampleImplementation
{
    public class CreateOrderUseCase : ICreateOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void Execute(CreateOrderInputTo inputTo, DummyPresenter<string, ErrorTo> presenter)
        {
            var result = _orderRepository.CreateOrder(inputTo);
            if (result)
            {
                presenter.Respond("Order has been placed");
                return;
            }

            var error = new ErrorTo
            {
                Error = "An error occured placing the order"
            };

            presenter.Respond(error);
        }
    }
}