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
        private readonly IDifferentialJsonService _differentialJsonService;
        public DifferentialController(IDifferentialJsonService differentialJsonService)
        {
            _differentialJsonService = differentialJsonService;
        }

        [HttpPut]
        [Route("{id}/left")]
        public ActionResult<JsonResponse> CreateLeft(string id, [FromBody] JsonInput input)
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
        public ActionResult CreateRight(string id, [FromBody] JsonInput input)
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
        public ActionResult<JsonResponse> GetDifferential(string id)
        {
            var result = _differentialJsonService.GetJsonDiff(id);

            return new ObjectResult(result);
        }

    }
}
