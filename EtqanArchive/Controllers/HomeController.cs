using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtqanArchive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            //DateTime.Now.To
            return Ok("Api run successfully");
        }
    }
}
