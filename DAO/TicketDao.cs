using Movie.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;


namespace Movie.DAO
{
    public class TicketDao : Dao
    {
     

        public TicketDao(){}


        public List<int> InsertTicket(Ticket ticket)
        {
            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                try
                {
                    OracleCommand cmd = new OracleCommand(
                        "INSERT INTO ticket(username,id_show_time,price,id_chair) " +
                        "VALUES(:paramUsername,:paramIdShowtime,:paramPrice,:paramIdChair) " ,
                        conn);

                    List<int> idChairs = ticket.IdChair;
                    List<int> idTickets = new List<int>();

                    cmd.BindByName = true;
                    cmd.Parameters.Add("paramUsername", ticket.Username);
                    cmd.Parameters.Add("paramIdShowtime", ticket.idShowtime);
                    cmd.Parameters.Add("paramPrice", ticket.Price);

                    foreach (int chair in idChairs)
                    {
                        cmd.Parameters.Add("paramIdChair", chair);
                        cmd.ExecuteNonQuery();

                        OracleCommand cmd2 = new OracleCommand(
                            "SELECT id_ticket FROM ticket ORDER BY id_ticket desc fetch first 1 row only",
                            conn);

                        DataTable tab = fillDataTable(cmd2);
                        int id = Convert.ToInt32(tab.Rows[0]["id_ticket"]);
                        idTickets.Add(id);
                    }

                    transaction.Commit();
                    return idTickets;
                }
                catch(Exception e)
                {
                    e.Message.ToString();
                    transaction.Rollback();
                    return null;
                }
            }
            catch
            {
                return null;
            } 
            finally
            {
                conn.Close();
            }
        }

        public Ticket SelectTicketByID(int idTicket)
        {
            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                    "SELECT username,name_movie,name_movie_theater,name_room,id_chair,duration,price,start_time,type,city " +
                    "FROM ticket tk " +
                    "JOIN show_time st ON tk.id_show_time = st.id_show_time " +
                    "JOIN movie mv ON mv.id_movie = st.id_movie " +
                    "JOIN room rm ON rm.id_room = st.id_room " +
                    "JOIN movie_theater mt ON mt.id_movie_theater = rm.id_movie_theater " +
                    "where id_ticket = :paramIdTicket", 
                    conn);

                cmd.BindByName = true;
                cmd.Parameters.Add(":paramIdTicket", idTicket);

                DataTable tab = fillDataTable(cmd);
                var row = tab.Rows[0];

                Ticket ticket = new Ticket()
                {
                    Username = Convert.ToString( row["username"]),
                    MovieName = Convert.ToString(row["name_movie"]),
                    MovieTheaterName = Convert.ToString(row["name_movie_theater"]),
                    StartTime = Convert.ToDateTime(row["start_time"]),
                    RoomName = Convert.ToString(row["name_room"]),
                    Price = Convert.ToInt32(row["price"]),
                    Type = Convert.ToString(row["type"]),
                    City = Convert.ToString(row["city"]),
                    ChairNumber= Convert.ToInt32(row["id_chair"]),
                    Duration = Convert.ToInt32(row["duration"])
                };

                return ticket;

            } catch(Exception e)
            {
                e.Message.ToString();
                return null;
            }

        }


    }
}