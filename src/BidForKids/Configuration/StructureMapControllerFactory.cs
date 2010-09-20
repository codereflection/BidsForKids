using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace BidsForKids.Configuration
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext context, string controllerName)
        {
            var controllerType = base.GetControllerType(context, controllerName);

            return ObjectFactory.GetInstance(controllerType) as IController;
        }
    }
}
