namespace urlme.Utils.Configuration
{
    /// <summary>
    /// Provides access to configuration data related to Users.
    /// </summary>
    public static class User
    {
        /// <summary>
        /// Gets the url to the image to use when a user does not have a profile photo.
        /// </summary>
        public static string NoProfilePlaceholderImage
        {
            get { return ConfigurationManager.Instance.AppSettings["NoProfilePlaceholderImage"]; }
        }

        /// <summary>
        /// Gets the maximum times a user can be marked as offensive before being marked for review.
        /// </summary>
        public static int MaxUserOffensiveCount
        {
            get
            {
                int val = 3;
                int.TryParse(ConfigurationManager.Instance.AppSettings["MaxUserOffensiveCount"], out val);
                return val;
            }
        }

        /// <summary>
        /// Gets the maximum height a user's profile image may be.
        /// </summary>
        public static int ProfileImageMaxHeight
        {
            get
            {
                int val = 120;
                int.TryParse(ConfigurationManager.Instance.AppSettings["ProfileImageMaxHeight"], out val);
                return val;
            }
        }

        /// <summary>
        /// Gets the maximum width a user's profile image may be.
        /// </summary>
        public static int ProfileImageMaxWidth
        {
            get
            {
                int val = 120;
                int.TryParse(ConfigurationManager.Instance.AppSettings["ProfileImageMaxWidth"], out val);
                return val;
            }
        }

        /// <summary>
        /// Gets the number of uploads/favorites to show at a time for the my channel page.
        /// </summary>
        public static int MyChannelMediaCount
        {
            get
            {
                int val = 5;
                int.TryParse(ConfigurationManager.Instance.AppSettings["MyChannelMediaCount"], out val);
                return val;
            }
        }

        /// <summary>
        /// Gets the number of comments to show at a time for the my channel page.
        /// </summary>
        public static int MyChannelCommentCount
        {
            get
            {
                int val = 5;
                int.TryParse(ConfigurationManager.Instance.AppSettings["MyChannelCommentCount"], out val);
                return val;
            }
        }

        /// <summary>
        /// Gets the maximum times a user's comment can be marked as offensive before being marked for review.
        /// </summary>
        public static int MaxUserCommentOffensiveCount
        {
            get
            {
                int val = 3;
                int.TryParse(ConfigurationManager.Instance.AppSettings["MaxUserCommentOffensiveCount"], out val);
                return val;
            }
        }

        /// <summary>
        /// Gets the desired height for a user profile image.
        /// </summary>
        public static int UserProfileHeight
        {
            get 
            { 
                int val = 45;
                int.TryParse(ConfigurationManager.Instance.AppSettings["UserProfileHeight"], out val);
                return val;
            }
        }

        /// <summary>
        /// Gets the desired width for a user profile image.
        /// </summary>
        public static int UserProfileWidth
        {
            get
            {
                int val = 45;
                int.TryParse(ConfigurationManager.Instance.AppSettings["UserProfileWidth"], out val);
                return val;
            }
        }

        /// <summary>
        /// Gets the number of users to display in the user bar (subscriptions/subscribers on the my channel page).
        /// </summary>
        public static int UserSectionCount
        {
            get
            {
                int val = 8;
                int.TryParse(ConfigurationManager.Instance.AppSettings["UserSectionCount"], out val);
                return val;
            }
        }
    }
}
