using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin.Testing;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace TddBuddy.CleanArchitecture.TestUtils.Builders
{
    // todo : handle middle ware and user details 
    public class TestServerBuilder<TTypeInControllerAssembly>
    {
        private readonly Container _container;
        private readonly HttpConfiguration _httpConfiguration = new HttpConfiguration();

        public TestServerBuilder()
        {
            // use our type resolver instead
            OverrideControllerTypeResolver();
            // replace the internal static 'Controller' value, silly M$
            OverrideControllerConstant();

            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            _httpConfiguration.MapHttpAttributeRoutes();
            RegisterControllers();
        }

        private void RegisterControllers()
        {
            var controllerAssembly = typeof(TTypeInControllerAssembly).Assembly;
            _container.RegisterWebApiControllers(_httpConfiguration, controllerAssembly);
        }

        private void OverrideControllerConstant()
        {
            var suffix =
                typeof(DefaultHttpControllerSelector).GetField("ControllerSuffix", BindingFlags.Static | BindingFlags.Public);
            if (suffix != null) suffix.SetValue(null, string.Empty);
        }

        private void OverrideControllerTypeResolver()
        {
            _httpConfiguration.Services.Replace(typeof(IHttpControllerTypeResolver), new CustomHttpControllerTypeResolver());
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
