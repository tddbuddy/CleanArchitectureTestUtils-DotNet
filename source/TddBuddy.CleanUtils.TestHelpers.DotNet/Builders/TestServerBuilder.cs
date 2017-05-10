using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin.Testing;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace TddBuddy.CleanUtils.TestHelpers.DotNet.Builders
{
    // todo : handle middle ware and user details 
    public class TestServerBuilder<TTypeInControllerAssembly>
    {
        private readonly Container _container;
        private readonly HttpConfiguration _httpConfiguration = new HttpConfiguration();

        public TestServerBuilder()
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            _httpConfiguration.MapHttpAttributeRoutes();
            var controllerAssembly = typeof(TTypeInControllerAssembly).Assembly;
            _container.RegisterWebApiControllers(_httpConfiguration, controllerAssembly);
        }

        public TestServerBuilder<TTypeInControllerAssembly> WithInstanceRegistration<T>(T instance) where T : class
        {
            // ReSharper disable once RedundantTypeArgumentsOfMethod
            _container.Register<T>(()=> instance);
            return this;
        }

        public TestServer Build()
        {
            return TestServer.Create(appBuilder =>
            {
                _httpConfiguration.EnsureInitialized();
                _httpConfiguration.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

                _httpConfiguration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(_container);
                appBuilder.UseWebApi(_httpConfiguration);
            });
        }
        
        private class GlobalExceptionHandler : ExceptionHandler
        {
            public override void Handle(ExceptionHandlerContext context)
            {
                throw context.Exception;
            }
        }
    }
}
