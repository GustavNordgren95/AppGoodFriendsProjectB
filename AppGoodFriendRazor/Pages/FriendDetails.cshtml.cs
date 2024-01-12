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
        private readonly ILogger<FriendDetailsModel> _logger;

        public IFriend Friend { get; set; }
        public IPet Pet { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid PetId { get; set; }

        public FriendDetailsModel(IFriendsService friendsService, ILogger<FriendDetailsModel> logger)
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

        public async Task<IActionResult> OnPostUpdatePetAsync(Guid petId, string petName, enAnimalKind petKind, enAnimalMood petMood, Guid friendId)
        {
            if (friendId == Guid.Empty)
            {
      
                return Page();
            }

            var petDto = new csPetCUdto
            {
                PetId = petId,
                Name = petName,
                Kind = petKind,
                Mood = petMood,
                FriendId = friendId
            };

            await _friendsService.UpdatePetAsync(null, petDto);
            return RedirectToPage(new { id = friendId });
        }

        public async Task<IActionResult> OnPostSavePetAsync(string petName, enAnimalKind petKind, enAnimalMood petMood, Guid friendId, Guid? petId = null)
        {
            if (petId == null || petId == Guid.Empty)
            {
                // Creating a new pet, note to self that the new id is created in the service and doesn't need to be created here
                var newPetDto = new csPetCUdto
                {   
                    Name = petName,
                    Kind = petKind,
                    Mood = petMood,
                    FriendId = friendId
                };
                await _friendsService.CreatePetAsync(null, newPetDto);
            }
            else
            {

                var petDto = new csPetCUdto
                {
                    PetId = petId.Value,
                    Name = petName,
                    Kind = petKind,
                    Mood = petMood,
                    FriendId = friendId
                };
                await _friendsService.UpdatePetAsync(null, petDto);
            }

            return RedirectToPage(new { id = friendId });
        }




    }
}
