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

    public class KeepsController : ControllerBase
    {
        private readonly KeepsService _ks;

        public KeepsController(KeepsService ks)
        {
            _ks = ks;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Keep>> Create([FromBody] Keep keepData)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                keepData.CreatorId = userInfo.Id;
                Keep keep = _ks.Create(keepData);
                keep.Creator = userInfo;
                return Ok(keep);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        [HttpGet]
        public ActionResult<List<Keep>> Get()
        {
            try
            {
                List<Keep> keep = _ks.Get();
                return Ok(keep);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<Keep> Get(int id)
        {
            try
            {
                Keep keep = _ks.Get(id);
                return Ok(keep);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Keep>> Edit([FromBody] Keep keepData, int id)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                keepData.Id = id;
                Keep keep = _ks.Edit(keepData, userInfo.Id);
                return Ok(keep);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Keep>> Delete(int id)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                _ks.Delete(id, userInfo.Id);
                return Ok("Keep deleted");
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}