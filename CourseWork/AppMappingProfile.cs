using AutoMapper;
using CourseWork.DAL.Entities;
using CourseWork.Service.Models;
using CourseWork.ViewModels.Account;
using CourseWork.ViewModels.Collection;
using CourseWork.ViewModels.Item;
using CourseWork.ViewModels.Profile;

namespace CourseWork;

public class AppMappingProfile : AutoMapper.Profile
{
	public AppMappingProfile()
	{
		CreateMap<UserModel, User>().ReverseMap();
		CreateMap<UserModel, ChangePasswordViewModel>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name));
		CreateMap<ChangePasswordViewModel, UserModel>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
			.ForMember(dest => dest.Password, opt => opt.MapFrom(x => x.NewPassword));
		CreateMap<UserModel, RegisterViewModel>().ReverseMap();
		CreateMap<UserModel, LoginViewModel>().ReverseMap();

		CreateMap<CollectionModel, Collection>().ReverseMap();
		CreateMap<CollectionModel, CollectionViewModel>().ReverseMap();
		CreateMap<CollectionModel, CreateCollectionViewModel>().ReverseMap();
		CreateMap<CollectionModel, EditCollectionViewModel>().ReverseMap();

		CreateMap<ItemModel, Item>().ReverseMap();
		CreateMap<ItemModel, ItemViewModel>().ReverseMap();
		CreateMap<ItemViewModel, CreateItemViewModel>().ReverseMap();
		CreateMap<ItemViewModel, EditItemViewModel>().ReverseMap();

		CreateMap<ProfileModel, DAL.Entities.Profile>().ReverseMap();
		CreateMap<ProfileModel, ProfileViewModel>().ReverseMap();

		CreateMap<TagModel, Tag>().ReverseMap();
	}
}
