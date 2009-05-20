using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Entities
{
    class User
    {
        private int _userID;
        private string _email;
        private bool _isAdmin;
        private IList<Link> _links;

        #region Get/Setters
        public int UserID { get { return _userID; } }
        public string Email { get { return _email; } set { _email = value; } }
        public bool IsAdmin { get { return _isAdmin; } set { _isAdmin = value; } }
        // should Links get/set be readonly?
        public IList<Link> Links { get { return _links; } set { _links = value; } }
        #endregion
    }
}
