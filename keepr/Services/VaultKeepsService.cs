using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
    public class VaultKeepsService
    {
        private readonly VaultKeepsRepository _repo;
        private readonly VaultsService _vs;

        public VaultKeepsService(VaultKeepsRepository repo, VaultsService vs)
        {
            _repo = repo;
            _vs = vs;
        }

        internal VaultKeep Create(VaultKeep vaultKeepData, string userId)
        {
            if (userId == null)
            {
                throw new System.Exception("You must be logged in to post.");
            }
            Vault vault = _vs.Get(vaultKeepData.VaultId, userId);
            if (vault.CreatorId != userId)
            {
                throw new System.Exception("Forbidden post action!");
            }
            // FIXME figure out how to verify ownership here
            VaultKeep newVaultKeep = _repo.Create(vaultKeepData);

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