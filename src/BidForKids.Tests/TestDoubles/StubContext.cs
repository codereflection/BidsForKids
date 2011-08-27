using System.Web;
using Rhino.Mocks;
using System.Collections.Specialized;

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
            var context = MockRepository.GenerateStub<HttpContextBase>();
            var request = MockRepository.GenerateStub<HttpRequestBase>();
            var response = MockRepository.GenerateStub<HttpResponseBase>();
            var sessionState = MockRepository.GenerateStub<HttpSessionStateBase>();
            var serverUtility = MockRepository.GenerateStub<HttpServerUtilityBase>();
            request.Stub(x => x.QueryString).Return(new NameValueCollection());
            context.Stub(x => x.Request).Return(request);
            context.Stub(x => x.Response).Return(response);
            context.Stub(x => x.Session).Return(sessionState);
            context.Stub(x => x.Server).Return(serverUtility);
            return context;
        }
    }
}
