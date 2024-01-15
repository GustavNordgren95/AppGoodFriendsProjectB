using DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using Services;

namespace AppGoodFriendRazor.Pages.Shared
{
    public class AddQuoteModel : PageModel
    {
        private readonly IFriendsService _friendsService;
        private readonly ILogger<AddPetModel> _logger;

        public IFriend Friend { get; set; }
        public IQuote Quote { get; set; }
        public List<IQuote> Quotes { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid FriendId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid QuoteId { get; set; }

        public async Task<List<IQuote>> ReadAvailableQuotesAsync( Guid friendId, bool seeded, string filter, int pageNumber, int pageSize)
        {
            using (var db = csMainDbContext.DbContext(null))
            {
                filter ??= "";
                var linkedQuoteIds = await db.Friends
                                             .Where(f => f.FriendId == friendId)
                                             .SelectMany(f => f.Quotes)
                                             .Select(q => q.QuoteId)
                                             .ToListAsync();

                var _quotes = db.Quotes.AsNoTracking()
                                       .Where(q => q.Seeded == seeded
                                                   && !linkedQuoteIds.Contains(q.QuoteId)
                                                   && (q.Quote.ToLower().Contains(filter) || q.Author.ToLower().Contains(filter)))
                                       .Skip(pageNumber * pageSize)
                                       .Take(pageSize);

                return await _quotes.ToListAsync<IQuote>();
            }
        }



        public async Task<IActionResult> OnPostSaveNewQuoteAsync(string quote, string author, Guid friendId, Guid? quoteId=null)
        {
            if (string.IsNullOrWhiteSpace(quote) || string.IsNullOrWhiteSpace(author))
            {
                return Page();
            }

            if (quoteId == null || quoteId == Guid.Empty)
            {
                var newQuoteDto = new csQuoteCUdto
                {
                    Quote = quote,
                    Author = author,
                };
                await _friendsService.CreateQuoteAsync(null, newQuoteDto);
            }

            return RedirectToPage(new { id = friendId });
        }
    }
}
