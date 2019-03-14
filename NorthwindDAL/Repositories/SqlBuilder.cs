﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDAL.Repositories
{
    public static class SqlBuilder
    {
        public static DbParameter Create(string paramName, DbType paramType, object value)
        {
            SqlParameter param = new SqlParameter(paramName, paramType);
            if (value == null)
                param.Value = DBNull.Value;
            else
                param.Value = value;

            return param;
        }
    }
}
