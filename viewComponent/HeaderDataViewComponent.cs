using Microsoft.AspNetCore.Mvc;

namespace Hotel.viewComponent
{
	
		public class HeaderDataViewComponent : ViewComponent
		{
			public IViewComponentResult Invoke()
			{
				var roomData = HttpContext.Items["BaseData"];
				// You can cast the roomData to the appropriate type as needed
				return View(roomData);
			}
		}
	}

