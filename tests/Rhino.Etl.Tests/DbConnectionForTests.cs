using System.Configuration;

namespace Rhino.Etl.Tests
{
    internal class DbConnectionForTests
    {
        public static ConnectionStringSettings GetForTestDb()
        {
            var settings = new ConnectionStringSettings("test",
                "Data Source=.; Initial Catalog=RhinoEtlTests; user=sa; Password=myPwd",
                "System.Data.SqlClient.SqlConnection,System.Data,Version=2.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089");
            return settings;
        }
    }
}
