using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoHoangAnh_SE1842NET_A01_FE.Pages.News
{
    public class DetailModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? ArticleId { get; set; }

        public void OnGet()
        {

        }
    }
}