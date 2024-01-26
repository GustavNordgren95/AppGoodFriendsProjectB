using DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Services;
using System.Linq;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace AppGoodFriendRazor.Pages
{
    public class AddQuoteModel : PageModel
    {
        private readonly IFriendsService _friendsService = null;
        private readonly ILogger<AddQuoteModel> _logger;

        public IFriend Friend { get; set; }

        public IQuote Quote { get; set; }

        [BindProperty]
        public string PageHeader { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public csQuoteIM QuoteIM { get; set; }

        public List<IQuote> AvailableQuotes { get; set; }

        [BindProperty]
        public Guid SelectedQuoteId { get; set; }

        public AddQuoteModel(IFriendsService friendsService, ILogger<AddQuoteModel> logger)
        {
            _friendsService = friendsService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            _logger.LogInformation("OnGetAsync called with Id: {Id}", Id);

            if (Id == Guid.Empty)
            {
                _logger.LogWarning("Id is empty");
                return NotFound();
            }

            Friend = await _friendsService.ReadFriendAsync(null, Id, false);
            if (Friend == null)
            {
                _logger.LogWarning("No friend found with Id: {Id}", Id);
                return NotFound();
            }

            var allQuotes = await _friendsService.ReadQuotesAsync(null, true, false, "", int.MaxValue, int.MaxValue);
            var friendQuoteIds = Friend.Quotes.Select(q => q.QuoteId).ToHashSet();

            AvailableQuotes = allQuotes.Where(q => !friendQuoteIds.Contains(q.QuoteId)).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var friend = await _friendsService.ReadFriendAsync(null, Id, false);
                if (friend == null)
                {
                    _logger.LogError($"No friend found with Id: {Id}");
                    return NotFound($"No friend found with ID {Id}.");
                }

                var friendDto = new csFriendCUdto(friend)
                {
                    QuotesId = friend.Quotes?.Select(q => q.QuoteId).ToList() ?? new List<Guid>()
                };

                if (!friendDto.QuotesId.Contains(QuoteIM.SelectedQuoteId))
                {
                    friendDto.QuotesId.Add(QuoteIM.SelectedQuoteId);
                }

                var updatedFriend = await _friendsService.UpdateFriendAsync(null, friendDto);
                if (updatedFriend != null)
                {
                    return RedirectToPage("/FriendDetails", new { id = Id });
                }
                else
                {
                    ModelState.AddModelError("", "Unable to update the friend with new quote.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding quote to friend with ID {Id}", Id);
                ModelState.AddModelError("", "An error occurred while adding the quote.");
            }

            return Page();
        }

        public class  csQuoteIM
        {
            public Guid SelectedQuoteId { get; set; }

        }
    }
}