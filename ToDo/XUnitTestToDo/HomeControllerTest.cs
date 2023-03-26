using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Controllers;

namespace XUnitTestToDo
{
	public class HomeControllerTest
	{
		[Fact]
		public void Home_Index_Get_Test()
		{
			var controller = new HomeController();
			var result = controller.Index() as ViewResult;
			Assert.Equal("Index", result?.ViewName);
		}
	}
}
