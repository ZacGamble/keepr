using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
    public class VaultsService
    {
        private readonly VaultsRepository _repo;

        public VaultsService(VaultsRepository repo)
        {
            _repo = repo;
        }

        internal Vault Create(Vault vaultData)
        {
            return _repo.Create(vaultData);
        }

        internal Vault Get(int id)
        {
            Vault found = _repo.Get(id);
            if (found == null)
            {
                throw new Exception("Invalid ID");
            }
            if (found.IsPrivate)
            {
                throw new Exception("You do not have access");
            }

            return found;
        }

        internal Vault Edit(Vault vaultData)
        {
            Vault original = Get(vaultData.Id);
            original.Name = vaultData.Name ?? original.Name;
            original.Description = vaultData.Description ?? original.Description;
            original.IsPrivate = vaultData.IsPrivate != original.IsPrivate ? vaultData.IsPrivate : original.IsPrivate;

            _repo.Edit(original);
            return original;
        }

        internal void Delete(int id)
        {
            Get(id);
            _repo.Delete(id);
        }

        internal List<VaultKeepViewModel> GetVaultKeepsById(int id, string userId)

        {
            Vault found = Get(id);
            if (found.IsPrivate && found.CreatorId != userId)
            {
                throw new Exception("You do not have access");
            }
            return _repo.GetVaultKeepViewModelFromVault(id);
        }

        internal List<Vault> GetMyVaults(string id)
        {
            return _repo.GetMyVaults(id);
        }

        internal List<Vault> GetVaultsByCreatorId(string profileId)
        {
            return _repo.GetVaultsByCreatorId(profileId);
        }
    }
}