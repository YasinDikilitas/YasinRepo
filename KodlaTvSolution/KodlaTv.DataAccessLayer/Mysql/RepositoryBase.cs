using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.DataAccessLayer.Mysql
{
    public class RepositoryBase
    {
        protected static object db;
        private static object _obj = new object();
        protected RepositoryBase()
        {
            CreateContext();
        }
        private static void CreateContext()
        {
            if (db == null)
            {
                lock (_obj)
                {
                    if (db == null)
                    {
                        db = new object();
                    }

                }

            }

        }
    }
}
