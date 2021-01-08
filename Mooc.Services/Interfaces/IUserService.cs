using Mooc.Core.IDependency;
using Mooc.Dtos.Base;
using Mooc.Dtos.User;
using Mooc.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mooc.Services.Interfaces
{
    public interface IUserService : IDependency
    {
        Task<bool> Add(CreateOrUpdateUserDto createOrUpdateUserDto);

        List<UserDto> GetList();

        PageResult<UserDto> GetListByPage(int pageSize,int pageNumber);

        Task<UserDto> GetUser(int id);
        Task<UserDto> GetUser(string userName);

        Task<CreateOrUpdateUserDto> GetEditUser(int id);

        bool Update(CreateOrUpdateUserDto updateUser);

        bool Delete(int deleteUser);
        
        List<UserDto> GetLoginUser(string email,string pw);


    }
}
