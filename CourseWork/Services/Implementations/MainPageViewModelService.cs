using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.ViewModels.Shared;
using CourseWork.Services.Interfaces;
using CourseWork.ViewModels.Index;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Services.Implementations
{
	public class MainPageViewModelService : IMainPageViewModelService
	{
		public readonly IRepository<Collection> _collectionRepository;

		public MainPageViewModelService(IRepository<Collection> collectionRepository)
		{
			_collectionRepository = collectionRepository;
		}

		public async Task<IndexViewModel> GetIndexViewModel(int page, int pageSize, string searchString, Theme? theme)
		{
			var collections = await _collectionRepository.GetAll()
				.OrderByDescending(x => x.Date)
				.ToListAsync();

			if (!string.IsNullOrWhiteSpace(searchString))
			{
				collections = collections.Where(c => c.Name.Contains(searchString)
					|| c.Author.Contains(searchString) || c.Description.Contains(searchString))
					.ToList();
			}

			if (theme != null)
			{
				collections = collections.Where(c => c.Theme == theme).ToList();
			}

			var count = collections.Count;
			var items = collections.Skip((page - 1) * pageSize).Take(pageSize).ToList();

			var pagination = new Pagination(count, page, pageSize);
			var indexViewModel = new IndexViewModel()
			{
				Pagination = pagination,
				Collections = items,
				Themes = GetThemes().ToList(),
				ThemeFilterApplied = theme != null ? theme : null,
				SearchString = string.IsNullOrWhiteSpace(searchString) ? string.Empty : searchString
			};

			return indexViewModel;
		}

		private IEnumerable<SelectListItem> GetThemes()
		{
			var themes = new List<SelectListItem>()
			{
				new SelectListItem() { Value = null, Text = "All", Selected = true },
				new SelectListItem() { Value = Theme.Books.ToString(), Text = Theme.Books.ToString() },
				new SelectListItem() { Value = Theme.Signs.ToString(), Text = Theme.Signs.ToString() },
				new SelectListItem() { Value = Theme.Silverware.ToString(), Text = Theme.Silverware.ToString() },
				new SelectListItem() { Value = Theme.Pictures.ToString(), Text = Theme.Pictures.ToString() },
				new SelectListItem() { Value = Theme.Coins.ToString(), Text = Theme.Coins.ToString() },
				new SelectListItem() { Value = Theme.Other.ToString(), Text = Theme.Other.ToString() },
			};

			return themes;
		}
	}
}
