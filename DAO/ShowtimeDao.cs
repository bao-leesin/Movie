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
    public class ShowtimeDao
    {
        public List<Showtime> getShowtimeByDay(DateTime showDay)
        {
            var showtimes = new List<Showtime>();
            try
            {
                //var cn = ConfigurationManager.ConnectionStrings["LOSDB"].ToString();
                using (var conn = new OracleConnection(ConfigurationManager.ConnectionStrings["LOSDB"].ToString()))
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();

                    OracleCommand cmd = new OracleCommand(
                        "SELECT  A.ID_SHOW_TIME, A.type, to_char(A.start_time, 'HH24:MI:SS'), C.city " +
                        "FROM show_time A JOIN room B ON A.ID_ROOM = B.id_room " +
                        "JOIN movie_theater C ON B.id_movie_theater = C.id_movie_theater " +
                        "WHERE TRUNC(start_time) = TO_DATE(':paramShowDay', 'DD/MM/YYYY')"
                        , conn);

                    cmd.BindByName = true;
                    cmd.Parameters.Add("paramShowday", showDay);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataTable tab = new DataTable();

                    da.Fill(tab);
                    tab.Rows

                    transaction.Rollback();
                    conn.Close();

                    return showtimes ;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return showtimes;
            }
        }
    }
}