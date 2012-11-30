using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace Swarm.Common.Mvc.Core.ActionResults
{
    public class RssActionResult : ActionResult
    {
        public SyndicationFeed Feed { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = Resources.Constants.RssContentType;

            Rss20FeedFormatter rss = new Rss20FeedFormatter(Feed);
            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                rss.WriteTo(writer);
            }
        }
    }
}
