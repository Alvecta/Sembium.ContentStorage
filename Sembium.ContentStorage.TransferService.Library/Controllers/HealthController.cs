using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sembium.ContentStorage.TransferService.Library.Controllers
{
    [Route("[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "OK";
        }
    }
}
