using LinkUp.Application.DTos.PostsCategories;
using LinkUp.Application.Interfaces.Base;
using PostCategoryDto = LinkUp.Application.DTos.Posts.PostCategoryDto;

namespace LinkUp.Application.Interfaces.Services;

public interface IPostCategoryService : IGenericService<CreatePostCategoryDto, UpdatePostCategoryDto, CategoryDto>;