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

        internal Vault Create(Vault vaultData, string userId)
        {
            if (vaultData.CreatorId != userId)
            {
                throw new Exception("You must be logged in to create vaults.");
            }
            return _repo.Create(vaultData);
        }

        internal Vault Get(int id)
        {
            Vault found = _repo.Get(id);
            if (found == null)
            {
                throw new Exception("Invalid ID");
            }


            return found;
        }

        internal Vault Edit(Vault vaultData, string userId)
        {
            Vault original = Get(vaultData.Id);
            if (original.CreatorId != userId)
            {
                throw new Exception("O' you don't have the right!");
            }
            original.Name = vaultData.Name ?? original.Name;
            original.Description = vaultData.Description ?? original.Description;
            original.Img = vaultData.Img ?? original.Img;
            original.IsPrivate = vaultData.IsPrivate != original.IsPrivate ? vaultData.IsPrivate : original.IsPrivate;

            _repo.Edit(original);
            return original;
        }

        internal void Delete(int id, string userId)
        {
            Vault original = Get(id);
            if (original.CreatorId != userId)
            {
                throw new Exception("O' you don't have the right!");
            }
            _repo.Delete(id);
        }

        internal List<VaultKeepViewModel> GetVaultKeepsById(int id, string userId)

        {
            Vault found = Get(id);
            if (found.IsPrivate && found.CreatorId != userId)
            {
                throw new Exception("You do not have access");
            }
            List<VaultKeepViewModel> vaultKeeps = _repo.GetVaultKeepViewModelFromVault(id);
            return vaultKeeps;
        }

        internal List<Vault> GetMyVaults(string id)
        {
            return _repo.GetMyVaults(id);
        }

        internal List<Vault> GetVaultsByCreatorId(string profileId)
        {

            List<Vault> foundVaults = _repo.GetVaultsByCreatorId(profileId);

            // foundVaults = foundVaults.FindAll(v => v.IsPrivate == false);

            return foundVaults;
        }
    }
}