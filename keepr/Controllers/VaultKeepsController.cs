using System.Threading.Tasks;
using keepr.Models;
using keepr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeWorks.Auth0Provider;
using System;

namespace keepr.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaultKeepsController : ControllerBase
    {
        private readonly VaultKeepsService _vks;

        public VaultKeepsController(VaultKeepsService vks)
        {
            _vks = vks;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<VaultKeep>> Create([FromBody] VaultKeep vaultKeepData, string userId)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                vaultKeepData.CreatorId = userInfo.Id;
                VaultKeep newVaultKeep = _vks.Create(vaultKeepData, userInfo?.Id);
                return Ok(newVaultKeep);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<VaultKeep>> Delete(int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                _vks.Delete(id, userInfo.Id);
                return Ok("Vaulted Keep Deleted");
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}