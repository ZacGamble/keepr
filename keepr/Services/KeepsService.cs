using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
    public class KeepsService
    {
        private readonly KeepsRepository _repo;

        public KeepsService(KeepsRepository repo)
        {
            _repo = repo;
        }

        internal List<Keep> GetByCreatorId(string profileId)
        {
            return _repo.GetByCreatorId(profileId);
        }

        internal Keep Create(Keep keepData)
        {
            return _repo.Create(keepData);
        }

        internal List<Keep> Get()
        {
            return _repo.Get();
        }

        internal Keep Get(int id)
        {
            Keep found = _repo.Get(id);
            if (found == null)
            {
                throw new Exception("Invalid ID");
            }
            return found;
        }

        internal Keep Edit(Keep keepData, string userId)
        {
            Keep original = Get(keepData.Id);
            if (original.CreatorId != userId)
            {
                original.Views = keepData.Views > 0 ? keepData.Views : original.Views;
                original.Kept = keepData.Kept > 0 ? keepData.Kept : original.Kept;
                _repo.Edit(original);
                return original;
            }
            else if (original.CreatorId == userId)
            {
                original.Name = keepData.Name ?? original.Name;
                original.Description = keepData.Description ?? original.Description;
                original.Img = keepData.Img ?? original.Img;
                original.Views = keepData.Views > 0 ? keepData.Views : original.Views;
                original.Kept = keepData.Kept > 0 ? keepData.Kept : original.Kept;
                _repo.Edit(original);
                return original;
            }
            else { throw new Exception("something went wrong"); }
        }

        internal void Delete(int id, string userId)
        {
            Keep original = Get(id);
            if (original.CreatorId != userId)
            {
                throw new Exception("You do not own this keep");
            }
            _repo.Delete(id);
        }
    }
}