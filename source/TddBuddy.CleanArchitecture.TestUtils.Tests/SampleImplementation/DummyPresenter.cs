using System.Net;
using System.Web.Http;
using System.Web.Http.Results;

namespace TddBuddy.CleanArchitecture.TestUtils.Tests.SampleImplementation
{
    public class DummyPresenter<TOkContent, TErrorContent>
        where TOkContent : class
        where TErrorContent : class
    {
        private readonly ApiController _controller;
        private TOkContent _okContent;
        private TErrorContent _errorContent;

        public DummyPresenter(ApiController controller)
        {
            _controller = controller;
        }

        public IHttpActionResult Render()
        {
            if (_okContent != null)
            {
                return new OkNegotiatedContentResult<TOkContent>(_okContent, _controller);
            }

            return new NegotiatedContentResult<TErrorContent>((HttpStatusCode) 422, _errorContent, _controller);
        }

        public void Respond(TOkContent payload)
        {
            _okContent = payload;
        }

        public void Respond(TErrorContent payload)
        {
            _errorContent = payload;
        }
    }
}