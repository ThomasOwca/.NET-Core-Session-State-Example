using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SessionStateExample.SessionExtensions;

namespace SessionStateExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        public string SomeMethod()
        {
            string SessionKeyName = "_Name";
            string SessionKeyAge = "_Age";
            string SessionKeyGroup = "_Group";
            string name = "";

            // Check if user has already passed validation once before, before executing theoretical validation logic again.
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                if (HttpContext.Request.Headers["Name"] == HttpContext.Session.GetString(SessionKeyName) && HttpContext.Request.Headers["Group"] == HttpContext.Session.GetString(SessionKeyGroup))
                {
                    // Short circuit, and by-pass the need to re-validate the user or whatever...
                    return "Bypass Valid logic.";
                }
            }

            // Passing theoretical validation.... continue below.
            // Validation would go here...

            // Validation is over.
            // Requires: using Microsoft.AspNetCore.Http;
            // First time through when Session is empty.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {

                // First time around, set the user key if there is a header for "Name".
                if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Name"]))
                {
                    HttpContext.Session.SetString(SessionKeyName, HttpContext.Request.Headers["Name"]);
                    HttpContext.Session.SetInt32(SessionKeyAge, 773);
                }

                if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Group"]))
                {
                    HttpContext.Session.SetString(SessionKeyGroup, HttpContext.Request.Headers["Group"]);
                }
            }

            return "Valid";
        }

        [HttpGet]
        public IActionResult Get()
        {
            var status = SomeMethod();

            return Ok(status);
        }
    }
}
