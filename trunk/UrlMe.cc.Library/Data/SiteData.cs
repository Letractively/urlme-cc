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
    public class SiteData
    {
        public static Database db = DatabaseFactory.CreateDatabase("Default");
        public static SqlCommand cmd;

        public static DataSet GetSiteUpdatesSiteCD(string siteCD)
        {
            DataSet ret;
            cmd = (SqlCommand)db.GetStoredProcCommand("SiteUpdateSelectBySiteCD");

            cmd.Parameters.Add("siteCD", SqlDbType.NVarChar).Value = siteCD;

            try { ret = db.ExecuteDataSet(cmd); }
            catch { return null; }

            return ret;
        } //GetSiteUpdatesSiteCD

    } // SiteData
} // namespace
