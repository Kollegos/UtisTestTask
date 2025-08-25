using Microsoft.EntityFrameworkCore;
using UtisTestTask.Core.Base.Requests;
using UtisTestTask.Core.Requests.Tasks.CreateTask;
using UtisTestTask.Core.Requests.Tasks.DeleteTask;
using UtisTestTask.Core.Tests.Infrastructure;
using UtisTestTask.DAL;
using UtisTestTask.Entities.Enums;
using UtisTestTask.Entities.Tasks;

namespace UtisTestTask.Core.Tests
{
    public class RequestTests
    {
        private RequestHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new RequestHandler();
            
        }

        [TestCase("Test1","",1)]
        public async Task CreateTask(string title, string description, int daysOffset)
        {
            await using var context = new UtisTestContext(Settings.TestDbConnectionString);
            var command = new CreateTaskCommand(new CustomConsoleLogger<CreateTaskCommand>(), context, new CreateTaskCommandValidator());
            var result = await _handler.Handle(command,
                new CreateTaskCommandData { Title = title, Description = description, DueDateUtc = DateTime.UtcNow.AddDays(daysOffset) });
            
            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.Not.EqualTo(Guid.Empty));
            var existInDb = await context.Tasks.AnyAsync(_ => _.TaskId == result.Value);
            Assert.IsTrue(existInDb);
            await context.Tasks.Where(_ => _.TaskId == result.Value).ExecuteDeleteAsync();
        }

        [Test]
        public async Task DeleteTask()
        {
            await using var context = new UtisTestContext(Settings.TestDbConnectionString);
            var taskId = Guid.NewGuid();
            var task = await context.Tasks.AddAsync(new TaskEntity{Title = "test1", Description = "desc1", DueDateUtc = DateTime.UtcNow, Status = ScheduledTaskStatus.New, TaskId = taskId });
            await context.SaveChangesAsync();
            task.State = EntityState.Detached;

            var existInDb = await context.Tasks.AnyAsync(_ => _.TaskId == taskId);
            Assert.IsTrue(existInDb);

            var command = new DeleteTaskCommand(new CustomConsoleLogger<DeleteTaskCommand>(), context, new DeleteTaskCommandValidator(context));
            var result = await _handler.Handle(command,
                new DeleteTaskCommandData { TaskId = taskId });
            Assert.IsNotNull(result);

            existInDb = await context.Tasks.AnyAsync(_ => _.TaskId == taskId);
            Assert.IsFalse(existInDb);
        }
    }
}