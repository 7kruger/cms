using System.Collections.Generic;
using System.Threading.Tasks;
using CourseWork.Service.Models;
using Microsoft.AspNetCore.Http;

namespace CourseWork.Service.Interfaces
{
	public interface IItemService
	{
		Task<IEnumerable<ItemModel>> GetItems();
		Task<ItemModel> GetItem(string id);
		Task<ItemModel> Create(ItemModel model, string username, IFormFile image);
		Task<ItemModel> Edit(ItemModel model, IFormFile image);
		Task<bool> Delete(string id);
	}
}
