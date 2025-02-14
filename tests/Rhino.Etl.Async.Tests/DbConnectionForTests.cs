using System.Configuration;

namespace Rhino.Etl.Async.Tests
{
    internal class DbConnectionForTests
    {
        public static ConnectionStringSettings GetForTestDb()
        {
            var settings = new ConnectionStringSettings("test",
                "Data Source=.\\SQL2022; Initial Catalog=RhinoEtlTests; user=sa; Password=sm4rtm0b1l3Sql",
                "System.Data.SqlClient.SqlConnection,System.Data,Version=2.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089");
            return settings;
        }
    }
}
