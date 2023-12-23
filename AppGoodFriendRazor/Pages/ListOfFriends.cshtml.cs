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
using Models.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace AppGoodFriendRazor.Pages
{
    public class ListOfFriendsModel : PageModel
    {
        private readonly IFriendsService _friendsService;
        private readonly ILogger<ListOfFriendsModel> _logger;

        public List<IFriend> Friends { get; private set; }
        public List<IAddress> Addresses { get; private set; }

        public ListOfFriendsModel(IFriendsService friendsService, ILogger<ListOfFriendsModel> logger)
        {
            _friendsService = friendsService;
            _logger = logger;
        }

        public int CurrentPage { get; set; } = 1; // Current page number
        public int PageSize { get; set; } = 10;   // Number of items per page
        public int TotalPages { get; set; }
        public string CurrentFilter { get; set; }

        public async Task<IActionResult> OnGetAsync(string filter = "", int currentPage = 1, int pageSize = 10)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            CurrentFilter = filter;

            var info = await _friendsService.InfoAsync;
            var totalFriendsCount = info.Db.nrSeededFriends; // Replace TotalFriendsCount with the actual property name that holds the count

            TotalPages = (int)Math.Ceiling((double)totalFriendsCount / pageSize);
            int adjustedPageNumber = CurrentPage - 1;

            Friends = await _friendsService.ReadFriendsAsync(null, true, true, filter?.Trim()?.ToLower(), adjustedPageNumber, PageSize);

            if (Friends == null || !Friends.Any())
            {
                _logger.LogWarning("No friends were found in the database for the current page.");
                Friends = new List<IFriend>(); // Ensure Friends is never null
            }

            return Page();
        }

    }
}