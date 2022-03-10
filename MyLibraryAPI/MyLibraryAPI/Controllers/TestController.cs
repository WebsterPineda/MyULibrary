using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Results;

namespace MyLibraryAPI.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            string tmp = "ABCDEFGC#DEFC#";
            return Ok(tmp.IndexOf("C#") + " - " + tmp.Substring(7+1,1));
        }
    }
}