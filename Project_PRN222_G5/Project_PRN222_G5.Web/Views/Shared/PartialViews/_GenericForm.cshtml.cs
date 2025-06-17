using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Project_PRN222_G5.Web.Views.Shared.PartialViews
{
    public class _GenericFormModel : PageModel
    {
        public object Data { get; set; } = null!;
        public Dictionary<string, List<SelectListItem>>? ExtraSelectLists { get; set; }
    }
}