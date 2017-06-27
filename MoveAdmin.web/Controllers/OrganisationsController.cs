using System;
using System.Threading.Tasks;
using AltInnSrr.Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using NoCommons.Org;

namespace AltInnSrr.Api.Controllers
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
                return Forbid($"{id} er ikke et gyldig organisasjonsnummer");
            }

            try
            {

                var organisation = new Organisation(id, GetService<ISrrClient>(),
                    GetService<IEnhetsregisteretClient>());
                await organisation.GetInforation();
                return Ok(organisation.ToJson());
            }
            catch (Exception e)
            {
                return HandleErrors(e);
            }
        }

        private  T GetService<T>()  
        {
            return (T) HttpContext.RequestServices.GetService(typeof(T));
        }

        // POST api/organisations
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]int id)
        {
            if (!OrganisasjonsnummerValidator.IsValid(id.ToString()))
            {
                return Forbid($"{id} er ikke et gyldig organisasjonsnummer");
            }

            var organisation = new Organisation(id, GetService<ISrrClient>(), GetService<IEnhetsregisteretClient>());
            try
            {
                await organisation.Add();
                var uri = this.HttpContext.Request.Path + "/" + organisation.OrganisationNumber;
                return Created(uri, organisation.ToJson());
            }
            catch (Exception e)
            {
                return HandleErrors(e);
            }
        }

        // PUT api/organisations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]AltInnSrrRights value)
        {
            var organisation = new Organisation(id, GetService<ISrrClient>(), GetService<IEnhetsregisteretClient>());
            try
            {
                await organisation.Update(value);
                return Ok(organisation.ToJson());
            }
            catch (Exception e)
            {
                return HandleErrors(e);
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

            var organisation = new Organisation(id, GetService<ISrrClient>(), GetService<IEnhetsregisteretClient>());

            try
            {
                await organisation.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return HandleErrors(e);
            }
        }

        private ObjectResult HandleErrors(Exception e)
        {

            if(e is EnhetNotFoundException)
            {
                return NotFound(e.Message);
            }
            if (e is AltInnSrrException)
            {
                return StatusCode(500, e.Message);
            }
            return StatusCode(500, e.Message);
        }
    }
}

