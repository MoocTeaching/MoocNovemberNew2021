using Mooc.Core.IDependency;
using Mooc.Dtos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Services.Interfaces
{
    public interface IUserImageInfoService : IDependency
    {
        bool Insert(UserImageInfoDto userImageInfoDto);
    }
}
