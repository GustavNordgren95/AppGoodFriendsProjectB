using DbRepos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DTO;
using Services;

namespace AppGoodFriendRazor.Pages
{
    public class FriendsInCityModel : PageModel
    {
        private readonly csFriendsDbRepos _repository;

        public string Country { get; set; }
        public List<gstusrInfoFriendsDto> FriendsInCountry { get; set; }
        public List<gstusrInfoPetsDto> PetsInCountry { get; set; }

        public FriendsInCityModel(csFriendsDbRepos repository)
        {
            _repository = repository;
        }

        public async Task OnGetAsync(string country)
        {
            // Guard clause to ensure country is not null or empty
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentNullException(nameof(country));
            }

            Country = country;
            var info = await _repository.InfoAsync();

            // Check if info or its properties are null and handle accordingly
            if (info == null || info.Friends == null || info.Pets == null)
            {
                FriendsInCountry = new List<gstusrInfoFriendsDto>();
                PetsInCountry = new List<gstusrInfoPetsDto>();
                return;
            }

            FriendsInCountry = info.Friends
              .Where(friend => friend.City != null && friend.City != "Total" && friend.Country.Equals(Country, StringComparison.OrdinalIgnoreCase))
              .ToList();

            PetsInCountry = info.Pets
                .Where(pet => pet.City != null && pet.City != "Total" && pet.Country.Equals(Country, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

    }
}
