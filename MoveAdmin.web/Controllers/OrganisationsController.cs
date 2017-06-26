using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AltInnSrr;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Newtonsoft.Json;
using NoCommons.Org;

namespace MoveAdmin.Web.Controllers
{
    [Route("api/[controller]")]
    public class OrganisationsController : Controller
    {
        // GET api/organisations
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var result =
                Organisation.GetOrganisations(
                    this.HttpContext.RequestServices.GetService(typeof(ISrrClient)) as ISrrClient);
            return Ok(JsonConvert.SerializeObject(result.Result));
        }

        // GET api/organisations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!OrganisasjonsnummerValidator.IsValid(id.ToString()))
            {
                return Forbid();
            }

            var organisation = new Organisation(this.HttpContext.RequestServices.GetService(typeof(ISrrClient)) as ISrrClient, id);
            await organisation.GetInforation();
            return Ok(organisation.ToJson());
        }

        // POST api/organisations
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]int value)
        {
            if (!OrganisasjonsnummerValidator.IsValid(value.ToString()))
            {
                return Forbid();
            }

            var organisation = new Organisation(this.HttpContext.RequestServices.GetService(typeof(ISrrClient)) as ISrrClient, value);
            try
            {
                await organisation.Add();
                var uri = this.HttpContext.Request.Path + "/" + organisation.OrganisationNumber;
                return Created(uri, organisation.ToJson());
            }
            catch (AltInnSrrException e)
            {
                return StatusCode(500, e);
            }
        }

        // PUT api/organisations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]AltInnSrrRights value)
        {
            var organisation = new Organisation(this.HttpContext.RequestServices.GetService(typeof(ISrrClient)) as ISrrClient, id);
            try
            {
                await organisation.Update(value);
                return Ok(organisation.ToJson());
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // DELETE api/organisations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            if (!OrganisasjonsnummerValidator.IsValid(id.ToString()))
            {
                return Forbid();
            }

            var organisation = new Organisation(this.HttpContext.RequestServices.GetService(typeof(ISrrClient)) as ISrrClient, id);

            try
            {
                await organisation.Delete(id);
                return Ok();
            }
            catch (AltInnSrrException e)
            {
                return StatusCode(500, e);
            }
        }        
    }
}

