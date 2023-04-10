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
    public class MovieDao : Dao
    {

        public MovieDao()
        {
        }
        public List<Film> getAllMovie()
        {
            List<Film> movies = new List<Film>();
            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                  "SELECT * FROM movie WHERE id_movie IN " +
                  "(SELECT ID_MOVIE FROM show_time  WHERE START_TIME > SYSDATE)"
                   , conn);
     
                cmd.BindByName = true;
                string thisTime = DateTime.Now.ToString("MM/dd/yyyy H:mm");
              

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable tab = new DataTable();
                da.Fill(tab);

                movies = (from DataRow row in tab.Rows
                          select new Film()
                          {
                              Id = Convert.ToInt32(row["id_movie"]),
                              FilmName = Convert.ToString(row["name_movie"]),
                              Duration = Convert.ToInt32(row["duration"]),
                              ImageUrl = Convert.ToString(row["url_image"])
                          }
                            ).ToList();

                transaction.Rollback();
                conn.Close();

                return movies;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return movies;
            }
        }
    }
}