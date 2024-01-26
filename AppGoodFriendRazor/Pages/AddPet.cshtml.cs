using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Services;
using System.ComponentModel.DataAnnotations;

namespace AppGoodFriendRazor.Pages
{
    public class AddPetModel : PageModel
    {
        private readonly IFriendsService _friendsService;
        private readonly ILogger<AddPetModel> _logger;

        public IFriend Friend { get; set; }

        [BindProperty]
        public csPetIM PetIM { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public AddPetModel(IFriendsService friendsService, ILogger<AddPetModel> logger)
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

            // Initialize your input model here if needed
            PetIM = new csPetIM();

            return Page();
        }

        public async Task<IActionResult> OnPostSavePetAsync()
        {

            var petDto = new csPetCUdto
            {
                Name = PetIM.Name,
                Kind = PetIM.Kind,
                Mood = PetIM.Mood,
                FriendId = Id 
            };

            if (PetIM.PetId == null || PetIM.PetId == Guid.Empty)
            {
                // Create a new pet
                await _friendsService.CreatePetAsync(null, petDto);
            }
            else
            {
                // Update an existing pet
                petDto.PetId = PetIM.PetId.Value;
                await _friendsService.UpdatePetAsync(null, petDto);
            }

            return RedirectToPage("/FriendDetails", new { id = Id });
        }

        public class csPetIM
        {
            public Guid? PetId { get; set; } // Nullable for the case of adding new pets

            [Required]
            [Display(Name = "Pet Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Pet Kind")]
            public enAnimalKind Kind { get; set; }

            [Required]
            [Display(Name = "Pet Mood")]
            public enAnimalMood Mood { get; set; }
        }
    }
}

