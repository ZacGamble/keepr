using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using keepr.Models;
using keepr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaultsController : ControllerBase
    {
        private readonly VaultsService _vs;

        public VaultsController(VaultsService vs)
        {
            _vs = vs;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Vault>> Create([FromBody] Vault vaultData)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                vaultData.CreatorId = userInfo.Id;
                Vault vault = _vs.Create(vaultData);
                vault.Creator = userInfo;
                return Ok(vault);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Vault> Get(int id)
        {
            try
            {
                Vault vault = _vs.Get(id);
                return Ok(vault);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/keeps")]
        [Authorize]
        public ActionResult<List<Keep>> GetVaultKeepsById(int id)
        {
            try
            {
                List<Keep> vaultKeeps = _vs.GetVaultKeepsById(id);
                return Ok(vaultKeeps);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<Vault> Edit([FromBody] Vault vaultData, int id)
        {
            try
            {
                vaultData.Id = id;
                Vault vault = _vs.Edit(vaultData);
                return Ok(vault);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<Vault> Delete(int id)
        {
            try
            {
                _vs.Delete(id);
                return Ok("Vault deleted");
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}