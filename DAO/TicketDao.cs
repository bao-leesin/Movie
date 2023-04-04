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
    public class TicketDao
    {
        OracleConnection conn = null;

        public TicketDao()
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

        public bool InsertTicket(Ticket ticket)
        {
            try 
            { 
                conn.Open();
                var transaction = conn.BeginTransaction();
                try
                {
                    OracleCommand cmd = new OracleCommand(
                        "INSERT INTO ticket(username,id_show_time,price,id_chair) " +
                        "VALUES(:paramUsername,:paramIdShowtime,:paramPrice,:paramIdChair)", 
                        conn);

                    List<int> idChairs = ticket.IdChair;

                    cmd.BindByName = true;
                    cmd.Parameters.Add("paramUsername",ticket.Username);
                    cmd.Parameters.Add("paramIdShowtime", ticket.idShowtime);
                    cmd.Parameters.Add("paramPrice", ticket.Price);

                    foreach(int chair in idChairs)
                    {
                        cmd.Parameters.Add("paramIdChair", chair);
                        cmd.ExecuteNonQuery();
                    }
                    //int result = cmd.ExecuteNonQuery();

                    //if (result != 0){
                    //transaction.Commit();
                    //return true;
                    //}
                    //else{
                    //transaction.Rollback();
                    //return false;
                    //}

                   transaction.Rollback();
                    return true;
                }catch
                {
                    transaction.Rollback();
                    return false;
                }
            } catch
            {
               return false;
            }
            finally {
                conn.Close();
            }
           

        }


    }
}