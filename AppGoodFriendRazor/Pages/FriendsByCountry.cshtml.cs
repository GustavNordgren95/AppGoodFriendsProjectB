using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DTO;
using Services;

namespace AppGoodFriendRazor.Pages
{
    public class FriendsByCountryModel : PageModel
    {
        private readonly ILogger<FriendsByCountryModel> _logger;
        private IFriendsService _service;

        public List<FriendsByCountry> friendsByCountry = new List<FriendsByCountry>();

        public int TotalCitiesWithFriends { get; set; }
        public int FriendsWithoutCountry { get; set; }

        public FriendsByCountryModel(ILogger<FriendsByCountryModel> logger, IFriendsService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task OnGet()
        {
            var info = await _service.InfoAsync;
            var friends = info.Friends;
            var dbInfo = info.Db;

            var groupedWithCountry = friends
                .Where(friend => !string.IsNullOrEmpty(friend.Country))
                .GroupBy(friend => friend.Country)
                .Select(countryGroup => new FriendsByCountry
                {
                    Country = countryGroup.Key,
                    Cities = countryGroup
                        .Where(friend => !string.IsNullOrEmpty(friend.City))
                        .GroupBy(friend => friend.City)
                        .Select(cityGroup => new gstusrInfoFriendsDto
                        {
                            City = cityGroup.Key,
                            NrFriends = cityGroup.Sum(friend => friend.NrFriends)
                        })
                        .ToList(),
                    CityCount = countryGroup
                        .Select(friend => friend.City)
                        .Where(city => !string.IsNullOrEmpty(city))
                        .Distinct()
                        .Count()
                });

            var unknownCountryGroup = friends
                .Where(friend => string.IsNullOrEmpty(friend.Country))
                .GroupBy(friend => "Unknown")
                .Select(group => new FriendsByCountry
                {
                    Country = group.Key,
                    Cities = new List<gstusrInfoFriendsDto>(),
                    CityCount = 0
                });

            friendsByCountry = groupedWithCountry.Concat(unknownCountryGroup).ToList();

            FriendsWithoutCountry = dbInfo.nrSeededFriends + dbInfo.nrUnseededFriends - dbInfo.nrFriendsWithAddress;


        }
    }


    public class FriendsByCountry
    {
        public string Country { get; set; }
        public List<gstusrInfoFriendsDto> Cities { get; set; }
        public int CityCount { get; set; }
    }
}
