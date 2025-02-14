using System.Data;
using System.Data.Common;

namespace Rhino.Etl.Async.Infrastructure
{
    public static class DbCommandHelper
    {
        public static Task<int> ExecuteNonQueryAsync(this IDbCommand cmd)
        {
            var dbCmd = CastToDbCommand(cmd);
            return dbCmd.ExecuteNonQueryAsync();
        }

        public static Task<DbDataReader> ExecuteReaderAsync(this IDbCommand cmd)
        {
            var dbCmd = CastToDbCommand(cmd);
            return dbCmd.ExecuteReaderAsync();
        }

        private static DbCommand CastToDbCommand(IDbCommand source)
        {
            if (source == null) return null;
            var dest = source as DbCommand;
            if (dest == null) {
                throw new Exception($"Cannot cast '{source.GetType().Name}' to '{nameof(DbCommand)}'");
            }
            return dest;
        }
    }
}
