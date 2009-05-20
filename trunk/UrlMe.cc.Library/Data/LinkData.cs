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
    public class LinkData
    {
        public static Database db = DatabaseFactory.CreateDatabase("Default");
        public static SqlCommand cmd;

        public static int NewLink(int userID, string path, string destinationUrl, bool publicInd, DateTime expirationDate, string description)
        {
            cmd = (SqlCommand)db.GetStoredProcCommand("LinkInsert");

            cmd.Parameters.Add("userID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("path", SqlDbType.NVarChar).Value = path;
            cmd.Parameters.Add("destinationUrl", SqlDbType.NVarChar).Value = destinationUrl;
            cmd.Parameters.Add("description", SqlDbType.NVarChar).Value = description;
            cmd.Parameters.Add("publicInd", SqlDbType.Bit).Value = publicInd;
            cmd.Parameters.Add("expirationDate", SqlDbType.SmallDateTime).Value = expirationDate;

            try { db.ExecuteNonQuery(cmd); }
            catch { return -1; }

            return 0;
        } //NewLink

        public static int NewLink(int userID, string path, string destinationUrl, bool publicInd, DateTime expirationDate)
        {
            cmd = (SqlCommand)db.GetStoredProcCommand("LinkInsert");

            cmd.Parameters.Add("userID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("path", SqlDbType.NVarChar).Value = path;
            cmd.Parameters.Add("destinationUrl", SqlDbType.NVarChar).Value = destinationUrl;
            cmd.Parameters.Add("publicInd", SqlDbType.Bit).Value = publicInd;
            cmd.Parameters.Add("expirationDate", SqlDbType.SmallDateTime).Value = expirationDate;

            try { db.ExecuteNonQuery(cmd); }
            catch { return -1; }

            return 0;
        } //NewLink

        public static DataSet GetLinksByUserID(int userID)
        {
            DataSet ret;
            cmd = (SqlCommand)db.GetStoredProcCommand("LinkSelectByUserID");

            cmd.Parameters.Add("userID", SqlDbType.Int).Value = userID;

            try { ret = db.ExecuteDataSet(cmd); }
            catch { return null; }

            return ret;
        } //NewUser

        public static int ToggleActiveInd(int linkID)
        {
            cmd = (SqlCommand)db.GetStoredProcCommand("LinkUpdateToggleActiveInd");

            cmd.Parameters.Add("linkID", SqlDbType.Int).Value = linkID;

            try { db.ExecuteNonQuery(cmd); }
            catch { return -1; }

            return 0;
        } //ToggleActiveInd

        public static int TogglePublicInd(int linkID)
        {
            cmd = (SqlCommand)db.GetStoredProcCommand("LinkUpdateTogglePublicInd");

            cmd.Parameters.Add("linkID", SqlDbType.Int).Value = linkID;

            try { db.ExecuteNonQuery(cmd); }
            catch { return -1; }

            return 0;
        } //NewLink

        public static string LookupPath(string path)
        {
            string retDestinationUrl = ""; 

            cmd = (SqlCommand)db.GetStoredProcCommand("LinkSelectDestinationUrlByPath");

            cmd.Parameters.Add("path", SqlDbType.NVarChar).Value = path;

            try { retDestinationUrl = (string)db.ExecuteScalar(cmd); }
            catch { /* nothing, leave return val as blank */ }
            return retDestinationUrl;
        } // AuthenticateUser
    } // UserData
} // namespace
