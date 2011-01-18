using System.Web.Routing;
using Xunit;

namespace BidsForKids.Tests.Routes
{
    public class RouteFacts
    {
        [Fact]
        public void RouteWithControllerNoActionNoId()
        {
            // Arrange
            var context = new StubContext("~/controller1");
            var routes = new RouteCollection();
            WebApplication.RegisterRoutes(routes);

            // Act
            var routeData = routes.GetRouteData(context);

            // Assert
            Assert.NotNull(routeData);
            Assert.Equal("controller1", routeData.Values["controller"]);
            Assert.Equal("Index", routeData.Values["action"]);
            Assert.Equal("", routeData.Values["id"]);
        }

        [Fact]
        public void RouteWithControllerWithActionNoId()
        {
            // Arrange
            var context = new StubContext("~/controller1/action2");
            var routes = new RouteCollection();
            WebApplication.RegisterRoutes(routes);

            // Act
            var routeData = routes.GetRouteData(context);

            // Assert
            Assert.NotNull(routeData);
            Assert.Equal("controller1", routeData.Values["controller"]);
            Assert.Equal("action2", routeData.Values["action"]);
            Assert.Equal("", routeData.Values["id"]);
        }

        [Fact]
        public void RouteWithControllerWithActionWithId()
        {
            // Arrange
            var context = new StubContext("~/controller1/action2/id3");
            var routes = new RouteCollection();
            WebApplication.RegisterRoutes(routes);

            // Act
            RouteData routeData = routes.GetRouteData(context);

            // Assert
            Assert.NotNull(routeData);
            Assert.Equal("controller1", routeData.Values["controller"]);
            Assert.Equal("action2", routeData.Values["action"]);
            Assert.Equal("id3", routeData.Values["id"]);
        }

        [Fact]
        public void RouteWithTooManySegments()
        {
            // Arrange
            var context = new StubContext("~/a/b/c/d");
            var routes = new RouteCollection();
            WebApplication.RegisterRoutes(routes);

            // Act
            var routeData = routes.GetRouteData(context);

            // Assert
            Assert.Null(routeData);
        }

        [Fact]
        public void RouteForEmbeddedResource()
        {
            // Arrange
            var context = new StubContext("~/foo.axd/bar/baz/biff");
            var routes = new RouteCollection();
            WebApplication.RegisterRoutes(routes);

            // Act
            var routeData = routes.GetRouteData(context);

            // Assert
            Assert.NotNull(routeData);
            Assert.IsAssignableFrom<StopRoutingHandler>(routeData.RouteHandler);
        }
    }
}