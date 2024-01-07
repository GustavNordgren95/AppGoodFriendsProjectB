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

namespace AppGoodFriendRazor.Pages
{
    public class ListOfFriendsModel : PageModel
    {

        public class FriendIndexData
        {
            public IEnumerable<IFriend> Friends { get; set; }
            public IEnumerable<IAddress> Addresses { get; set; }
        }
        private readonly IFriendsService _friendsService;
        private readonly IFriendsService _addressService;
        private readonly ILogger<ListOfFriendsModel> _logger;

        public List<IFriend> Friends { get; private set; }
        public List<IAddress> Addresses { get; private set; }

        public class FriendAddressViewModel
        {
            public Guid FriendId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public DateTime? Birthday { get; set; }
            public string Address { get; set; }
        }

        public ListOfFriendsModel(IFriendsService friendsService, IFriendsService addressService, ILogger<ListOfFriendsModel> logger)
        {
            _friendsService = friendsService;
            _addressService = addressService;
            _logger = logger;
        }

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string CurrentFilter { get; set; }

        #region HTTP Requests
        public async Task<IActionResult> OnGetAsync(string filter = "", int currentPage = 1, int pageSize = 10)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            CurrentFilter = filter;

            var info = await _friendsService.InfoAsync;
            var totalFriendsCount = info.Db.nrSeededFriends;
            TotalPages = (int)Math.Ceiling((double)totalFriendsCount / PageSize);

            Friends = await _friendsService.ReadFriendsAsync(null, true, true, filter?.Trim()?.ToLower(), CurrentPage - 1, PageSize) ?? new List<IFriend>();

            

            if (!Friends.Any())
            {
                _logger.LogWarning("No friends were found in the database for the current page.");
                Friends = new List<IFriend>();
            }

            return Page();
        }
        #endregion
    }
}