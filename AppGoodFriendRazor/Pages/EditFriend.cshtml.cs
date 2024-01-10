using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Models.DTO;
using System;
using System.Threading.Tasks;

namespace AppGoodFriendRazor.Pages
{
    public class EditFriendModel : PageModel
    {
        private readonly IFriendsService _friendsService;
        // Add other services if necessary

        // Define your ViewModel here
        public class FriendViewModel
        {
            public csFriendCUdto Friend { get; set; }
            public csAddressCUdto Address { get; set; }
            // You can also add properties for Pets, Quotes, etc.
        }

        [BindProperty]
        public FriendViewModel FriendData { get; set; }

        public EditFriendModel(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            // Attempt to retrieve the friend and address from the database
            var friendDbM = await _friendsService.ReadFriendAsync(null, id, false);
            var addressDbM = await _friendsService.ReadAddressAsync(null, id, false);

            // Check if the friend or address data is null and handle accordingly
            if (friendDbM == null || addressDbM == null)
            {
                return NotFound();
            }

            // Convert the database models to DTOs safely using null-conditional operators
            FriendData = new FriendViewModel
            {
                Friend = new csFriendCUdto
                {
                    // Safely map properties using null-conditional operators
                    FriendId = friendDbM?.FriendId ?? Guid.Empty,
                    FirstName = friendDbM?.FirstName,
                    // Continue mapping other properties...
                },
                Address = new csAddressCUdto
                {
                    // Safely map properties using null-conditional operators
                    AddressId = addressDbM?.AddressId ?? Guid.Empty,
                    StreetAddress = addressDbM?.StreetAddress,
                    // Continue mapping other properties...
                }
                // Initialize other properties if needed
            };

            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Update friend details
            await _friendsService.UpdateFriendAsync(null, FriendData.Friend);
            // Update address details, assuming you have such a method in your service
            await _friendsService.UpdateAddressAsync(null, FriendData.Address);

            // Redirect to the details page or wherever is appropriate
            return RedirectToPage("FriendDetails", new { id = FriendData.Friend.FriendId });
        }
    }
}

