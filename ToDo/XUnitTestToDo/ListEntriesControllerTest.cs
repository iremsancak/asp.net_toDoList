using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using ToDo.Controllers;
using ToDo.Data;
using ToDo.Models;

namespace XUnitTestToDo
{
	//Test by https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Testing/TestingWithoutTheDatabase/SqliteInMemoryBloggingControllerTest.cs
	public class ListEntriesControllerTest: IDisposable
	{
		private readonly DbConnection _connection;
		private readonly DbContextOptions<ToDoContext> _contextOptions;
		public ListEntriesControllerTest()
		{
			// Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
			// at the end of the test (see Dispose below).
			_connection = new SqliteConnection("Filename=:memory:");
			_connection.Open();

			// These options will be used by the context instances in this test suite, including the connection opened above.
			_contextOptions = new DbContextOptionsBuilder<ToDoContext>()
				.UseSqlite(_connection)
				.Options;

			// Create the schema and seed some data
			using var context = new ToDoContext(_contextOptions);

			//Auskommentieren? von hier...
			if (context.Database.EnsureCreated())
			{
				using var viewCommand = context.Database.GetDbConnection().CreateCommand();
				viewCommand.CommandText = @"
		CREATE VIEW AllResources AS
		SELECT Description
			FROM ListEntry;";
				viewCommand.ExecuteNonQuery();
			}
			//...Bis hier
			context.ListEntry.AddRange(
				new ListEntry { Id = 1, Description = "Task1", isDone = false},
				new ListEntry { Id = 2, Description = "Task2", isDone = false });
			context.SaveChanges();
		}
		ToDoContext CreateContext() => new ToDoContext(_contextOptions);
		public void Dispose() => _connection.Dispose();

		[Fact]
		public void Mock_DbContext_ListEntry_Test()
		{
			using var context = CreateContext();
			Assert.Equal(2, context.ListEntry.ToList().Count);
		}
		[Fact]
		public async void ToDoList_Get_Test()
		{
			using var context = CreateContext();
			var controller = new ListEntriesController(context);
			var result = await controller.Index() as ViewResult;
		}
		[Fact]
		public async void Edit_Get_Test()
		{
			using var context = CreateContext();
			var controller = new ListEntriesController(context);
			var result = await controller.Edit(1) as ViewResult;
			Assert.Equal(context.ListEntry.ToList().Find(x => x.Id == 1), result.Model);
		}
		[Fact]
		public async void Edit_Post_Test()
		{
			using var context = CreateContext();
			var controller = new ListEntriesController(context);
			var result = await controller.Edit(2, new ListEntry { Id = 2, Description = "Task_edited", isDone = false }) as ViewResult;
			Assert.Equal("Task_edited", context.ListEntry.ToList().Find(x => x.Id == 2).Description);
		}
		[Fact]
		public async void Delete_Get_Test()
		{
			using var context = CreateContext();
			var controller = new ListEntriesController(context);
			var result = await controller.Delete(1) as ViewResult;
			Assert.Equal(context.ListEntry.ToList().Find(x => x.Id == 1), result.Model);
		}
		[Fact]
		public async void Delete_Post_Test()
		{
			using var context = CreateContext();
			var controller = new ListEntriesController(context);
			await controller.DeleteConfirmed(1);
			Assert.Equal(1, context.ListEntry.ToList().Count);
		}
		[Fact]
		public void Create_Get_Test()
		{
			using var context = CreateContext();
			var controller = new ListEntriesController(context);
			var result = controller.Create() as ViewResult;
			Assert.Equal("Create", result.ViewName);
		}
		[Fact]
		public void Create_Post_Test()
		{
			using var context = CreateContext();
			var controller = new ListEntriesController(context);
			var result = controller.Create(new ListEntry { Id = 3, Description = "Task_edited", isDone = false });
			Assert.Equal(3, context.ListEntry.ToList().Count);
		}
		[Fact]
		public async void Delete_Get_Invalid_Parameter_Test()
		{
			using var context = CreateContext();
			var controller = new ListEntriesController(context);
			var result = await controller.Delete(3) as ViewResult;
			Assert.Null(result);
		}
		[Fact]
		public async void Edit_Get_Invalid_Parameter_Test()
		{
			using var context = CreateContext();
			var controller = new ListEntriesController(context);
			var result = await controller.Edit(3) as ViewResult;
			Assert.Null(result);
		}
	}
}