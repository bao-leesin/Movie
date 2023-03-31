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

        public List<String> getTierChair( )
        {
            conn.Open();
            var transaction = conn.BeginTransaction();

            try
            {
                OracleCommand cmd = new OracleCommand("SELECT tier_chair FROM chair_price order by 1",conn);
                
                cmd.BindByName = true;

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

        public List<Chair> getChairPrice( ) {
            conn.Open();
            var transaction = conn.BeginTransaction();

            try
            {
                OracleCommand cmd = new OracleCommand("SELECT tier_chair,base_price FROM chair_price order by 1", conn);

                cmd.BindByName = true;

                var tab = fillDataTable(cmd);

                List<Chair> tierList = (from DataRow dr in tab.Rows
                                        select new Chair
                                        {
                                            Tier = dr["tier_chair"].ToString(),
                                            OverSpending = Convert.ToInt32(dr["BASE_PRICE"])
                                        }
                                        ).ToList();

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
                OracleCommand cmd = new OracleCommand("select id_chair from ticket where id_show_time = :paramIdShowtime", conn);

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

        public List<string> getChairsByTier( string tier)
        {

            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand("select id_chair from chair where tier_chair = :paramTier", conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("paramTier", tier);

                DataTable tab = fillDataTable(cmd);

                List<string> chairList = tab.AsEnumerable().Select(x => (x[0]).ToString()).ToList();

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