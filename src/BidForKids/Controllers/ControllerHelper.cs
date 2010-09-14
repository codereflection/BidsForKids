using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
namespace BidForKids.Controllers
{
    public class ControllerHelper
    {
        /// <summary>
        /// Redirects to ReturnTo query string value, passing RedirectParameter=RedirectId, else redirects to index
        /// </summary>
        /// <param name="RedirectId">New ID to pass back to ReturnTo url</param>
        /// <returns></returns>
        public static ActionResult ReturnToOrRedirectToIndex(Controller controller, int RedirectId, string RedirectParameter)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReturnTo"]) == false)
            {
                string lServerUrlDecode = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["ReturnTo"]);
                if (lServerUrlDecode.IndexOf("http:") == -1 && lServerUrlDecode.IndexOf("/") != 0)
                {
                    lServerUrlDecode = "/" + lServerUrlDecode;
                }

                lServerUrlDecode += lServerUrlDecode.IndexOf("?") == -1 ? "?" + RedirectParameter + "=" + RedirectId.ToString() : "&" + RedirectParameter + "=" + RedirectId.ToString();

                return new RedirectResult(lServerUrlDecode);
            }
            else
            {
                //controller.RouteData.Values["controller"].ToString()
                return new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = controller.RouteData.Values["controller"].ToString(),
                    action = "index"
                }));
            }
        }
    }
}
