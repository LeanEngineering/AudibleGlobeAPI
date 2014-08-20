using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudibleGlobeApiWorkerRole.DAO
{
    public static class Db
    {
        private const string AzureSqlConnectionString = "Server=tcp:nhpry60tw7.database.windows.net,1433;Database=AudibleGlobeSQL;User ID=audibleglobesql@nhpry60tw7;Password=BernardoFanti1;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        public static IDbConnection AzureSql()
        {
            return new SqlConnection(AzureSqlConnectionString);
        }
    }
}
