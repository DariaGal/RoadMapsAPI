using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;

namespace team7_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "hello", "world" };
        }

        // GET api/values/5
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Tree> Get(int id)
        {
            var client = new MongoClient("mongodb+srv://admin:cdznjqbcnjxybr@clusterteam7-hb7ef.azure.mongodb.net/test?retryWrites=true");
            var database = client.GetDatabase("team7db");
            var trees = database.GetCollection<Tree>("trees");
            var tree = trees.Find(t => true).ToList();
            return Ok(tree);
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string>Post([FromBody] string value)
        {
            if (value == null)
            {
                return BadRequest("oops, try to add some string in body");
            }
            return Ok("you send string: " + value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
