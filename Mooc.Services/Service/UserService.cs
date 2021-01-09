using AutoMapper;
using Mooc.DataAccess.Context;
using Mooc.Dtos.Base;
using Mooc.Dtos.User;
using Mooc.Models.Entities;
using Mooc.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Mooc.Services.Service
{
    public class UserService : IUserService
    {
        private DataContext _db;
        //private IMapper _mapper;
        public UserService(IDataContextProvider dataContextProvider)
        {
            this._db = dataContextProvider.GetDataContext();
            //this._mapper = mapper;
        }

        public async Task<bool> Add(CreateOrUpdateUserDto createOrUpdateUserDto)
        {
            try
            {
                var user = Mapper.Map<User>(createOrUpdateUserDto);
                this._db.Users.Add(user);
                return await this._db.SaveChangesAsync() > 0;
            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                throw;
            }

        }
        public bool Delete(int deleteID)
        {
            var deleteUser =  _db.Users.Find(deleteID);
            //var user = Mapper.Map<User>(deleteUser);
            this._db.Users.Remove(deleteUser);
            return this._db.SaveChanges() > 0;
        }

        public List<UserDto> GetList()
        {
            var list = _db.Users.ToList();
            return Mapper.Map<List<UserDto>>(list);
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(p => p.Id == id);
            if (user == null)
                return null;
            return Mapper.Map<UserDto>(user);
        }

        //Edit
        public async Task<CreateOrUpdateUserDto> GetEditUser(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(p => p.Id == id);

            if (user == null)
                return null;

            return Mapper.Map<CreateOrUpdateUserDto>(user);

        }

        //Update Edit items
        public async Task<bool> Update(CreateOrUpdateUserDto updateUserDto)
        {
            try
            {
                var user = this._db.Users.FirstOrDefault(p => p.Id == updateUserDto.Id);

                Mapper.Map<CreateOrUpdateUserDto, User>(updateUserDto, user);
                user.AddTime = DateTime.Now;
                //var teacher = Mapper.Map<Teacher>(updateTeacher);
                //this._db.Teachers.Add(teacher);
                return await this._db.SaveChangesAsync() > 0;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    //Log.Error(eve.Entry.Entity.GetType().Name.ToString());
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                throw;
            }
        }

        public List<UserDto> GetLoginUser(string email, string password)
        {
            var user = _db.Users.Where(s => s.Email.Equals(email) && s.PassWord.Equals(password)).ToList();
            if (user == null)
                return null;
            return Mapper.Map<List<UserDto>>(user);
        }

        public async Task<UserDto> GetUser(string userName)
        {
            User user = await _db.Users.FirstOrDefaultAsync(p => p.UserName == userName);
            if (user == null)
                return null;
            return Mapper.Map<UserDto>(user);
        }

        public PageResult<UserDto> GetListByPage(int pageSize, int pageNumber)
        {
            PageResult<UserDto> pageResult = new PageResult<UserDto>();
            pageResult.Count = _db.Users.Count();

            var list = _db.Users.OrderBy(p=>p.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            pageResult.data = Mapper.Map<List<UserDto>>(list);
            return pageResult;

        }
    }
}