using Movie.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Areas.Admin.Data
{
    public class MovieDao : Dao, ICreate
    {
        public bool Create()
        {
            conn.Open();
            var transaction = conn.BeginTransaction();  
            return true;
        }
    }
}