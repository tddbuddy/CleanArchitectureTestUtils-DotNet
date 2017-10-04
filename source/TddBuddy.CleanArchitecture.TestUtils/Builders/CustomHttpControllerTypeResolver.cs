using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace TddBuddy.CleanArchitecture.TestUtils.Builders
{
    public class CustomHttpControllerTypeResolver : DefaultHttpControllerTypeResolver
    {
        public CustomHttpControllerTypeResolver()
            : base(IsHttpEndpoint)
        { }

        internal static bool IsHttpEndpoint(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return type.IsClass 
                  && type.IsVisible 
                  && !type.IsAbstract
                  && typeof(ApiController).IsAssignableFrom(type) 
                  && typeof(IHttpController).IsAssignableFrom(type);
        }
    }
}