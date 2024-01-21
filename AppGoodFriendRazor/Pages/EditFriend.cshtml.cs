using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Models.DTO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using static AppGoodFriendRazor.Pages.EditFriendModel;
using System.ComponentModel.DataAnnotations;
using Models;
using static Npgsql.PostgresTypes.PostgresCompositeType;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;

namespace AppGoodFriendRazor.Pages
{
    public class EditFriendModel : PageModel
    {
        private readonly IFriendsService _friendsService = null;
        private readonly ILogger<EditFriendModel> _logger;

        [BindProperty]
        public csFriendIM FriendIM { get; set; }

        [BindProperty]
        public string PageHeader { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public EditFriendModel(IFriendsService friendsService, ILogger<EditFriendModel> logger)
        {
            _friendsService = friendsService;
            _logger = logger;
        }

        #region HTTP Requests
        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }

            var friend = await _friendsService.ReadFriendAsync(null, Id, false);
            if (friend == null)
            {
                return NotFound();
            }

            FriendIM = new csFriendIM(friend);
            PageHeader = "Edit details of a friend";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var friendCUdto = new csFriendCUdto
            {
                FriendId = FriendIM.FriendId,
                FirstName = FriendIM.FirstName,
                LastName = FriendIM.LastName,
                Email = FriendIM.Email,
                Birthday = FriendIM.Birthday,

                AddressId = FriendIM.AddressId,
                PetsId = FriendIM.Pets,
                QuotesId = FriendIM.Quotes    
            };

            await _friendsService.UpdateFriendAsync(null, friendCUdto);

            // Redirect to the FriendDetails page for the edited friend
            return RedirectToPage("./FriendDetails", new { id = FriendIM.FriendId });
        }

        #endregion

        #region Input Model

        public class csFriendIM
        {
            public Guid FriendId { get; set; }

            [Required(ErrorMessage = "You must provide a first name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "You must provide a last name")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "You must provide an email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "You must prove a birthday")]
            public DateTime? Birthday { get; set; }


            //Ids belonging to the related entities used for making sure they are carried along
            //with the updated friend
            public Guid AddressId { get; set; }
            public List<Guid> Pets { get; set; } = new List<Guid>();

            public List<Guid> Quotes { get; set; } = new List<Guid>(); 


            [Required(ErrorMessage = "You must provide a first name")]
            public string editFirstName { get; set; }

            [Required(ErrorMessage = "You must provide a last name")]
            public string editLastName { get; set; }

            [Required(ErrorMessage = "You must provide an email")]
            public string editEmail { get; set; }

            [Required(ErrorMessage = "You must provide a birthday")]
            public DateTime? editBirthday { get; set; }

            

            public IFriend UpdateModel(IFriend model)
            {
                model.FriendId = this.FriendId;
                model.FirstName = this.FirstName;
                model.LastName = this.LastName;
                model.Email = this.Email;
                model.Birthday = this.Birthday;

                return model;
            }

            public csFriendIM() { }

            public csFriendIM(csFriendIM original)
            {
                FriendId = original.FriendId;
                FirstName = original.FirstName;
                LastName = original.LastName;
                Email = original.Email;
                Birthday = original.Birthday;

                editFirstName = original.editFirstName;
                editLastName = original.editLastName;
                editEmail = original.editEmail;
                editBirthday = original.editBirthday;
            }
            public csFriendIM(IFriend model)
            {

                FriendId = model.FriendId;
                FirstName = model.FirstName;
                LastName = model.LastName;
                Email = model.Email;
                Birthday = model.Birthday;

                AddressId = model.Address.AddressId;

                Pets = model.Pets.Select(x => x.PetId).ToList();
                Quotes = model.Quotes.Select(x => x.QuoteId).ToList();
            }

            public class csAddressIM
            {
                public Guid AddressId { get; set; }

                public csAddressIM() { }

                public csAddressIM(csAddressIM original)
                {
                    AddressId = original.AddressId;
                }
            }            
        }
        #endregion
    }
}