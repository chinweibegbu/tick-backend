using Tick.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Tick.Controllers
{
    public class MetaController : ControllerBase
    {
        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }

        [HttpGet("/health")]
        [BasicAuthorization]
        public ActionResult<string> Health()
        {
            return Ok("Ok");
        }
    }
}
