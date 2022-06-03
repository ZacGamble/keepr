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

        internal VaultKeep Create(VaultKeep vaultKeepData)
        {
            return _repo.Create(vaultKeepData);
        }
        internal VaultKeep Get(int id)
        {
            return _repo.Get(id);
        }

        internal void Delete(int id)
        {
            Get(id);
            _repo.Delete(id);
        }
    }
}