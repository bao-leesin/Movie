using Movie.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Movie.DAO
{
    public class MovieDao
    {
        public List<Film> getAllMovie()
        {
            List<Film> movies = new List<Film>();
            try
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                OracleCommand cmd = new OracleCommand(
                    "SELECT * FROM movie WHERE id_movie IN (SELECT id_movie FROM show_time WHERE start_time > :timeParam))"
                    , conn);

                cmd.BindByName = true;
              
                cmd.Parameters.Add("timeParam", DateTime.Now);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable tab = new DataTable();
                da.Fill(tab);

                movies = (from DataRow row in tab.Rows
                             select new Film()
                             {
                                
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