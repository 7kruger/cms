using AutoMapper;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Enum;
using CourseWork.Service.Interfaces;
using CourseWork.Services.Interfaces;
using CourseWork.ViewModels.Collection;
using CourseWork.ViewModels.Index;
using CourseWork.ViewModels.Item;
using CourseWork.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Services.Implementations
{
	public class MainPageViewModelService : IMainPageViewModelService
	{
		private readonly IRepository<Collection> _collectionRepository;
		private readonly IRepository<Tag> _tagRepository;
		private readonly IItemService _itemService;
		private readonly IMapper _mapper;

		public MainPageViewModelService(IRepository<Collection> collectionRepository,
										IRepository<Tag> tagRepository,
										IItemService itemService,
										IMapper mapper)
		{
			_collectionRepository = collectionRepository;
			_tagRepository = tagRepository;
			_itemService = itemService;
			_mapper = mapper;
		}

		public async Task<IndexViewModel> GetIndexViewModel(int page, string searchString, Theme? theme, SortState? sort, SearchIn? searchIn, string? hashtag)
		{
			if (searchIn == SearchIn.Items)
			{
				return await ItemIndexViewModel(page, searchString, theme, sort, searchIn, hashtag); ;
			}
			return await CollectionIndexViewModel(page, searchString, theme, sort, searchIn, hashtag);
		}

		private async Task<IndexViewModel> CollectionIndexViewModel(int page, string searchString, Theme? theme, SortState? sort, SearchIn? searchIn, string? hashtag)
		{
			var pageSize = Constants.COLLECTIONS_PER_PAGE;
			var collections = _collectionRepository.GetAll();

			if (!string.IsNullOrWhiteSpace(searchString))
			{
				collections = collections.Where(c => c.Title.Contains(searchString)
					|| c.Author.Contains(searchString) || c.Description.Contains(searchString));
			}

			if (theme != null)
			{
				collections = collections.Where(c => c.Theme == theme);
			}

			switch (sort)
			{
				case SortState.NameAsc:
					collections = collections.OrderBy(c => c.Title);
					break;
				case SortState.NameDesc:
					collections = collections.OrderByDescending(c => c.Title);
					break;
				case SortState.DateDesc:
					collections = collections.OrderBy(c => c.Date);
					break;
				default:
					collections = collections.OrderByDescending(c => c.Date);
					break;
			}

			if (!string.IsNullOrWhiteSpace(hashtag))
			{
				var t = await _tagRepository.GetAll().FirstOrDefaultAsync(t => t.Name == hashtag);
				collections = collections.Where(c => c.Tags.Contains(t));
			}

			var count = collections.Count();
			var result = await collections.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			var pagination = new Pagination(count, page, pageSize);
			var indexViewModel = new IndexViewModel()
			{
				Pagination = pagination,
				Collections = _mapper.Map<List<CollectionViewModel>>(result),
				Items = new List<ItemViewModel>(),
				Themes = GetThemes().ToList(),
				ThemeFilterApplied = theme != null ? theme : null,
				SortStates = GetSortStates().ToList(),
				SortFilterApplied = sort ?? null,
				SearchInFilterApplied = searchIn ?? null,
				SearchString = string.IsNullOrWhiteSpace(searchString) ? string.Empty : searchString
			};

			return indexViewModel;
		}

		private async Task<IndexViewModel> ItemIndexViewModel(int page, string searchString, Theme? theme, SortState? sort, SearchIn? searchIn, string? hashtag)
		{
			var pageSize = Constants.ITEMS_PER_PAGE;
			var items = (await _itemService.GetItems())?.AsEnumerable();

			if (!string.IsNullOrWhiteSpace(searchString))
			{
				items = items.Where(c => c.Title.Contains(searchString)
					|| c.Author.Contains(searchString) || c.Description.Contains(searchString));
			}

			switch (sort)
			{
				case SortState.NameAsc:
					items = items.OrderBy(c => c.Title);
					break;
				case SortState.NameDesc:
					items = items.OrderByDescending(c => c.Title);
					break;
				case SortState.DateDesc:
					items = items.OrderBy(c => c.Date);
					break;
				default:
					items = items.OrderByDescending(c => c.Date);
					break;
			}

			var count = items.Count();
			var result = items.Skip((page - 1) * pageSize)
						.Take(pageSize)
						.ToList();

			var pagination = new Pagination(count, page, pageSize);
			var indexViewModel = new IndexViewModel()
			{
				Pagination = pagination,
				Collections = new List<CollectionViewModel>(),
				Items = _mapper.Map<List<ItemViewModel>>(result),
				Themes = GetThemes().ToList(),
				SortStates = GetSortStates().ToList(),
				SortFilterApplied = sort ?? null,
				SearchInFilterApplied = searchIn ?? null,
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

		private IEnumerable<SelectListItem> GetSortStates()
		{
			var sortStates = new List<SelectListItem>()
			{
				new SelectListItem() { Value = SortState.Default.ToString(), Text = "По умолчанию", Selected = true },
				new SelectListItem() { Value = SortState.NameAsc.ToString(), Text = "По имени (А -> Я)", Selected = false },
				new SelectListItem() { Value = SortState.NameDesc.ToString(), Text = "По имени (Я -> А)", Selected = false },
				new SelectListItem() { Value = SortState.DateAsc.ToString(), Text = "Сначала новые", Selected = false },
				new SelectListItem() { Value = SortState.DateDesc.ToString(), Text = "Сначала старые", Selected = false },
			};

			return sortStates;
		}
	}
}
