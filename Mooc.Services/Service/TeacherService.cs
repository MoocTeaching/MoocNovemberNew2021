using AutoMapper;
using Mooc.Dtos.Teacher;
using Mooc.Models.Entities;
using Mooc.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Mooc.Utils;
using Mooc.DataAccess.Context;
using Mooc.Dtos.Base;

namespace Mooc.Services.Service
{
    public class TeacherService : ITeacherService
    {
        private DataContext _db;
        private IUserImageInfoService _userImageInfoService;
        public TeacherService(IDataContextProvider dataContextProvider, IUserImageInfoService userImageInfoService)
        {
            this._db = dataContextProvider.GetDataContext();
            this._userImageInfoService = userImageInfoService;
        }

        //Add
        public async Task<bool> Add(CreateOrUpdateTeacherDto createOrUpdateTeacherDto)
        {
            try
            {
                var teacher = Mapper.Map<Teacher>(createOrUpdateTeacherDto);
                this._db.Teachers.Add(teacher);
                return await this._db.SaveChangesAsync() > 0;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var er in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation error:",
                        er.Entry.Entity.GetType().Name, er.Entry.State);
                    foreach (var ve in er.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public List<TeacherDto> GetList()
        {
            var list = _db.Teachers.ToList();
            return Mapper.Map<List<TeacherDto>>(list);
        }

        public async Task<TeacherDto> GetTeacher(int id)
        {
            var teacher = await _db.Teachers.FirstOrDefaultAsync(p => p.Id == id);
            if (teacher == null)
                return null;
            return Mapper.Map<TeacherDto>(teacher);
        }

        //Edit
        public async Task<CreateOrUpdateTeacherDto> GetEditTeacher(int id)
        {
            try
            {
                var teacher = await _db.Teachers.FirstOrDefaultAsync(p => p.Id == id);

                if (teacher == null)
                    return null;


                var teacherDto = Mapper.Map<CreateOrUpdateTeacherDto>(teacher);
                //获取图片
                if (string.IsNullOrEmpty(teacherDto.MongodbImgId))
                {
                    return teacherDto;
                }

                var userImageInfoDto = await _userImageInfoService.Get(teacherDto.MongodbImgId);
                if (userImageInfoDto != null)
                {
                     teacherDto.ImageBase64Data = userImageInfoDto.ImageBase64;

                }
                return teacherDto;
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

        //Update
        public async Task<bool> Update(CreateOrUpdateTeacherDto updateTeacher)
        {
            try
            {
                var teacher = this._db.Teachers.FirstOrDefault(p => p.Id == updateTeacher.Id);

                Mapper.Map<CreateOrUpdateTeacherDto, Teacher>(updateTeacher, teacher);
                teacher.AddTime = DateTime.Now;
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

        public bool Delete(int deleteID)
        {
            var teacher = _db.Teachers.Find(deleteID);
            this._db.Teachers.Remove(teacher);
            return this._db.SaveChanges() > 0;
        }

        public async Task<TeacherDto> GetTeacher(string teacherName)
        {
            Teacher teacher = await _db.Teachers.FirstOrDefaultAsync(p => p.TeacherName == teacherName);
            if (teacher == null)
                return null;
            return Mapper.Map<TeacherDto>(teacher);
        }

        public PageResult<TeacherDto> GetListByPage(int pageSize, int pageNumber)
        {
            PageResult<TeacherDto> pageResult = new PageResult<TeacherDto>();
            pageResult.Count = _db.Teachers.Count();
            var list = _db.Teachers.OrderBy(p => p.AddTime).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            pageResult.data = Mapper.Map<List<TeacherDto>>(list);
            return pageResult;
        }
    }
}
