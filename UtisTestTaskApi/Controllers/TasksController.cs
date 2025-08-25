using Microsoft.AspNetCore.Mvc;
using UtisTestTask.Core.Base;
using UtisTestTask.Core.Base.Requests;
using UtisTestTask.Core.Requests.Tasks.CreateTask;
using UtisTestTask.Core.Requests.Tasks.DeleteTask;
using UtisTestTask.Core.Requests.Tasks.GetTasks;
using UtisTestTask.Core.Requests.Tasks.UpdateTask;
using EmptyResult = UtisTestTask.Core.Base.Requests.EmptyResult;

namespace UtisTestTask.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TasksController : BaseController
    {

        public TasksController(IRequestHandler requestHandler) : base(requestHandler)
        {
        }


        [HttpGet]
        public Task<ActionResult<GetTasksQueryResult>> GetTasks([FromQuery] GetTasksQueryData data, [FromServices] GetTasksQuery command)
            => RequestHandler.Handle(command, data);

        [HttpPost]
        public Task<ActionResult<Guid>> CreateTask([FromBody] CreateTaskCommandData data, [FromServices] CreateTaskCommand command)
            => RequestHandler.Handle(command, data);

        [HttpPut]
        public Task<ActionResult<Guid>> UpdateTask([FromBody] UpdateTaskCommandData data, [FromServices] UpdateTaskCommand command)
            => RequestHandler.Handle(command, data);

        [HttpDelete]
        public Task<ActionResult<EmptyResult>> DeleteTask([FromBody] DeleteTaskCommandData data, [FromServices] DeleteTaskCommand command)
            => RequestHandler.Handle(command, data);
    }
}
