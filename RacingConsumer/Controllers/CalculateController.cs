using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abstractions;

namespace RacingConsumer.Controllers
{
    [Route("Calculate")]
    [ApiController]
    public class CalculateController : ControllerBase
    {

        private readonly IDispatchService _dispatchService;
        public CalculateController(IDispatchService dispatchService)
        {
            _dispatchService = dispatchService;
        }
        // GET api/values
        [HttpGet]
        public async Task<IDispatchResponse> Get()
        {
            return await _dispatchService.DispatchCommand(new CalculateSumCommand() { A = 10, B = 2 });
        }
    }
}
