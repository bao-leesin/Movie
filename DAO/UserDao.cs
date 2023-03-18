using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Movie.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Movie.Common;



namespace Movie.DAO
{
    public class UserDao
    {
        public UserDao() {
           
        }

        public bool LoginUser(string username, string password)
        {
            try
            {
                //var cn = ConfigurationManager.ConnectionStrings["LOSDB"].ToString();
                using (var conn = new OracleConnection(ConfigurationManager.ConnectionStrings["LOSDB"].ToString()))
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();

                    OracleCommand cmd = new OracleCommand(
                        "SELECT USERNAME " +
                        "FROM ACCOUNT " +
                        "WHERE  (USERNAME = :username) AND (PASSWORD = :password)"
                        , conn);

                    cmd.BindByName = true;
                    cmd.Parameters.Add("username", username);
                    cmd.Parameters.Add("password", password);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataTable tab = new DataTable();

                    da.Fill(tab);
                    var rowNumber = tab.Rows.Count;

                    transaction.Rollback();
                    conn.Close();
                    return (rowNumber == 0) ? false : true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
}
    }
}
