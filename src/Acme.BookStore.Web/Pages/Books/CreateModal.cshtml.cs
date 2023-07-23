using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Books;
using Acme.BookStore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Localization;

namespace Acme.BookStore.Web.Pages.Books;

public class CreateModalModel : BookStorePageModel
{
    [BindProperty]
    public CreateBookViewModel Book { get; set; }

    public List<SelectListItem> Authors { get; set; }

    private readonly IBookAppService _bookAppService;
    private readonly IStringLocalizer<BookStoreResource> _localizer;

    public CreateModalModel(
        IBookAppService bookAppService,
        IStringLocalizer<BookStoreResource> localizer)
    {
        _bookAppService = bookAppService;
        _localizer = localizer;
    }

    public async Task OnGetAsync()
    {
        Book = new CreateBookViewModel();

        var authorLookup = await _bookAppService.GetAuthorLookupAsync();
        Authors = authorLookup.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        if (!Authors.Any())
        {
            Authors = new List<SelectListItem>();
        }
        Authors.AddFirst(new SelectListItem { Text = _localizer["UI:SelectItems.Default"], Value = string.Empty });
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _bookAppService.CreateAsync(
            ObjectMapper.Map<CreateBookViewModel, CreateUpdateBookDto>(Book)
            );
        return NoContent();
    }

    public class CreateBookViewModel
    {
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
