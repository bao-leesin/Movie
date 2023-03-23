using Movie.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Movie.DAO
{
    public class ShowtimeDao
    {
 

        //Convert từ datable sang list

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
            // Từng bản ghi đc lôi ra
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            //check đối tượng T
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        //********************************************************************

        public List<BookingShowtime> getShowtimeByDay(string showDay)
        {
            List<BookingShowtime> showtimes = new List<BookingShowtime>();
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
                        "WHERE TRUNC(start_time) = TO_DATE(:paramShowDay, 'DD/MM/YYYY')"
                        , conn);

                    cmd.BindByName = true;
                    char[] showDay1 = "30/1/2022".ToCharArray();
                    cmd.Parameters.Add("paramShowday", showDay1);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataTable tab = new DataTable();
                    da.Fill(tab);

                    showtimes = ConvertDataTable<BookingShowtime>(tab);

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