using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace BidForKids.Configuration
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext context, string controllerName)
        {
            Type controllerType = base.GetControllerType(controllerName);

            return ObjectFactory.GetInstance(controllerType) as IController;
        }
    }
}
