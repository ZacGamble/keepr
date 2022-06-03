using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
    public class VaultsRepository
    {
        private readonly IDbConnection _db;

        public VaultsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal Vault Create(Vault vaultData)
        {
            string sql = @"
            INSERT INTO vaults
            (name, description, isPrivate, creatorId)
            VALUES
            (@Name, @Description, @isPrivate, @CreatorId);
            SELECT LAST_INSERT_ID();";
            vaultData.Id = _db.ExecuteScalar<int>(sql, vaultData);
            return vaultData;
        }

        internal Vault Get(int id)
        {
            string sql = @"
            SELECT 
                v.*
            FROM vaults v
            WHERE v.id = @id";
            return _db.QueryFirstOrDefault<Vault>(sql, new { id });
        }

        internal void Edit(Vault original)
        {
            string sql = @"
            UPDATE vaults
            SET
            name = @Name,
            description = @Description,
            isPrivate = @IsPrivate
            WHERE id = @Id;";
            _db.Execute(sql, original);
        }

        internal void Delete(int id)
        {
            string sql = @"DELETE FROM vaults WHERE id = @id LIMIT 1;";
            _db.Execute(sql, new { id });
        }

        internal List<Keep> GetVaultKeepsById(int id)
        {
            string sql = @"
            SELECT
            vk.*
            FROM keeps vk
            WHERE vk.id = @id
            ";
            return _db.Query<Keep>(sql, new { id }).ToList();
        }
    }
}