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
using static Npgsql.PostgresTypes.PostgresCompositeType;
using SQLitePCL;
using System.Net;

namespace AppGoodFriendRazor.Pages
{
    public class ListOfFriendsModel : PageModel
    {

        private readonly IFriendsService _friendsService;
        private readonly ILogger<ListOfFriendsModel> _logger;

        public List<IFriend> Friends { get; private set; }
        public ListOfFriendsModel(IFriendsService friendsService, ILogger<ListOfFriendsModel> logger)
        {
            _friendsService = friendsService;
            _logger = logger;
        }

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 0;
        public int TotalPages { get; set; }
        public string CurrentFilter { get; set; }

        #region HTTP Requests
        public async Task<IActionResult> OnGetAsync(string filter = "", int currentPage = 1, int pageSize = 5)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            CurrentFilter = filter;

            // Fetch all friends (consider optimizing this for large datasets)
            var allFriends = await _friendsService.ReadFriendsAsync(null, true, false, "", 0, int.MaxValue);

            // Filter by city if a filter is provided
            if (!string.IsNullOrWhiteSpace(filter))
            {
                allFriends = allFriends.Where(friend => friend.Address?.City?.Equals(filter, StringComparison.OrdinalIgnoreCase) == true).ToList();
            }

            // Calculate total pages based on filtered results
            TotalPages = (int)Math.Ceiling((double)allFriends.Count / PageSize);

            // Apply paging to the filtered result
            Friends = allFriends.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            return Page();
        }

        #endregion
    }
}