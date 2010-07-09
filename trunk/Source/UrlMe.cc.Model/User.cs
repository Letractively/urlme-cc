using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace UrlMe.cc.Model
{
    [Serializable]
    public class User
    {
        public static int NewUser(string email, string password, string passwordHint)
        {
            int ret = -1; // init to -1, assuming insert fails

            byte[] hashedBytes;
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();

            hashedBytes = hasher.ComputeHash(encoder.GetBytes(password));

            using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            {
                Data.User u = new UrlMe.cc.Data.User();
                u.Email = email;
                u.Password = hashedBytes;
                u.PasswordHint = passwordHint;
                db.Users.InsertOnSubmit(u);

                try
                {
                    db.SubmitChanges();
                    ret = db.Users.Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower()).SingleOrDefault().UserId;
                }
                catch
                {
                    // silent, leave ret = -1
                }
            }

            return ret;
        } // NewUser

        public static int AuthenticateUser(string email, string password)
        {
            int ret = -1; // init to -1, assuming no match in user table

            // hash password var's
            byte[] hashedBytes;
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();

            hashedBytes = hasher.ComputeHash(encoder.GetBytes(password));

            using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            {
                try
                {
                    ret = db.Users.Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Password == hashedBytes).SingleOrDefault().UserId;
                }
                catch
                {
                    // silent, leave ret = -1
                }
            }
           
            return ret;
        } // AuthenticateUser
    }
}
