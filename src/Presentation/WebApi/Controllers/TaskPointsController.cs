using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TaskPointsController : ControllerBase
{
    [HttpGet("{id:guid}", Name = "GetTaskPointById")]
    public ActionResult<IActionResult> GetTaskPointById(
        Guid id,
        CancellationToken token)
    {

    }

    [HttpGet]
    public ActionResult<IActionResult> GetAll(
        [FromQuery] FilterRequest filter,
        CancellationToken token)
    {

    }

    [HttpPost]
    public ActionResult<IActionResult> CreateTaskPoint(
        [FromBody] CreatingTaskPointRequest request,
        CancellationToken token)
    {

    }



    [HttpDelete("{id:guid}")]
    public ActionResult<IActionResult> MarkTaskPointAsDeleted(
        Guid id,
        CancellationToken token)
    {

    }

}