using System;
using BidsForKids.Models;
using System.Web.Mvc;
using BidsForKids.Controllers;
using System.Web.Routing;
using Rhino.Mocks;
using System.Collections.Specialized;

namespace BidsForKids.Tests.Controllers
{
    public class BidsForKidsControllerTestBase
    {
        static public IProcurementRepository _ProcurementFactory;
        public BidsForKidsControllerTestBase()
        {
            _ProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
        }

        public static T SetupNewControllerWithMockContext<T>(IProcurementRepository factory)
            where T : BidsForKidsControllerBase, new()
        {
            var controller = new T() { factory = factory };
            controller.ControllerContext =
                new ControllerContext(StubContext.FakeHttpContext(), new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(controller.HttpContext, new RouteData()));

            return controller;
        }

        public static T SetupQueryStringParameters<T>(Controller controller, string queryString) where T : BidsForKidsControllerBase
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            if (string.IsNullOrEmpty(queryString))
                return null;

            if (!queryString.Contains("?"))
            {
                queryString = "?" + queryString;
            }
            var parameters = new NameValueCollection();

                string[] parts = queryString.Split("?".ToCharArray());
                string[] keys = parts[1].Split("&".ToCharArray());

                foreach (string key in keys)
                {
                    string[] part = key.Split("=".ToCharArray());
                    parameters.Add(part[0], part[1]);
                }

                controller.ControllerContext.HttpContext.Request.Expect(x => x.QueryString).Return(parameters).Repeat.Any();
                    
                return (T)controller;
        }
    }
}
