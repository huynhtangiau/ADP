using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Acme.BookStore.Localization;
using Microsoft.Extensions.Localization;

namespace Acme.BookStore.Web.Pages.Books;

public class EditModalModel : BookStorePageModel
{
    [BindProperty]
    public EditBookViewModel Book { get; set; }

    public List<SelectListItem> Authors { get; set; }

    private readonly IBookAppService _bookAppService;
    private readonly IStringLocalizer<BookStoreResource> _localizer;
    public EditModalModel(IBookAppService bookAppService,
        IStringLocalizer<BookStoreResource> localizer)
    {
        _bookAppService = bookAppService;
        _localizer = localizer;
    }

    public async Task OnGetAsync(Guid id)
    {
        var bookDto = await _bookAppService.GetAsync(id);
        Book = ObjectMapper.Map<BookDto, EditBookViewModel>(bookDto);

        var authorLookup = await _bookAppService.GetAuthorLookupAsync();
        Authors = authorLookup.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        if (!Authors.Any())
        {
            Authors = new List<SelectListItem>();
        }
        Authors.AddFirst(new SelectListItem { Text = _localizer["UI:SelectItems.Default"], Value = string.Empty});
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _bookAppService.UpdateAsync(
            Book.Id,
            ObjectMapper.Map<EditBookViewModel, CreateUpdateBookDto>(Book)
        );

        return NoContent();
    }

    public class EditBookViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [SelectItems(nameof(Authors))]
        [DisplayName("Author")]
        public Guid AuthorId { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public BookType Type { get; set; } = BookType.Undefined;

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Required]
        public float Price { get; set; }
    }
}
