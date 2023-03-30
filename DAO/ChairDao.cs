using Movie.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Movie.DAO
{
    public class ChairDao
    {
        OracleConnection conn = null;
        public ChairDao() {
            conn = new OracleConnection(ConfigurationManager.ConnectionStrings["LOSDB"].ToString());
        }
        public static DataTable fillDataTable(OracleCommand cmd)
        {
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable tab = new DataTable();
            da.Fill(tab);

            return tab;
        }

        

        
    }
}