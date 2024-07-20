using Hotel.Data;
using Hotel.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hotel.Controllers
{
	[ServiceFilter(typeof(BaseDataActionFilter))]
	public class MyBaseController : Controller
	{
		internal HotelDbContext _context;

		public MyBaseController( HotelDbContext context) {
		_context=context;
		
		}	
	}
}
