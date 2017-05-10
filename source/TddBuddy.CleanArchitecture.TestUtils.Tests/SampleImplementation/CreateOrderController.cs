using System;
using System.Web.Http;

namespace TddBuddy.CleanArchitecture.TestUtils.Tests.SampleImplementation
{
    [RoutePrefix("api/v1/order")]
    public class CreateOrderController : ApiController
    {
        private readonly ICreateOrderUseCase _createOrderUseCase;

        public CreateOrderController(ICreateOrderUseCase createOrderUseCase)
        {
            _createOrderUseCase = createOrderUseCase;
        }

        [Route("create/{orderId}/{productName}/{quantity}")]
        [HttpGet]
        public IHttpActionResult Execute(Guid orderId, string productName, int quantity)
        {
            var inputTo = new CreateOrderInputTo
            {
                OrderId = orderId,
                ProductName = productName,
                Quantity = productName

            };
            var presenter = new DummyPresenter<string, ErrorTo>(this);
            _createOrderUseCase.Execute(inputTo, presenter);
            return presenter.Render();
        }
    }
}