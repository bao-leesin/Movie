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
    public class RoomDao : Dao
    {
        public RoomDao()
        {
        }

        public int getChairsOfRoom(int idShowtime)
        {
            
            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(
                                    "SELECT chair_quantity from room WHERE id_room = " +
                                    "(select id_room from show_time where id_show_time = :paramIdShowtime)",
                                    conn);
                cmd.BindByName = true;
                cmd.Parameters.Add("paramIdShowtime",idShowtime);

                var tab = fillDataTable(cmd);

        
                int ChairQuantity = Convert.ToInt32( tab.Rows[0]["chair_quantity"]);

                transaction.Commit();
                conn.Close();
                return ChairQuantity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                transaction.Rollback();
                conn.Close();
                return 0;
            }

        }

        
    }
}