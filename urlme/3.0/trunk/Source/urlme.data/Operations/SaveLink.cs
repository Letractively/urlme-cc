namespace urlme.data.Operations
{
    public class SaveLink
    {
        public Enumerations.SaveLinkResult Result { get; set; }
        public data.Models.Link Item { get; set; }
        public bool HasError
        {
            get
            {
                return this.Result != Enumerations.SaveLinkResult.Success;
            }
        }

        public SaveLink()
        {
            this.Result = Enumerations.SaveLinkResult.Success;
        }
    }
}
