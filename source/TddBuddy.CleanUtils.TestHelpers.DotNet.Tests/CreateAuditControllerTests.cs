using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using TddBuddy.CleanUtils.TestHelpers.DotNet.Factories;

namespace TddBuddy.CleanUtils.TestHelpers.DotNet.Tests
{
    [TestFixture]
    public class CreateAuditControllerTests
    {
        [Test]
        public void Create_WhenValidTemplateId_ShouldReturnSuccess()
        {
            //---------------Set up test pack-------------------
            var dateOf = new DateTime(2017, 3, 3);
            var requestUri = "api/v1/audit/create/spar";
            var usecase = GenerateCreateAuditUseCase(dateOf);
            var userDetailsService = Substitute.For<IUserDetailsService>();

            var testServer = new TestServerBuilder<CreateAuditController>()
                                    .WithInstanceRegistration<ICreateAuditUsecase>(usecase)
                                    .WithInstanceRegistration<IUserDetailsService>(userDetailsService)
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
        public void Create_WhenInvalidTemplateId_ShouldReturnUnprocessableEntityCode()
        {
            //---------------Set up test pack-------------------
            var requestUri = "api/v1/audit/create/invalid-template";
            var usecase = GenerateCreateAuditUseCase();
            var userDetailsService = Substitute.For<IUserDetailsService>();
            var testServer = new TestServerBuilder<CreateAuditController>()
                                    .WithInstanceRegistration<ICreateAuditUsecase>(usecase)
                                    .WithInstanceRegistration<IUserDetailsService>(userDetailsService)
                                    .Build();
            using (testServer)
            {
                var client = TestHttpClientFactory.CreateClient(testServer);
                //---------------Execute Test ----------------------
                var response = client.GetAsync(requestUri).Result;
                //---------------Test Result -----------------------
                Assert.AreEqual(CustomHttpStatusCode.UnprocessableEntity, response.StatusCode);
            }
        }

        private CreateAuditUseCase GenerateCreateAuditUseCase()
        {
            return GenerateCreateAuditUseCase(DateTime.MaxValue);
        }

        private CreateAuditUseCase GenerateCreateAuditUseCase(DateTime creationDate)
        {
            var auditRepository = new AuditRepositoryTestDataBuilder()
                                    .WithCreateAudit(audit => audit.Created = creationDate)
                                    .Build();
            var templateRepository = Substitute.For<IAuditTemplateRepository>();
            templateRepository.Find(Arg.Any<string>()).Returns("{ \"isFalse\":false }");
            var usecase = new CreateAuditUseCase(auditRepository, templateRepository);
            return usecase;
        }
    }
}
