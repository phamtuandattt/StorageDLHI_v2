using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.QueryStatements
{
    public class QueryStatement
    {
        public const string GET_SUPPLIERS = "SELECT *FROM SUPPLIERS";
        public const string INSERT_SUPPLIERS = "INSERT INTO SUPPLIERS VALUES ('{0}', N'{1}', '{2}', N'{3}', '{4}', N'{5}', N'{6}', '{7}')";
    }
}
