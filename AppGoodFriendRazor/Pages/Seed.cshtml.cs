using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AppGoodFriendRazor.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace AppGoodFriendRazor.Pages
{
    public class SeedModel : PageModel
    {
        //Just like for WebApi
        IFriendsService _service = null;
        ILogger<SeedModel> _logger = null;

        public int NfOfFriends => _nrOfFriends().Result;
        private async Task<int> _nrOfFriends()
        {
            var list = await _service.ReadFriendsAsync(null, true, false, "", int.MaxValue, int.MaxValue);
            return list.Count;
        }

        [BindProperty]
        [Required(ErrorMessage = "You must enter nr of items to seed")]
        public int NrOfItems { get; set; } = 5;

        [BindProperty]
        public bool RemoveSeeds { get; set; } = true;

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (RemoveSeeds)
                {
                    await _service.RemoveSeedAsync(null, true);
                }
                await _service.SeedAsync(null, NrOfItems);

                return Redirect($"~/ListOfFriends");
            }
            return Page();
        }

        //Inject services just like in WebApi
        public SeedModel(IFriendsService service, ILogger<SeedModel> logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}
