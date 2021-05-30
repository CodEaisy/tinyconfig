using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using TinyConfig.Abstractions;
using TinyConfig.SimpleDB.Cryptography;


namespace TinyConfig.SimpleDB.Core
{
    /// <summary>
    /// simple db config store
    /// </summary>
    public class SimpleDbConfigStore : ITinyConfigStore
    {
        private readonly SimpleDbConfigOptions _options;
        private readonly AesCrypto _aes;

        public SimpleDbConfigStore(SimpleDbConfigOptions options)
        {
            _options = options;
            _aes = new AesCrypto(options.EncryptionKey);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ITinySetting>> GetAll()
        {
            using var conn = CreateConnection();
            var settings = await conn.QueryAsync<SimpleSetting>($"SELECT * FROM {_options.TableName}");

            return settings.Select(entry => {
                if (entry.IsSecret) entry.Value = _aes.Decrypt(entry.Value);
                return entry;
            });
        }

        /// <inheritdoc />
        public async Task<bool> HasAny()
        {
            using var conn = CreateConnection();
            var count = await conn.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM {_options.TableName}");
            return count > 0;
        }

        /// <inheritdoc />
        public Task<bool> HasChanged(object versionToken)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// create new <see cref="SqlConnection"/>
        /// </summary>
        /// <returns><see cref="SqlConnection"/></returns>
        private SqlConnection CreateConnection() => new SqlConnection(_options.ConnectionString);
    }
}
