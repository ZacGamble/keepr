using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Services;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    public class ProfilesController : ControllerBase
    {
        private readonly AccountService _acctServ;
        private readonly KeepsService _ks;
        private readonly VaultsService _vs;

        public ProfilesController(AccountService acctServ, KeepsService ks, VaultsService vs)
        {
            _acctServ = acctServ;
            _ks = ks;
            _vs = vs;
        }

        [HttpGet("{id}")]

        public ActionResult<Profile> GetProfileById(string id)
        {
            try
            {
                Profile profile = _acctServ.GetProfileById(id);
                return Ok(profile);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{profileId}/keeps")]
        public ActionResult<List<Keep>> GetKeepsByProfileId(string profileId)
        {
            try
            {
                _acctServ.GetProfileById(profileId);
                List<Keep> keeps = _ks.GetByCreatorId(profileId);
                return Ok(keeps);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet("{profileId}/vaults")]
        public ActionResult<List<Vault>> GetVaultsByProfileId(string profileId)
        {
            try
            {

                _acctServ.GetProfileById(profileId);
                List<Vault> vaults = _vs.GetVaultsByCreatorId(profileId);
                return Ok(vaults);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }

}