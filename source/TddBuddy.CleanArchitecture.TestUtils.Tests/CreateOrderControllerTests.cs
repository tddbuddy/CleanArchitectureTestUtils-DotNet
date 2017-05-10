using System;
using System.Net;
using NSubstitute;
using NUnit.Framework;
using TddBuddy.CleanArchitecture.TestUtils.Builders;
using TddBuddy.CleanArchitecture.TestUtils.Factories;
using TddBuddy.CleanArchitecture.TestUtils.Tests.SampleImplementation;

namespace TddBuddy.CleanArchitecture.TestUtils.Tests
{
    // todo: create xxx, fetch yyy
    [TestFixture]
    public class CreateOrderControllerTests
    {
        [Test]
        public void Create_WhenValidProductName_ShouldReturnSuccess()
        {
            //---------------Set up test pack-------------------
            var orderId = Guid.NewGuid();
            var productName = "blueberry-muffin";
            var quantity = 2;
            var requestUri = $"api/v1/order/create/{orderId}/{productName}/{quantity}";
            var usecase = CreateUseCase(true);

            var testServer = new TestServerBuilder<CreateOrderController>()
                                    .WithInstanceRegistration<ICreateOrderUseCase>(usecase)
                                    .Build();
            using (testServer)
            {
                var client = TestHttpClientFactory.CreateClient(testServer);
                //---------------Execute Test ----------------------
                var response = client.GetAsync(requestUri).Result;
                //---------------Test Result -----------------------
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Test]
        public void Create_WhenInvalidProductName_ShouldReturnUnprocessableEntityCode()
        {
            //---------------Set up test pack-------------------
            var orderId = Guid.NewGuid();
            var productName = "invalid-product";
            var quantity = 2;
            var requestUri = $"api/v1/order/create/{orderId}/{productName}/{quantity}";
            var usecase = CreateUseCase(false);
            var testServer = new TestServerBuilder<CreateOrderController>()
                                    .WithInstanceRegistration<ICreateOrderUseCase>(usecase)
                                    .Build();
            using (testServer)
            {
                var client = TestHttpClientFactory.CreateClient(testServer);
                //---------------Execute Test ----------------------
                var response = client.GetAsync(requestUri).Result;
                //---------------Test Result -----------------------
                Assert.AreEqual((HttpStatusCode)422, response.StatusCode);
            }
        }

        private CreateOrderUseCase CreateUseCase(bool createResult)
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            orderRepository.CreateOrder(Arg.Any<CreateOrderInputTo>()).Returns(createResult);
            return new CreateOrderUseCase(orderRepository);
        }
    }
}
