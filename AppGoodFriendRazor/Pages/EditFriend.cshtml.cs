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

            };

            await _friendsService.UpdateFriendAsync(null, friendCUdto);

            // Redirect to the FriendDetails page for the edited friend
            return RedirectToPage("./FriendDetails", new { id = FriendIM.FriendId });


        }




        #endregion

        #region Input Model
        public enum enStatusIM { Unknown, Unchanged, Inserted, Modified, Deleted }

        public class csPetIM
        {
            public Guid PetId { get; set; }
            public string Name { get; set; }
            // Other pet properties...
        }

        public class csQuoteIM
        {
            public Guid QuoteId { get; set; }
            public string Quote { get; set; }
            public string Author { get; set; }
            // Other quote properties...
        }

        public class csFriendIM
        {
            public enStatusIM StatusIM { get; set; }

            public Guid FriendId { get; set; }

            [Required(ErrorMessage = "First name is required")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "Last name is required")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Birthday is required")]
            public DateTime? Birthday { get; set; }
            [Required(ErrorMessage = "Email is required")]
            public string Email { get; set; }

            public string editFirstName { get; set; }
            public string editLastName { get; set; }
            public DateTime? editBirthday { get; set; }
            public string editEmail { get; set; }

            public Guid? AddressId { get; set; }
            public string StreetAddress { get; set; }
            public int? ZipCode { get; set; }
            public string City { get; set; }
            public string Country { get; set; }

            public Guid? PetId { get; set; }
            public string Name { get; set; }
            public virtual enAnimalKind Kind { get; set; }
            public virtual enAnimalMood Mood { get; set; }

            public Guid? QuoteId { get; set; }
            public string Quote { get; set; }
            public string Author { get; set; }

            public List<csPetIM> Pets { get; set; } = new List<csPetIM>();
            public List<csQuoteIM> Quotes { get; set; } = new List<csQuoteIM>();


            #region constructors and model update

            public csFriendIM() { }
            public csFriendIM(csFriendIM original)
            {
                StatusIM = original.StatusIM;

                FriendId = original.FriendId;
                FirstName = original.FirstName;
                LastName = original.LastName;
                Birthday = original.Birthday;
                Email = original.Email;

                editFirstName = original.editFirstName;
                editLastName = original.editLastName;
                editBirthday = original.editBirthday;
                editEmail = original.editEmail;
            }

            public csFriendIM(IFriend original)
            {
                StatusIM = enStatusIM.Unchanged;
                FriendId = original.FriendId;
                FirstName = editFirstName = original.FirstName;
                LastName = editLastName = original.LastName;
                Birthday = editBirthday = original.Birthday;
                Email = editEmail = original.Email;

                AddressId = original.Address?.AddressId;
                StreetAddress = original.Address?.StreetAddress;
                City = original.Address?.City;
                Country = original.Address?.Country;
                ZipCode = original.Address?.ZipCode;




            }

            public IFriend UpdateModel(IFriend model)
            {
                model.FriendId = FriendId;
                model.FirstName = FirstName;
                model.LastName = LastName;
                model.Birthday = Birthday;
                model.Email = Email;


                return model;
            }
            #endregion

        }
        #endregion
    }
}

