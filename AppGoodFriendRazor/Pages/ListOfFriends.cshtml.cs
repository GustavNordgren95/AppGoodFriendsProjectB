using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Models;
using DbContext;
using Microsoft.EntityFrameworkCore;
using Services;

namespace AppGoodFriendRazor.Pages
{
    //Demonstrate how to use the model to present a list of objects
    public class ListOfFriendsModel : PageModel
    {
        //Just like for WebApi
        IFriendsService _service = null;
        ILogger<ListOfFriendsModel> _logger = null;

        //public member becomes part of the Model in the Razor page
        public List<csFriend> Friends { get; set; } = new List<csFriend>();


        //Will execute on a Get request
        public IActionResult OnGet()
        {
            //Just to show how to get current uri
            var uri = Request.Path;

            //Use the Service
            Friends = _service.ReadFriends();
            return Page();
        }

        //Inject services just like in WebApi
        public ListOfFriendsModel(IFriendsService service, ILogger<ListOfFriendsModel> logger)
        {
            _logger = logger;
            _service = service;
        }
    }
}
