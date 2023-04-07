namespace Dignite.Cms.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class EntryPage
    {
        protected EntryPage()
        {
        }

        public EntryPage(string route, string template)
        {
            Route = route;
            Template = template;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Template { get; set; }
    }
}
