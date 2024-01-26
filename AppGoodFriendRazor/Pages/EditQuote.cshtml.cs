using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Services;
using System.ComponentModel.DataAnnotations;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace AppGoodFriendRazor.Pages
{
    public class EditQuoteModel : PageModel
    {
        private readonly IFriendsService _friendsService;

        [BindProperty]
        public QuoteInputModel QuoteIM { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid FriendId { get; set; }

        public EditQuoteModel(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        public async Task<IActionResult> OnGetAsync(Guid quoteId, Guid friendId)
        {
            var quote = await _friendsService.ReadQuoteAsync(null, quoteId, false);
            if (quote == null)
            {
                return NotFound();
            }

            QuoteIM = new QuoteInputModel
            {
                QuoteId = quote.QuoteId,
                Text = quote.Quote,
                Author = quote.Author
            };

            FriendId = friendId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var quoteDto = new csQuoteCUdto
            {
                QuoteId = QuoteIM.QuoteId,
                Quote = QuoteIM.Text,
                Author = QuoteIM.Author
            };

            await _friendsService.UpdateQuoteAsync(null, quoteDto);
            return RedirectToPage("/FriendDetails", new { id = FriendId });
        }
    }

    public class QuoteInputModel
    {
        public Guid QuoteId { get; set; }

        [Required(ErrorMessage = "The quote text is required.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "The author is required.")]
        public string Author { get; set; }
    }
}
