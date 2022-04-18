using DescartesJsonDiff.Models;
using DescartesJsonDiff.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DescartesJsonDiff.Controllers
{
    [ApiController]
    [Route("/v1/diff/")]
    public class DifferentialController : ControllerBase
    {
        private readonly IDifferentialService _differentialJsonService;
        public DifferentialController(IDifferentialService differentialJsonService)
        {
            _differentialJsonService = differentialJsonService;
        }

        [HttpPut]
        [Route("{id}/left")]
        public ActionResult<ApiResponse> CreateLeft(string id, [FromBody] ApiInput input)
        {
            if (input.data == null)
            {
                return new BadRequestObjectResult(new { Value = "Data cannot be null" });
            }

            _differentialJsonService.CreateJsonData(id, input, "left");

            return new CreatedResult(id, input);
        }

        [HttpPut]
        [Route("{id}/right")]
        public ActionResult CreateRight(string id, [FromBody] ApiInput input)
        {
            if (input.data == null)
            {
                return new BadRequestObjectResult(new { Value = "Data cannot be null" });
            }

             _differentialJsonService.CreateJsonData(id, input, "right");

            return new CreatedResult(id, input);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<ApiResponse> GetDifferential(string id)
        {
            var result = _differentialJsonService.GetJsonDiff(id);

            return new ObjectResult(result) { StatusCode = result == null? 404 : 200};
        }

    }
}
