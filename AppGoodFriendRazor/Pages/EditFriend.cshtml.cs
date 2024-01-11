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
        private readonly IFriendsService _friendsService;
        private readonly ILogger<EditFriendModel> _logger;

        public IFriend Friend { get; set; }

        [BindProperty]
        public csFriendIM FriendInput { get; set; }

        [BindProperty]
        public csFriendIM NewFriendIM { get; set; } = new csFriendIM();

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

            Friend = await _friendsService.ReadFriendAsync(null, Id, false);
            if (Friend == null)
            {
                return NotFound();
            }

            FriendInput = new csFriendIM(Friend);
            PageHeader = "Edit details of a friend";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var friendDto = new csFriendCUdto
                {
                    FriendId = FriendInput.FriendId,
                    FirstName = FriendInput.FirstName,
                    LastName = FriendInput.LastName,
                    Birthday = FriendInput.Birthday,
                    Email = FriendInput.Email
                    
                };

                await _friendsService.UpdateFriendAsync(null, friendDto);

                return RedirectToPage("FriendDetails", new { id = friendDto.FriendId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating friend details");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the friend's details.");
                return Page();
            }
        }

        #endregion

        #region Input Model
        public enum enStatusIM { Unknown, Unchanged, Inserted, Modified, Deleted }
        public class csAddressIM
        {
            public enStatusIM StatusIM { get; set; }

            public Guid AddressId { get; set; }
            public string StreetAddress { get; set; }
            public int ZipCode { get; set; }
            public string City { get; set; }
            public string Country { get; set; }

            public string editStreetAddress { get; set; }
            public int editZipCode { get; set; }
            public string editCity { get; set; }
            public string editCountry { get; set; }

            public csAddress UpdateModel(csAddress model)
            {
                model.AddressId = this.AddressId;
                model.StreetAddress = this.StreetAddress;
                model.ZipCode = this.ZipCode;
                model.City = this.City;
                model.Country = this.Country;

                return model;
            }

            public csAddressIM() { }

            public csAddressIM(csAddressIM original)
            {
                StatusIM = original.StatusIM;
                AddressId = original.AddressId;
                StreetAddress = original.StreetAddress;
                ZipCode = original.ZipCode;
                City = original.City;
                Country = original.Country;

                editStreetAddress = original.editStreetAddress;
                editZipCode = original.editZipCode;
                editCity = original.editCity;
                editCountry = original.editCountry;
            }
            public csAddressIM(csAddress model)
            {
                StatusIM = enStatusIM.Unchanged;
                AddressId = model.AddressId;
                StreetAddress = model.StreetAddress;
                ZipCode = model.ZipCode;
                City = model.City;
                Country= model.Country;
            }
        }

        public class csFriendIM
        {
            public enStatusIM StatusIM { get; set; }

            public Guid FriendId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? Birthday { get; set; }
            public string Email { get; set; }

            public string editFirstName { get; set; }
            public string editLastName { get; set; }
            public DateTime? editBirthday { get; set; }
            public string editEmail { get; set; }

            #region constructors and model update

            public csAddressIM Address { get; set; } = new csAddressIM();

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

            }

            public csFriendIM(csFriend model)
            {
                model.FriendId = FriendId;
                model.FirstName = FirstName;
                model.LastName = LastName;
                model.Birthday = Birthday;
                model.Email = Email;

                
            }
            #endregion

        }
        #endregion
    }
}

