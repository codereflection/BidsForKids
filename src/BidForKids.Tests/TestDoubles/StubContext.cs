using System.Web;
using System.Collections.Specialized;
using NSubstitute;

namespace BidsForKids.Tests
{
    class StubContext : HttpContextBase
    {
        StubRequest request;

        public StubContext(string relativeUrl)
        {
            request = new StubRequest(relativeUrl);
        }

        public override HttpRequestBase Request
        {
            get { return request; }
        }


        public static HttpContextBase FakeHttpContext()
        {
            var context = Substitute.For<HttpContextBase>();
            var request = Substitute.For<HttpRequestBase>();
            var response = Substitute.For<HttpResponseBase>();
            var sessionState = Substitute.For<HttpSessionStateBase>();
            var serverUtility = Substitute.For<HttpServerUtilityBase>();
            request.QueryString.Returns(new NameValueCollection());
            context.Request.Returns(request);
            context.Response.Returns(response);
            context.Session.Returns(sessionState);
            context.Server.Returns(serverUtility);
            return context;
        }
    }
}
