using System.Collections.Generic;
using System.Threading.Tasks;
using AltInnSrr;
using Microsoft.AspNetCore.Mvc;

namespace MoveAdmin.Web.Controllers
{
    [Route("api/[controller]")]
    public class OrganisationsController : Controller
    {
        // GET api/organisations
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/organisations/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var org = new Organisation(this.HttpContext.RequestServices.GetService(typeof(ISrrClient)) as ISrrClient);
            await org.GetInforation(id);
            return org.ToJson();
        }

        // POST api/organisations
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/organisations/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/organisations/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
