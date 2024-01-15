using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DTO;
using Models;
using Services;

namespace AppGoodFriendRazor.Pages
{
    public class EditPetModel : PageModel
    {
        private readonly IFriendsService _friendsService;
        private readonly ILogger<EditPetModel> _logger;

        public IFriend Friend { get; set; }
        public IPet Pet { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid petId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public EditPetModel(IFriendsService friendsService, ILogger<EditPetModel> logger)
        {
            _friendsService = friendsService;
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync()
        {

            Friend = await _friendsService.ReadFriendAsync(null, Id, false);

            Pet = await _friendsService.ReadPetAsync(null, petId, false);
            if (Pet == null)
            {
                // If Pet is null, it means no pet was found with the provided ID
                return NotFound($"No pet found with ID {petId}.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdatePetAsync(Guid petId, string petName, enAnimalKind petKind, enAnimalMood petMood, Guid friendId)
        {
            if (friendId == Guid.Empty)
            {

                return Page();
            }

            if (!ModelState.IsValid) { return Page(); }

            var petDto = new csPetCUdto
            {
                PetId = petId,
                Name = petName,
                Kind = petKind,
                Mood = petMood,
                FriendId = friendId
            };

            await _friendsService.UpdatePetAsync(null, petDto);
            return RedirectToPage("/FriendDetails", new { id = friendId });
        }
    }
}
