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

        //public void OpenConnection() { }
        //public void CloseConnection() { } 

        public static DataTable fillDataTable(OracleCommand cmd)
        {
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable tab = new DataTable();
            da.Fill(tab);

            return tab;
        }

        public List<BookingShowtime> getShowDaysByIdMovie(int? idMovie)
        {
            List<BookingShowtime> dateTimes = new List<BookingShowtime>();
            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                    "SELECT start_time " +
                    "FROM show_time " +
                    "WHERE id_movie = :idMovieParam " +
                    "GROUP BY start_time ORDER BY start_time "
                    , conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("idMovieParam", idMovie);

                var tab = fillDataTable(cmd);

                //dateTimes = tab.Rows
                //            .Cast<DataRow>()
                //            .Select(r => r.Field<DateTime>(tab.Columns.Cast<DataColumn>()
                //            .FirstOrDefault(c => c.DataType == typeof(DateTime))?.ColumnName)).ToList();

                dateTimes = (from DataRow row in tab.Rows
                          select new BookingShowtime()
                          {
                              startTime = Convert.ToDateTime(row["start_time"]),
                          }).ToList();

                transaction.Rollback();
                conn.Close();

                return dateTimes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return dateTimes;
            }
        }



        public List<BookingShowtime> GetCitiesByIdMovie(int? idMovie)
        {
            List<BookingShowtime> cities = new List<BookingShowtime>();

            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                    "SELECT city " +
                    "FROM show_time A JOIN room B ON A.ID_ROOM = B.id_room " +
                    "JOIN movie_theater C ON B.id_movie_theater = C.id_movie_theater " +
                    "WHERE TRUNC(start_time) = TO_DATE('30/04/2023','DD/MM/YYYY') and id_movie = :idMovieParam " +
                    "GROUP BY city ORDER BY city "
                    , conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("idMovieParam", idMovie);

                var tab = fillDataTable(cmd);

                cities = (from DataRow row in tab.Rows
                          select new BookingShowtime()
                          {
                              City = Convert.ToString(row["city"]),

                          }).ToList();

                transaction.Rollback();
                conn.Close();

                return cities;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return cities;
            }
        }

        public List<BookingShowtime> getTypeCinemaByIdMovie(int? idMovie)
        {
            List<BookingShowtime> types = new List<BookingShowtime>();
            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                    "SELECT type " +
                    "FROM show_time " +
                    "WHERE id_movie = :idMovieParam " +
                    "GROUP BY type ORDER BY type"
                    , conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("idMovieParam", idMovie);

                var tab = fillDataTable(cmd);

                types = (from DataRow row in tab.Rows
                          select new BookingShowtime()
                          {
                              Type = Convert.ToString(row["type"]),

                          }).ToList();

                transaction.Rollback();
                conn.Close();

                return types;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return types;
            }
        }


        public List<BookingShowtime> getBookingShowtime(int? idMovie, string cityName, DateTime showDayInput, string type )
        {
            List<BookingShowtime> showtimes = new List<BookingShowtime>();
            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                    "SELECT  ID_SHOW_TIME, name_movie_theater, " +
                    "to_char(start_time, 'HH24:MI:SS') as start_time " +
                    "FROM show_time A JOIN room B ON A.ID_ROOM = B.id_room " +
                    "JOIN movie_theater C ON B.id_movie_theater = C.id_movie_theater " +
                    "WHERE TRUNC(start_time) = TO_DATE(:paramShowDay, 'MM/dd/yyyy') " +
                    "AND id_movie = :paramIdMovie AND city = :paramCityName AND type = :paramType"
                    , conn);

                cmd.BindByName = true;
                string showDay = showDayInput.ToShortDateString();
                char[] convertedShowDay = showDay.ToCharArray();
                cmd.Parameters.Add("paramShowDay", convertedShowDay);
                cmd.Parameters.Add("paramIdMovie", idMovie);
                cmd.Parameters.Add("paramCityName", cityName);
                cmd.Parameters.Add("paramType", type);

                var tab = fillDataTable(cmd);

                showtimes = (from DataRow row in tab.Rows
                             select new BookingShowtime()
                             {
                                 Id = Convert.ToInt32(row["id_show_time"]),
                                 movieTheaterName = Convert.ToString(row["name_movie_theater"]),
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

        
        public List<BookingShowtime> getShowtimeByDay(DateTime showDayInput)
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
                    "WHERE TRUNC(start_time) = TO_DATE(:paramShowDay, 'MM/dd/yyyy')"
                    , conn);

                cmd.BindByName = true;
                string showDay = showDayInput.ToShortDateString();
                char[] convertedShowDay = showDay.ToCharArray();
                cmd.Parameters.Add("paramShowDay", convertedShowDay);

                var tab = fillDataTable(cmd);

                showtimes = (from DataRow row in tab.Rows
                             select new BookingShowtime()
                             {
                                 Id = Convert.ToInt32(row["id_show_time"]),
                                 Type = Convert.ToString(row["type"]),
                                 movieTheaterName = Convert.ToString(row["name_movie_theater"]),
                                 City = Convert.ToString(row["city"]),
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


    }
}