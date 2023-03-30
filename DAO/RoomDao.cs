using Movie.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Movie.DAO
{
    public class RoomDao
    {
        OracleConnection conn = null;
        public RoomDao()
        {
            conn = new OracleConnection(ConfigurationManager.ConnectionStrings["LOSDB"].ToString());
        }
        public static DataTable fillDataTable(OracleCommand cmd)
        {
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable tab = new DataTable();
            da.Fill(tab);

            return tab;
        }

        public Room getChairsOfRoom(int idShowtime)
        {
            
            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(
                                    "",
                                    conn);
                cmd.BindByName = true;
                cmd.Parameters.Add("");

                var tab = fillDataTable(cmd);

                Room room = new Room();
                room.ChairQuantity = Convert.ToInt32( tab.Rows[0]["chair_quantity"]);

                transaction.Commit();
                conn.Close();
                return room;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                transaction.Rollback();
                conn.Close();
                return null;
            }

        }

        public List<Chair> getSoldChairIdList(int idShowtime)
        {

            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand("", conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("paramIdShowTime", idShowtime);

                DataTable tab = fillDataTable(cmd);
                List<Chair> chairList = new List<Chair>();
                chairList = (from DataRow row in tab.Rows
                             select new Chair
                             {
                                 Id = Convert.ToInt32(row["ID"])
                             }).ToList();

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