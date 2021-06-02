using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using TinyConfig.Abstractions;
using TinyConfig.SqlServer.Cryptography;

namespace TinyConfig.SqlServer.Core
{
    /// <summary>
    /// simple db config store
    /// </summary>
    public class SqlServerConfigStore : ITinyConfigStore
    {
        private readonly SqlServerConfigOptions _options;
        private readonly AesCrypto _aes;

        public SqlServerConfigStore(SqlServerConfigOptions options)
        {
            _options = options;
            _aes = new AesCrypto(options.EncryptionKey);
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<ITinySetting>, object)> GetAllWithVersionToken()
        {
            using var conn = CreateConnection();
            var result = await conn.QueryMultipleAsync($"{SelectAllQuery}; {LastVersionQuery}");
            var settings = await result.ReadAsync<SimpleSetting>();
            var lastVersion = await result.ReadAsync<DateTime>();

            return (settings.Select(entry => {
                if (entry.IsSecret) entry.Value = _aes.Decrypt(entry.Value);
                return entry;
            }), lastVersion);
        }

        /// <inheritdoc />
        public async Task<bool> HasChanged(object versionToken = null)
        {
            using var conn = CreateConnection();
            var mostRecentLastModified = await conn.ExecuteScalarAsync<DateTime?>(LastVersionQuery);
            if (mostRecentLastModified.HasValue && versionToken != null)
                return mostRecentLastModified.Value.CompareTo(versionToken) != 0;
            return mostRecentLastModified.HasValue;
        }

        /// <summary>
        /// create new <see cref="SqlConnection"/>
        /// </summary>
        /// <returns><see cref="SqlConnection"/></returns>
        private SqlConnection CreateConnection() => new SqlConnection(_options.ConnectionString);

        private string SelectAllQuery => $"SELECT * FROM {_options.TableName}";

        private string LastVersionQuery => $"SELECT TOP 1 LastModifiedOn FROM {_options.TableName} ORDER BY LastModifiedOn DESC";
    }
}
