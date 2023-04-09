using Movie.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Areas.Admin.Data
{
    public class MovieDao : Dao, ICreate
    {
        public int Create(int a)
        {
            throw new NotImplementedException();
        }

        public int Create()
        {
            throw new NotImplementedException();
        }

        public override int Delete()
        {
            return 6;
        }

       
    }
}