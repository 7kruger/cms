using System.Collections.Generic;
using System.Threading.Tasks;
using CourseWork.Service.Models;
using Microsoft.AspNetCore.Http;

namespace CourseWork.Service.Interfaces
{
	public interface ICollectionService
	{
		Task<IEnumerable<CollectionModel>> GetCollections();
		Task<IEnumerable<CollectionModel>> SearchByValue(string value);
		Task<CollectionModel> GetCollection(string id);
		Task<CollectionModel> GetCollection(string id, int itemsPageId);
		Task<CollectionModel> Create(CollectionModel model, string username, IFormFile image);
		Task<CollectionModel> Edit(string id, CollectionModel model, string[] selectedItems, string[] tags, IFormFile image);
		Task<bool> Delete(string id);
	}
}
