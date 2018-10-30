﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DimensionAttributesController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new[] { "Date", "FullName", "Department", "ActivityType", "Title" });
        }

        //[HttpGet]
        //public IActionResult GetAllForFactTable(string factTable)
        //{
        //    return Ok(new[] { "Date", "FullName", "Department","ActivityType", "Title"});
        //}
    }
}