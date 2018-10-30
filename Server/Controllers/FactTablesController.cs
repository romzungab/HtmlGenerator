using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactTablesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(FacttableDimensionAttributes.GetFactTables().Select(ft => new
            {
                ft.Name,
                Dimensions = ft.Dimensions?.Select(d => new
                {
                    d.Name,
                    Attributes = d.Attributes?.Select(da => da.Name),
                })
            }));
        }
    }
}
