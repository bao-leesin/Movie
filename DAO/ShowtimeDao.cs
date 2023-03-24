using Microsoft.Ajax.Utilities;
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


        ////Convert từ datable sang list 

        //private static List<T> ConvertDataTable<T>(DataTable dt)
        //{
        //    List<T> data = new List<T>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //    // Từng hàng(bản ghi) đc lôi ra
        //        T item = GetItem<T>(row);
        //        data.Add(item);
        //    }
        //    return data;
        //}
        //private static T GetItem<T>(DataRow dr)
        //{
        //    //check lớp đối tượng T
        //    Type temp = typeof(T);
        //    T obj = Activator.CreateInstance<T>();

        //    //Duyệt qua từng cột của 1 hàng và từng biến của lớp T, nếu khớp thì SetValue = Rows[tên cột]
        //    //Thuật toán này n2, thêm 1 for ở DT nên khả năng là chậm. Cộng với việc tên cột phải == tên thuộc tính 
        //    // => ngắn nhưng cân nhắc dùng ở trg hợp khác

        //    foreach (DataColumn column in dr.Table.Columns)
        //    {
        //        foreach (PropertyInfo pro in temp.GetProperties())
        //        {
        //            if (pro.Name == column.ColumnName)
        //                pro.SetValue(obj, dr[column.ColumnName], null);
        //            else
        //                continue;
        //        }
        //    }
        //    return obj;
        //}

        //********************************************************************
        OracleConnection conn = null;

        public ShowtimeDao()
        {
             conn = new OracleConnection(ConfigurationManager.ConnectionStrings["LOSDB"].ToString());
        }

        public List<BookingShowtime> getBookingShowtimeById(int? idMovie)
        {
            List<BookingShowtime> showtimes = new List<BookingShowtime>();
            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                    "SELECT  A.ID_SHOW_TIME, A.type, to_char(A.start_time, 'HH24:MI:SS') as start_time, " +
                    "B.name_room, C.city, C.name_movie_theater " +
                    "FROM show_time A JOIN room B ON A.ID_ROOM = B.id_room " +
                    "JOIN movie_theater C ON B.id_movie_theater = C.id_movie_theater " +
                    "WHERE TRUNC(start_time) = TRUNC(sysdate) and id_movie = :idMovieParam"
                    , conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("idMovieParam", idMovie);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable tab = new DataTable();
                da.Fill(tab);

                showtimes = (from DataRow row in tab.Rows
                             select new BookingShowtime()
                             {
                                 id = Convert.ToInt32(row["id_show_time"]),
                                 type = Convert.ToString(row["type"]),
                                 movieTheaterName = Convert.ToString(row["name_movie_theater"]),
                                 roomName = Convert.ToString(row["name_room"]),
                                 city = Convert.ToString(row["city"]),
                                 startTime = Convert.ToDateTime(row["start_time"]),
                             }
                            ).ToList();

                transaction.Rollback();
                conn.Close();

                return showtimes;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return showtimes;
            }
        }


        public List<BookingShowtime> getShowtimeByDay(string showDay)
        {
            List<BookingShowtime> showtimes = new List<BookingShowtime>();
            try
            {
                    conn.Open();
                    var transaction = conn.BeginTransaction();

                    OracleCommand cmd = new OracleCommand(
                        "SELECT  A.ID_SHOW_TIME, A.type, to_char(A.start_time, 'HH24:MI:SS') as start_time, " +
                        "B.name_room, C.city, C.name_movie_theater " +
                        "FROM show_time A JOIN room B ON A.ID_ROOM = B.id_room " +
                        "JOIN movie_theater C ON B.id_movie_theater = C.id_movie_theater " +
                        "WHERE TRUNC(start_time) = TO_DATE(:paramShowDay, 'dd/MM/yyyy')"
                        , conn);

                    cmd.BindByName = true;
                    char[] convertedShowDay = showDay.ToCharArray();
                    cmd.Parameters.Add("paramShowDay", convertedShowDay);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataTable tab = new DataTable();
                    da.Fill(tab);

                    showtimes = (from DataRow row in tab.Rows
                                 select new BookingShowtime()
                                 {
                                    id = Convert.ToInt32(row["id_show_time"]),
                                    type = Convert.ToString(row["type"]),
                                    movieTheaterName = Convert.ToString(row["name_movie_theater"]),
                                    roomName = Convert.ToString(row["name_room"]),
                                    city = Convert.ToString(row["city"]),
                                    startTime = Convert.ToDateTime(row["start_time"]),
                                 }
                                ).ToList();

                    transaction.Rollback();
                    conn.Close();

                    return showtimes ;
                }
            
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return showtimes;
            }
        }

        public List<City> GetCitiesByIdFilm(int id)
        {
            List<City> cities = new List<City>();

            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                    "SELECT  C.city FROM show_time A JOIN room B ON A.ID_ROOM = B.id_room " +
                    "JOIN movie_theater C ON B.id_movie_theater = C.id_movie_theater " +
                    "WHERE TRUNC(start_time) = TO_DATE('30/04/2023','DD/MM/YYYY') and id_movie = :idMovieParam group by city "
                    ,conn);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable tab = new DataTable();
                da.Fill(tab);

                cities = (from DataRow row in tab.Rows
                          select new City()
                          {
                              NameCity = Convert.ToString(row["city"]),

                          }).ToList();

                return cities;
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return cities;
            }
        }
         
    }
}