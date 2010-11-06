using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Entities
{
    public class Link
    {
        public Link() { }
        
        #region Properties
        private int _linkID = -1;
        private string _path = null;
        private string _destinationUrl = null;
        private string _description = null;
        private DateTime _expirationDate = DateTime.Now;
        private bool _isPublic = false;
        private bool _isActive = false;
        #endregion

        #region Get/Setters
        public int LinkID { get { return _linkID; } }
        public string Path { get { return _path; } set { _path = value; } }
        public string DestinationUrl { get { return _destinationUrl; } set { _destinationUrl = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public DateTime ExpirationDate { get { return _expirationDate; } set { _expirationDate = value; } }
        public bool IsPublic { get { return _isPublic; } set { _isPublic = value; } }
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
        #endregion

    }
}
