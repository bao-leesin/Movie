using Movie.DAO;
using Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Areas.Admin.Data
{
    public class ShowtimeDao : Dao
    {
        public ShowtimeDao()
        {
            
        }
        public bool Create()
        {
            return true;
        }
    }
}