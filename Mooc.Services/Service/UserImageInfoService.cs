using AutoMapper;
using Mooc.DataAccess.Context;
using Mooc.Dtos.Dtos;
using Mooc.Models.Entities;
using Mooc.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Services.Service
{
    public class UserImageInfoService : IUserImageInfoService
    {

        private IMongodbContextProvider _mongodbContextProvider;
        public UserImageInfoService(IMongodbContextProvider mongodbContextProvider)
        {
            _mongodbContextProvider = mongodbContextProvider;
        }
        public bool Insert(UserImageInfoDto userImageInfoDto)
        {
            var obj = Mapper.Map<UserImageInfo>(userImageInfoDto);
            _mongodbContextProvider.GetMongodbContext().GetCollection<UserImageInfo>("UserImageInfo").InsertOne(obj);
            return true;
        }
    }
}
