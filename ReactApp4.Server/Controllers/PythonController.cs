using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Services;
using System;

namespace ReactApp4.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PythonController : ControllerBase
    {
        private readonly PythonCaller _pythonCaller;

        public PythonController(PythonCaller pythonCaller)
        {
            _pythonCaller = pythonCaller;
        }

        [HttpGet]
        public IActionResult CallPythonFunction()
        {
            try
            {
                var result = _pythonCaller.CallSayHello();
                return Ok($"Result of calling Python function: {result}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
