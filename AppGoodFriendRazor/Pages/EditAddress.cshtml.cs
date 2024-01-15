using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Services;
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

        public async Task<IActionResult> OnPostEditAddress(Guid addressId)
        {


            return Page();
        }

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
            public csAddressIM(IAddress model)
            {
                StatusIM = enStatusIM.Unchanged;
                AddressId = model.AddressId;
                StreetAddress = model.StreetAddress;
                ZipCode = model.ZipCode;
                City = model.City;
                Country = model.Country;
            }
        }
    }
}
