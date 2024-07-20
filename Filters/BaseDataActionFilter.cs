using Hotel.Data;
using Hotel.Dtos;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hotel.Filters
{
	public class BaseDataActionFilter : IActionFilter
	{
		private readonly HotelDbContext _dbContext;
		

		public BaseDataActionFilter(HotelDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var roomData = _dbContext.RoomTypes.ToList();
			context.HttpContext.Items["BaseData"] = roomData;
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
		}
	}

}
