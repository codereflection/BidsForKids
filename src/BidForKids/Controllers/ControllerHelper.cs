using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
namespace BidsForKids.Controllers
{
    public class ControllerHelper
    {
        public static ActionResult ReturnToOrRedirectToIndex(Controller controller, int RedirectId, string RedirectParameter)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReturnTo"]) == false)
            {
                var serverUrlDecode = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["ReturnTo"]);
                if (!serverUrlDecode.StartsWith("http:") && serverUrlDecode.IndexOf("/") != 0)
                {
                    serverUrlDecode = "/" + serverUrlDecode;
                }

                serverUrlDecode += serverUrlDecode.IndexOf("?") == -1 ?
                    "?" + RedirectParameter + "=" + RedirectId 
                    : "&" + RedirectParameter + "=" + RedirectId;

                return new RedirectResult(serverUrlDecode);
            }
            else
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = controller.RouteData.Values["controller"].ToString(),
                    action = "index"
                }));
            }
        }
    }
}
