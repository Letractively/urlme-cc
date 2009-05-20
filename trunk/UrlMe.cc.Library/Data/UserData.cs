using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace Library.Data
{
    public class UserData
    {
        public static Database db = DatabaseFactory.CreateDatabase("Default");
        public static SqlCommand cmd;

        public static int NewUser(string email, string password, string passwordHint)
        {
            int retUserId = -1; // init to -1, assuming insert fails

            cmd = (SqlCommand)db.GetStoredProcCommand("UserInsert");
            // hash password var's
            byte[] hashedBytes;
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();

            hashedBytes = hasher.ComputeHash(encoder.GetBytes(password));
            
            // add in params
            cmd.Parameters.Add("email",SqlDbType.NVarChar).Value = email;
            cmd.Parameters.Add("password", SqlDbType.Binary).Value = hashedBytes;
            cmd.Parameters.Add("passwordHint", SqlDbType.NVarChar).Value = passwordHint;
            
            // out param for user id
            SqlParameter outParam = new SqlParameter("userId", SqlDbType.Int, 4);
            outParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outParam);

            db.ExecuteNonQuery(cmd);
            retUserId = (int)cmd.Parameters["userId"].Value;

            return retUserId;
        } //NewUser

        public static int AuthenticateUser(string email, string password)
        {
            int retUserId = -1; // init to -1, assuming no match in user table
            
            cmd = (SqlCommand)db.GetStoredProcCommand("UserAuthenticate");
            // hash password var's
            byte[] hashedBytes;
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();

            hashedBytes = hasher.ComputeHash(encoder.GetBytes(password));

            cmd.Parameters.Add("email", SqlDbType.NVarChar).Value = email;
            cmd.Parameters.Add("password", SqlDbType.Binary).Value = hashedBytes;

            // out param for user id
            SqlParameter outParam = new SqlParameter("userId", SqlDbType.Int, 4);
            outParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outParam);
    
            db.ExecuteNonQuery(cmd);
            retUserId = (int)cmd.Parameters["userId"].Value;
            
            return retUserId;
        } // AuthenticateUser
    } // UserData
} // namespace
