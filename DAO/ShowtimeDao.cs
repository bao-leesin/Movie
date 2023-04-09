using Microsoft.Ajax.Utilities;
using Movie.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Movie.DAO
{
    public class ShowtimeDao : Dao
    {


        public ShowtimeDao()
        {
        }

        public BookingShowtime GetShowtimeDetail(int idShowtime)
        {
            conn.Open();
            var transaction = conn.BeginTransaction();
            try 
            {
                OracleCommand cmd = new OracleCommand(
                   "SELECT id_show_time,name_movie,name_movie_theater,name_room,base_price,start_time,type " +
                   "FROM show_time st JOIN movie mv ON st.id_movie = mv.id_movie " +
                   "JOIN room rm ON rm.id_room = st.id_room  " +
                   "JOIN movie_theater mt ON mt.id_movie_theater = rm.id_movie_theater " +
                   "WHERE id_show_time = :paramIdShowtime"
                    , conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("paramIdShowtime", idShowtime);

                 DataTable tab = fillDataTable(cmd);
                 var row = tab.Rows[0];

                 BookingShowtime showTimeDetail = new BookingShowtime()
                                    {
                                        Id = Convert.ToInt32(row["id_show_time"]),
                                        MovieName = Convert.ToString(row["name_movie"]),
                                        MovieTheaterName = Convert.ToString(row["name_movie_theater"]),
                                        StartTime = Convert.ToDateTime(row["start_time"]),
                                        RoomName = Convert.ToString(row["name_room"]),
                                        Price = Convert.ToInt32(row["base_price"]),
                                        Type = Convert.ToString(row["type"])
                                    };

                transaction.Rollback();
                conn.Close();
                return showTimeDetail ;
            }
            catch(Exception ex) {
                ex.ToString();
                transaction.Rollback();
                conn.Close();
                return null;
            }
        }

        public List<BookingShowtime> getShowDaysByIdMovie(int? idMovie)
        {
            conn.Open();
            var transaction = conn.BeginTransaction();
            List<BookingShowtime> dateTimes = new List<BookingShowtime>();
            try
            {
                OracleCommand cmd = new OracleCommand(
                    "SELECT distinct Trunc(start_time) as start_time " +
                    "FROM show_time " +
                    "WHERE id_movie = :idMovieParam AND start_time > sysdate ORDER BY 1"
                    , conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("idMovieParam", idMovie);

                var tab = fillDataTable(cmd);              

                dateTimes = (from DataRow row in tab.Rows
                          select new BookingShowtime()
                          {
                              Id = tab.Rows.IndexOf(row),
                              StartTime = Convert.ToDateTime(row["start_time"]),
                          }
                          ).ToList();

                transaction.Rollback();
                conn.Close();

                return dateTimes;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                conn.Close();
                Console.WriteLine(e.ToString());
                return dateTimes;
            }
        }

        public List<BookingShowtime> getShowingDayByProcedure(int? idMovie)
        {
            conn.Open();
            var transaction = conn.BeginTransaction();

            var cmd = new OracleCommand("SPGetShowingMovieStarttime(:param_movie_id)");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("param_movie_id", idMovie);
            
            DataTable tab = fillDataTable(cmd);

            return  new List<BookingShowtime>();
        }

        public List<BookingShowtime> getCitiesByIdMovie(int? idMovie)
        {
            List<BookingShowtime> cities = new List<BookingShowtime>();

            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                    "SELECT distinct city " +
                    "FROM show_time A JOIN room B ON A.ID_ROOM = B.id_room " +
                    "JOIN movie_theater C ON B.id_movie_theater = C.id_movie_theater " +
                    "WHERE id_movie = :idMovieParam ORDER BY 1"
                    , conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("idMovieParam", idMovie);

                var tab = fillDataTable(cmd);

                cities = (from DataRow row in tab.Rows
                          select new BookingShowtime()
                          {
                              Id = tab.Rows.IndexOf(row),
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
                    "SELECT distinct type FROM show_time WHERE id_movie = :idMovieParam AND start_time > sysdate ORDER BY 1"
                    , conn);

                cmd.BindByName = true;
                cmd.Parameters.Add("idMovieParam", idMovie);

                var tab = fillDataTable(cmd);

                types = (from DataRow row in tab.Rows
                          select new BookingShowtime()
                          {
                              Id = tab.Rows.IndexOf(row),
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

        public List<BookingShowtime> getBookingShowtime(int? idMovie, string cityName, DateTime showDayInput , string type )
        {
            List<BookingShowtime> showtimes = new List<BookingShowtime>();
            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                    "SELECT  id_show_time, name_movie_theater, " +
                    "to_char(start_time, 'HH24:MI:SS') as start_time " +
                    "FROM show_time A JOIN room B ON A.ID_ROOM = B.id_room " +
                    "JOIN movie_theater C ON B.id_movie_theater = C.id_movie_theater " +
                    "WHERE TRUNC(start_time) = TO_DATE(:paramShowDay, 'MM/dd/yyyy') " +
                    "AND id_movie = :paramIdMovie AND city = :paramCityName AND type = :paramType"
                    , conn);

                cmd.BindByName = true;
                string showDays = showDayInput.ToShortDateString();
                char[] convertedShowDay = showDays.ToCharArray();

                cmd.Parameters.Add("paramShowDay", convertedShowDay);
                cmd.Parameters.Add("paramIdMovie", idMovie);
                cmd.Parameters.Add("paramCityName", cityName);
                cmd.Parameters.Add("paramType", type);

                var tab = fillDataTable(cmd);

                showtimes = (from DataRow row in tab.Rows
                             select new BookingShowtime()
                             {
                                 Id = Convert.ToInt32(row["id_show_time"]),
                                 MovieTheaterName = Convert.ToString(row["name_movie_theater"]),
                                 StartTime = Convert.ToDateTime(row["start_time"]),
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
                                 MovieTheaterName = Convert.ToString(row["name_movie_theater"]),
                                 City = Convert.ToString(row["city"]),
                                 StartTime = Convert.ToDateTime(row["start_time"]),
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