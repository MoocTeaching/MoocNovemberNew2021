using AutoMapper;
using MongoDB.Driver;
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

        public async Task<UserImageInfoDto> Get(string mongodbImageID)
        {
            var collection = _mongodbContextProvider.GetMongodbContext().GetCollection<UserImageInfoDto>("UserImageInfo");

            var filter = Builders<UserImageInfoDto>.Filter.Eq("MongodbImgId", mongodbImageID);

            //var imageInfo1 = collection.Find(filter).First();
            var imageInfo = await collection.Find(filter).FirstAsync(); //Error get data from mongoDB...
            //var imageInfo = await collection.FindAsync(p => p.MongodbImgId == mongodbImageID);
            if (imageInfo == null)
                return null;

            return Mapper.Map<UserImageInfoDto>(imageInfo);
        }
    }
}
