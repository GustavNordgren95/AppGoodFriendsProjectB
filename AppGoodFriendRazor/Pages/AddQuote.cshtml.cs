using DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Services;
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

        public List<IQuote> AvailableQuotes { get; set; }

        [BindProperty]
        public Guid SelectedQuoteId { get; set; }

        public AddQuoteModel(IFriendsService friendsService, ILogger<AddQuoteModel> logger)
        {
            _friendsService = friendsService;
            _logger = logger; // Logger is set here
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
            // Reload Friend from Database
            Friend = await _friendsService.ReadFriendAsync(null, Id, false);

            if (Friend == null)
            {
                _logger.LogWarning("No friend found with Id: {Id}", Id);
                return NotFound();
            }

            // Check if the selected quote ID is valid and not already in the friend's list
            if (SelectedQuoteId == Guid.Empty || Friend.Quotes.Any(q => q.QuoteId == SelectedQuoteId))
            {
                // Handle invalid selection or already existing quote
                _logger.LogWarning("Invalid or duplicate QuoteId: {QuoteId}", SelectedQuoteId);
                ModelState.AddModelError("", "Invalid or duplicate quote selection.");
                return Page();
            }

            // Update the Friend by adding the selected quote ID
            var dtoFriend = new csFriendCUdto(Friend);
            if (dtoFriend.QuotesId == null)
            {
                dtoFriend.QuotesId = new List<Guid>();
            }
            dtoFriend.QuotesId.Add(SelectedQuoteId);
            Friend = await _friendsService.UpdateFriendAsync(null, dtoFriend);

            // Redirect to a confirmation page or refresh the current page
            return RedirectToPage("/FriendDetails", new { id = Id });
        }


    }
}