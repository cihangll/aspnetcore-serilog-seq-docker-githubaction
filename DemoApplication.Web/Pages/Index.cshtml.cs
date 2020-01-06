using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

namespace DemoApplication.Web.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
			_logger.LogInformation("Hello World!");

			for (int i = 0; i <= 10; i++)
			{
				if (i % 2 == 0)
					_logger.LogInformation("{value} is even.", i);
				else
				{
					_logger.LogWarning("{value} is odd.", i);
				}
			}

			try
			{
				string value = "test value";
				value = null;

				value.Split(";");
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error occured!");
			}
		}
	}
}
