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
            VaultKeep newVaultKeep = _repo.Create(vaultKeepData);

            if (newVaultKeep.CreatorId != userId)
            {
                throw new System.Exception("You do not have the right to post this.");
            }
            else
            {

                return newVaultKeep;
            }
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