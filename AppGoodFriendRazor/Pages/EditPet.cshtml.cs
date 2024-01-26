using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DTO;
using Models;
using Services;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using static AppGoodFriendRazor.Pages.EditFriendModel;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace AppGoodFriendRazor.Pages
{
    public class EditPetModel : PageModel
    {
        private readonly IFriendsService _friendsService;
        private readonly ILogger<EditPetModel> _logger;

        [BindProperty]
        public csPetIM PetIM { get; set; }

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

            var pet = await _friendsService.ReadPetAsync(null, petId, false);
            if (pet == null)
            {
                return NotFound($"No pet found with ID {petId}.");
            }
            PetIM = new csPetIM(pet);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var petDto = new csPetCUdto
            {
                PetId = PetIM.PetId,
                Name = PetIM.Name,
                Kind = PetIM.Kind,
                Mood = PetIM.Mood,
                FriendId = PetIM.FriendId
            };

            await _friendsService.UpdatePetAsync(null, petDto);
            return RedirectToPage("/FriendDetails", new { id = PetIM.FriendId });
        }

        public class csPetIM
        {
            public Guid PetId { get; set; }

            [Required(ErrorMessage = "Pet name is required.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Please select the kind of pet.")]
            public enAnimalKind Kind { get; set; }

            [Required(ErrorMessage = "Please select the mood of the pet.")]
            public enAnimalMood Mood { get; set; }

            public Guid FriendId { get; set; }

            [Required(ErrorMessage = "Pet name is required.")]
            public string editName { get; set; }

            [Required(ErrorMessage = "Please select the kind of pet.")]
            public enAnimalKind editKind { get; set; }

            [Required(ErrorMessage = "Please select the mood of the pet.")]
            public enAnimalMood editMood { get; set; }

            public IPet UpdateModel(IPet model)
            {
                model.PetId = this.PetId;
                model.Name = this.Name;
                model.Kind = this.Kind;
                model.Mood = this.Mood;

                return model;
            }

            public csPetIM() { }

            public csPetIM(csPetIM original)
            {
                PetId = original.PetId;
                Name = original.Name;
                Kind = original.Kind;
                Mood = original.Mood;

                editName = original.editName;
                editKind = original.editKind;
                editMood = original.editMood;
            }
            public csPetIM(IPet model)
            {
                PetId = model.PetId;
                Kind = model.Kind;
                Mood = model.Mood;

                FriendId = model.Friend.FriendId;
            }
        }

    }
}
