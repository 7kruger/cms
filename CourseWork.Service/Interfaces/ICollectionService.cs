using CourseWork.Domain.Entities;
using CourseWork.Domain.Response;
using CourseWork.Domain.ViewModels.Collection;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
	public interface ICollectionService
	{
		Task<IBaseResponse<List<Collection>>> GetCollections();
		Task<IBaseResponse<List<Collection>>> SearchByValue(string value);
		Task<IBaseResponse<CollectionViewModel>> GetCollection(string id);
		Task<IBaseResponse<Collection>> Create(CreateCollectionViewModel model, string username, IFormFile image);
		Task<IBaseResponse<Collection>> Edit(string id, CollectionViewModel model, string[] selectedItems, string[] tags, IFormFile image);
		Task<IBaseResponse<bool>> Delete(string id);
	}
}
