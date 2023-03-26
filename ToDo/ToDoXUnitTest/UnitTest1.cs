using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ToDo.Controllers;
namespace ToDoXUnitTest
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			var controller = new HomeController();
			var result = controller.Index(1);
			Assert.IsInstanceOfType(result, typeof(ViewResult));
			//Since view has been asserted as ViewResult
			ViewResult viewResult = result as ViewResult;
			//Assert.Equal("Index", result.ViewName);
		}
	}
}