namespace urlme.Utils.Web.Mvc
{
    using System.ServiceModel.Syndication;
    using System.Web.Mvc;
    using System.Xml;

    /// <summary>
    /// Provides the ability to easily return an RSS feed as an action result in MVC
    /// </summary>
    public class RssResult : ActionResult
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the RssResult class
        /// </summary>
        public RssResult() { }

        /// <summary>
        /// Initializes a new instance of the RssResult class
        /// </summary>
        /// <param name="feed">the feed to output</param>
        public RssResult(SyndicationFeed feed)
        {
            this.Feed = feed;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the feed for this result
        /// </summary>
        public SyndicationFeed Feed { get; set; } 
        #endregion

        #region Methods
        /// <summary>
        /// Handles outputting the rss feed for the action
        /// </summary>
        /// <param name="context">the context this result is called under</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            Rss20FeedFormatter formatter = new Rss20FeedFormatter(this.Feed);

            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                formatter.WriteTo(writer);
            }
        } 
        #endregion
    }
}
