namespace movies.Site.ViewModels.Movie
{
    public class Index
    {
        public Model.Movie Movie { get; set; }
        public bool UseAjaxForLinks { get; set; }
        public bool PrefetchLinks { get; set; }
    }
}