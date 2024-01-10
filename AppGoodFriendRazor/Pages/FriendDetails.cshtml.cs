using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Services;
using System;
using System.Threading.Tasks;

namespace AppGoodFriendRazor.Pages
{
    public class FriendDetailsModel : PageModel
    {
        private readonly IFriendsService _friendsService;
        private readonly ILogger<ListOfFriendsModel> _logger;

        public IFriend Friend { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public FriendDetailsModel(IFriendsService friendsService, ILogger<ListOfFriendsModel> logger)
        {
            _friendsService = friendsService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }

            Friend = await _friendsService.ReadFriendAsync(null, Id, false);
            if (Friend == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
