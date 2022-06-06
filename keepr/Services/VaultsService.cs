using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
    public class VaultsService
    {
        private readonly VaultsRepository _repo;
        private readonly AccountService _acctServ;

        public VaultsService(VaultsRepository repo, AccountService acctServ)
        {
            _repo = repo;
            _acctServ = acctServ;
        }

        internal Vault Create(Vault vaultData, string userId)
        {
            if (vaultData.CreatorId != userId)
            {
                throw new Exception("You must be logged in to create vaults.");
            }
            return _repo.Create(vaultData);
        }

        internal Vault Get(int id, string userId)
        {
            Vault found = _repo.Get(id);

            if (found == null)
            {
                throw new Exception("Invalid ID");
            }
            if (userId != found.CreatorId && found.IsPrivate)
            {
                throw new Exception("This is a private vault");
            }

            return found;
        }

        internal Vault Edit(Vault vaultData, string userId)
        {
            Vault original = Get(vaultData.Id, userId);
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
            Vault original = Get(id, userId);
            if (original.CreatorId != userId)
            {
                throw new Exception("O' you don't have the right!");
            }
            _repo.Delete(id);
        }

        internal List<VaultKeepViewModel> GetVaultKeepsById(int id, string userId)

        {
            Vault found = Get(id, userId);
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

        internal List<Vault> GetVaultsByCreatorId(string profileId, string userInfo)
        {

            List<Vault> foundVaults = _repo.GetVaultsByCreatorId(profileId);
            Account foundAccount = _acctServ.GetAccountById(profileId);
            if (foundAccount.Id != userInfo)
            {
                foundVaults = foundVaults.FindAll(v => v.IsPrivate == false);
                return foundVaults;
            }

            return foundVaults;
        }
    }
}