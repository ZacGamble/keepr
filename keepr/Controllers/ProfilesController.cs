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

        public ProfilesController(AccountService acctServ, KeepsService ks)
        {
            _acctServ = acctServ;
            _ks = ks;
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
    }

}