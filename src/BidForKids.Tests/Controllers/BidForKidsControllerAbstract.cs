using System;
using System.Linq;
using BidsForKids.Data.Models;
using System.Web.Mvc;
using BidsForKids.Controllers;
using System.Web.Routing;
using System.Collections.Specialized;
using NSubstitute;

namespace BidsForKids.Tests.Controllers
{
    public class BidsForKidsControllerTestBase
    {
        static public IProcurementRepository ProcurementFactory;
        public BidsForKidsControllerTestBase()
        {
            ProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
        }

        public static T SetupNewControllerWithMockContext<T>()
            where T : BidsForKidsControllerBase, new()
        {
            var controller = new T();
            controller.ControllerContext =
                new ControllerContext(StubContext.FakeHttpContext(), new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(controller.HttpContext, new RouteData()));

            return controller;
        }

        public static T SetupNewControllerWithMockContext<T>(IProcurementRepository factory)
            where T : BidsForKidsControllerBase, new()
        {
            var controller = new T { Repository = factory };
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

                var parts = queryString.Split("?".ToCharArray());
                var keys = parts[1].Split("&".ToCharArray());

                foreach (var part in keys.Select(key => key.Split("=".ToCharArray())))
                {
                    parameters.Add(part[0], part[1]);
                }

                controller.ControllerContext.HttpContext.Request.QueryString.Returns(parameters);
                    
                return (T)controller;
        }
    }
}
