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

        public List<String> getTierChair(int idShowtime)
        {
            conn.Open();
            var transaction = conn.BeginTransaction();

            try
            {
                OracleCommand cmd = new OracleCommand("",conn);
                
                cmd.BindByName = true;
                cmd.Parameters.Add("", idShowtime);

                var tab = fillDataTable(cmd);

                List<String> tierList = tab.AsEnumerable().Select(x => x[0].ToString()).ToList();

                transaction.Commit();
                conn.Close();

                return tierList;

            }
            catch (Exception ex)
            {
                ex.ToString();
                transaction.Rollback();
                conn.Close();
                return null;
            }
        }

        public List<int> getSoldChairList(int idShowtime)
        {

            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand("", conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("paramIdShowTime", idShowtime);

                DataTable tab = fillDataTable(cmd);

                List<int> chairList = tab.AsEnumerable().Select(x => Convert.ToInt32(x[0])).ToList();

                transaction.Commit();
                conn.Close();
                return chairList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                transaction.Rollback();
                conn.Close();
                return null;
            }
        }

        public List<int> getChairsByTier(int idShowtime, string tier)
        {

            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand("", conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("paramIdShowTime", idShowtime);

                DataTable tab = fillDataTable(cmd);

                List<int> chairList = tab.AsEnumerable().Select(x => Convert.ToInt32(x[0])).ToList();

                transaction.Commit();
                conn.Close();
                return chairList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                transaction.Rollback();
                conn.Close();
                return null;
            }
        }



    }
}