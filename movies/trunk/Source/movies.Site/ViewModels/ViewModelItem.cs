namespace movies.Site.ViewModels
{
    public class ViewModelItem<T> : ViewModelBase
    {
        public T Item { get; set; }
    }
}