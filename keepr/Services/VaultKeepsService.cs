using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
    public class VaultKeepsService
    {
        private readonly VaultKeepsRepository _repo;

        public VaultKeepsService(VaultKeepsRepository repo)
        {
            _repo = repo;
        }

        internal VaultKeep Create(VaultKeep vaultKeepData, string userId)
        {
            if (userId == null)
            {
                throw new System.Exception("You must be logged in to post.");
            }
            // FIXME figure out how to verify ownership here
            VaultKeep newVaultKeep = _repo.Create(vaultKeepData);
            if (userId != newVaultKeep.CreatorId)
            {
                throw new System.Exception("You must be logged in to post");
            }
            return newVaultKeep;

        }

        internal VaultKeep Get(int id)
        {
            return _repo.Get(id);
        }

        internal void Delete(int id, string userId)
        {
            VaultKeep found = Get(id);
            if (found.CreatorId != userId)
            {
                throw new System.Exception("You do not have the right to delete this.");
            }
            _repo.Delete(id);
        }

        internal VaultKeep GetByVaultId(int id)
        {
            return _repo.GetByVaultId(id);
        }
    }
}