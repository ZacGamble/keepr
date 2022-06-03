using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
    public class KeepsRepository
    {
        private readonly IDbConnection _db;

        public KeepsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal List<Keep> GetByCreatorId(string id)
        {
            string sql = @"
            SELECT
            k.*,
            a.*
            FROM keeps k
            JOIN accounts a ON k.creatorId = a.id
            WHERE creatorId = @id";
            return _db.Query<Keep, Profile, Keep>(sql, (k, p) =>
            {
                k.Creator = p;
                return k;
            }, new { id }).ToList();
        }

        internal Keep Create(Keep keepData)
        {
            string sql = @"
            INSERT INTO keeps
            (name, description, img, views, kept, creatorId)
            VALUES
            (@Name, @Description, @Img, @Views, @Kept, @CreatorId);
            SELECT LAST_INSERT_ID();
            ";
            keepData.Id = _db.ExecuteScalar<int>(sql, keepData);
            return keepData;
        }
        internal List<Keep> Get()
        {
            string sql = @"
            SELECT
            a.*,
            k.*
            FROM keeps k
            JOIN accounts a ON k.creatorId = a.id
            ";
            return _db.Query<Account, Keep, Keep>(sql, (a, k) =>
            {
                k.Creator = a;
                return k;
            }).ToList();
        }
        internal Keep Get(int id)
        {
            string sql = @"
            SELECT
                k.*
            FROM keeps k
            WHERE k.id = @id;";
            return _db.QueryFirstOrDefault<Keep>(sql, new { id });
        }

        internal void Edit(Keep original)
        {
            string sql = @"
            UPDATE keeps
            SET
            name = @Name,
            description = @Description,
            img = @Img,
            views = @Views,
            kept = @Kept
            WHERE id = @Id;
            ";
            _db.Execute(sql, original);
        }

        internal void Delete(int id)
        {
            string sql = @"DELETE FROM keeps WHERE id = @id LIMIT 1;";
            _db.Execute(sql, new { id });
        }
    }
}