using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Services;
using System.ComponentModel.DataAnnotations;
using static AppGoodFriendRazor.Pages.EditFriendModel;

namespace AppGoodFriendRazor.Pages
{
    public class EditAddressModel : PageModel
    {
        private readonly IFriendsService _friendsService = null;
        private readonly ILogger<EditAddressModel> _logger;

        [BindProperty]
        public csAddressIM AddressIM { get; set; }

        [BindProperty]
        public string PageHeader { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid FriendId { get; set; }

        public EditAddressModel(IFriendsService friendsService, ILogger<EditAddressModel> logger)
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

            var address = await _friendsService.ReadAddressAsync(null, Id, false);
            if (address == null)
            {
                return NotFound();
            }

            AddressIM = new csAddressIM(address);
            PageHeader = "Edit details of a friend";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var addressCUdto = new csAddressCUdto
            {
                AddressId = AddressIM.AddressId,
                StreetAddress = AddressIM.StreetAddress,
                ZipCode = AddressIM.ZipCode,
                City = AddressIM.City,
                Country = AddressIM.Country

            };

            await _friendsService.UpdateAddressAsync(null, addressCUdto);

            return RedirectToPage("/FriendDetails", new { id = FriendId });
        }

        public class csAddressIM
        {

            public Guid AddressId { get; set; }

            [Required(ErrorMessage = "You must provide a streetaddress")]
            public string StreetAddress { get; set; }

            [Required(ErrorMessage = "You must provide a zip code")]
            public int ZipCode { get; set; }

            [Required(ErrorMessage = "You must provide a city")]
            public string City { get; set; }

            [Required(ErrorMessage = "You must provide a country")]
            public string Country { get; set; }

            [Required(ErrorMessage = "You must provide a streetaddress")]
            public string editStreetAddress { get; set; }

            [Required(ErrorMessage = "You must provide a zip code")]
            public int editZipCode { get; set; }

            [Required(ErrorMessage = "You must provide a city")]
            public string editCity { get; set; }

            [Required(ErrorMessage = "You must provide a country")]
            public string editCountry { get; set; }

            public IAddress UpdateModel(IAddress model)
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
            public csAddressIM(IAddress model)
            {
                AddressId = model.AddressId;
                StreetAddress = model.StreetAddress;
                ZipCode = model.ZipCode;
                City = model.City;
                Country = model.Country;
            }
        }
    }
}
