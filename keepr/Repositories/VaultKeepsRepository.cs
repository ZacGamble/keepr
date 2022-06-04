using System.Data;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
    public class VaultKeepsRepository
    {
        private readonly IDbConnection _db;

        public VaultKeepsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal VaultKeep Create(VaultKeep vaultKeepData)
        {
            string sql = @"
           INSERT INTO vaultkeeps
           (creatorId, vaultId, keepId)
           VALUES
           (@CreatorId, @VaultId, @KeepId);
           SELECT LAST_INSERT_ID();
           ";
            vaultKeepData.Id = _db.ExecuteScalar<int>(sql, vaultKeepData);
            return vaultKeepData;
        }

        internal void Delete(int id)
        {
            string sql = @"DELETE FROM vaultkeeps WHERE id = @id LIMIT 1;";
            _db.Execute(sql, new { id });
        }

        internal VaultKeep Get(int id)
        {
            string sql = @"
            SELECT
            vk.*
            FROM vaultkeeps vk
            WHERE vk.id = @id;";
            return _db.QueryFirstOrDefault<VaultKeep>(sql, new { id });
        }

        internal VaultKeep GetByVaultId(int id)
        {
            string sql = @"
            SELECT
            vk.*
            FROM vaultkeeps vk
            WHERE vk.vaultId = @id;";
            return _db.QueryFirstOrDefault<VaultKeep>(sql, new { id });
        }
    }
}