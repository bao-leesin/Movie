using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Movie.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace Movie.DAO
{
    public class AccountDao
    {
        public AccountDao() {
           
        }
        public int LoginUser(string username, string password)
        {
            try
            {
                //var cn = ConfigurationManager.ConnectionStrings["LOSDB"].ToString();
                using (var conn = new OracleConnection(ConfigurationManager.ConnectionStrings["LOSDB"].ToString()))
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();

                    OracleCommand cmd = new OracleCommand(
                        "SELECT ROLE_ID " +
                        "FROM account " +
                        "WHERE  (username = :paramUsername) AND (password = :paramPassword)"
                        , conn);

                    cmd.BindByName = true;
                    cmd.Parameters.Add("paramUsername", username);
                    cmd.Parameters.Add("paramPassword", password);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataTable tab = new DataTable();

                    da.Fill(tab);
                    var rowNumber = tab.Rows.Count;

                    transaction.Rollback();
                    conn.Close();

                    return (rowNumber == 0) ? 0 : (int)tab.Rows[0]["ROLE_ID"]; 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public string getRole(string username)
        {
            try
            {
                //var cn = ConfigurationManager.ConnectionStrings["LOSDB"].ToString();
                using (var conn = new OracleConnection(ConfigurationManager.ConnectionStrings["LOSDB"].ToString()))
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();

                    OracleCommand cmd = new OracleCommand(
                        "SELECT ROLE_NAME " +
                        "FROM account JOIN role ON account.role_id = role.role_id " +
                        "WHERE  (username = :paramUsername)"
                        , conn);

                    cmd.BindByName = true;
                    cmd.Parameters.Add("paramUsername", username);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataTable tab = new DataTable();

                    da.Fill(tab);
                    var rowNumber = tab.Rows.Count;

                    transaction.Rollback();
                    conn.Close();

                    return (rowNumber == 0) ? null : tab.Rows[0]["ROLE_NAME"].ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
