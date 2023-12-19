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

        public async Task<IActionResult> OnGetAsync(string seeded = "true", string flat = "true",
            string filter = null, string pageNr = "0", string pageSize = "5")
        {

            var uri = Request.Path;

            bool _seeded = true;
            if (!bool.TryParse(seeded, out _seeded))
            {
                return BadRequest("seeded format error");
            }

            bool _flat = false;
            if (!bool.TryParse(flat, out _flat))
            {
                return BadRequest("flat format error");
            }

            int _pageNr = 0;
            if (!int.TryParse(pageNr, out _pageNr))
            {
                return BadRequest("pageNr format error");
            }

            int _pageSize = 0;
            if (!int.TryParse(pageSize, out _pageSize))
            {
                return BadRequest("pageSize format error");
            }

            if (Friends == null)
            {
                _logger.LogError("Friends service returned null.");
                Friends = new List<IFriend>(); // Ensure Friends is never null
            }
            else if (!Friends.Any())
            {
                _logger.LogWarning("No friends were found in the database.");
            }

            Friends = await _friendsService.ReadFriendsAsync(null, _seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);
            Addresses = await _friendsService.ReadAddressesAsync(null, _seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);
            return Page();
        }
    }
}
