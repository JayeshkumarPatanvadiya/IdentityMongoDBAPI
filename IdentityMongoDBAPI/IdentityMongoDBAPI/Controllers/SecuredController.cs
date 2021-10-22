using IdentityMongoDBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMongoDBAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class SecuredController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("message")]
        public IActionResult MyAction()
        {
            return Ok("000875");
        }
    }
}