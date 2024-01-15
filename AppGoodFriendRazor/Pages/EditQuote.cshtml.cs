using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppGoodFriendRazor.Pages
{
    public class EditQuoteModel : PageModel
    {
        public void OnGet()
        {
        }
        /*public async Task<IActionResult> OnPostUpdateQuoteAsync(Guid quoteId, string quote, string author, Guid friendId)
        {
            if (friendId == Guid.Empty)
            {
                return Page();
            }

            var quoteDto = new csQuoteCUdto
            {
                QuoteId = quoteId,
                Quote = quote,
                Author = author,
                // Other properties as needed
            };

            await _friendsService.UpdateQuoteAsync(null, quoteDto);
            return RedirectToPage(new { id = friendId });
        }*/
    }
}
