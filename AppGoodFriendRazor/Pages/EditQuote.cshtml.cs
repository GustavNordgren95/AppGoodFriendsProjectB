using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Services;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace AppGoodFriendRazor.Pages
{
    public class EditQuoteModel : PageModel
    {
        private readonly IFriendsService _friendsService;
        public IQuote Quote { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid FriendId { get; set; }

        public EditQuoteModel(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        public async Task<IActionResult> OnGetAsync(Guid quoteId, Guid friendId)
        {
            Quote = await _friendsService.ReadQuoteAsync(null, quoteId, false);
            if (Quote == null)
            {
                return NotFound();
            }
            FriendId = friendId;

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateQuoteAsync(Guid quoteId, string quoteText, string author)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var quoteDto = new csQuoteCUdto
            {
                QuoteId = quoteId,
                Quote = quoteText,
                Author = author
            };

            await _friendsService.UpdateQuoteAsync(null, quoteDto);
            return RedirectToPage("/FriendDetails", new { id = FriendId });
        }
    }

}
