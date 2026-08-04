using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
    public class AccountService
    {
        private readonly AccountsRepository _repo;
        private readonly KeepsRepository _keepsRepo;

        public AccountService(AccountsRepository repo, KeepsRepository keepsRepo)
        {
            _repo = repo;
            _keepsRepo = keepsRepo;
        }

        internal string GetProfileEmailById(string id)
        {
            return _repo.GetById(id).Email;
        }
        internal Account GetProfileByEmail(string email)
        {
            return _repo.GetByEmail(email);
        }
        internal Account GetOrCreateProfile(Account userInfo)
        {
            Account profile = _repo.GetById(userInfo.Id);
            if (profile == null)
            {
                Account created = _repo.Create(userInfo);
                SeedStarterKeeps(created.Id);
                return created;
            }

            // If existing profile currently has 0 keeps, seed starter keeps for them too!
            var userKeeps = _keepsRepo.GetByCreatorId(profile.Id);
            if (userKeeps == null || userKeeps.Count == 0)
            {
                SeedStarterKeeps(profile.Id);
            }

            return profile;
        }

        private void SeedStarterKeeps(string creatorId)
        {
            var starterKeeps = new List<Keep>
            {
                new Keep
                {
                    CreatorId = creatorId,
                    Name = "Mountain Solitude",
                    Description = "Serene alpine lakes nestled amidst dramatic, snow-dusted peaks in the North Cascades.",
                    Img = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?auto=format&fit=crop&w=1000&q=80",
                    Views = 12,
                    Kept = 4
                },
                new Keep
                {
                    CreatorId = creatorId,
                    Name = "Neon Cyberpunk Alley",
                    Description = "Futuristic streetscape bathed in vibrant neon magenta and cyan reflections after midnight rain.",
                    Img = "https://images.unsplash.com/photo-1519501025264-65ba15a82390?auto=format&fit=crop&w=1000&q=80",
                    Views = 28,
                    Kept = 9
                },
                new Keep
                {
                    CreatorId = creatorId,
                    Name = "Minimalist Architecture",
                    Description = "Clean brutalist lines, soft shadows, and warm sunlight breaking through architectural glass.",
                    Img = "https://images.unsplash.com/photo-1513694203232-719a280e022f?auto=format&fit=crop&w=1000&q=80",
                    Views = 15,
                    Kept = 3
                },
                new Keep
                {
                    CreatorId = creatorId,
                    Name = "Cozy Espresso Corner",
                    Description = "Artisanal latte art paired with vintage leather journals in a quiet morning coffee shop.",
                    Img = "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?auto=format&fit=crop&w=1000&q=80",
                    Views = 34,
                    Kept = 11
                },
                new Keep
                {
                    CreatorId = creatorId,
                    Name = "Cosmic Nebula Mirage",
                    Description = "Deep space photography capturing vibrant stellar nurseries and swirling cosmic dust.",
                    Img = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&w=1000&q=80",
                    Views = 45,
                    Kept = 18
                },
                new Keep
                {
                    CreatorId = creatorId,
                    Name = "Emerald Forest Canopy",
                    Description = "Sunbeams filtering through lush redwood trees on a misty morning trail.",
                    Img = "https://images.unsplash.com/photo-1448375240586-882707db888b?auto=format&fit=crop&w=1000&q=80",
                    Views = 22,
                    Kept = 7
                }
            };

            foreach (var keep in starterKeeps)
            {
                try
                {
                    _keepsRepo.Create(keep);
                }
                catch
                {
                    // Ignore duplicate seeding if any constraint occurs
                }
            }
        }

        internal Profile GetProfileById(string id)
        {
            Profile found = _repo.GetProfileById(id);
            if (found == null)
            {
                throw new Exception("invalid id");
            }
            return found;
        }

        internal Account Edit(Account editData, string userEmail)
        {
            Account original = GetProfileByEmail(userEmail);
            original.Name = editData.Name.Length > 0 ? editData.Name : original.Name;
            original.Picture = editData.Picture.Length > 0 ? editData.Picture : original.Picture;
            return _repo.Edit(original);
        }

        internal Account GetAccountById(string profileId)
        {
            Account profile = _repo.GetById(profileId);

            return profile;
        }
    }
}