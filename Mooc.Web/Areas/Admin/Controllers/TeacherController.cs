﻿using Mooc.Dtos.Base;
using Mooc.Dtos.Teacher;
using Mooc.Services.Interfaces;
using Mooc.Web.Areas.Admin.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{
    [CheckAdminLogin]
    public class TeacherController : Controller
    {
        private ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            this._teacherService = teacherService;
        }

        public ActionResult Index()
        {
            var list = _teacherService.GetList();
            return View(list);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTeacherList()
        {
            //throw new Exception("Index");
            PageResult<TeacherDto> result = new PageResult<TeacherDto>() { data = new List<TeacherDto>(), PageIndex = 0, PageSize = 0 };
            var listview = _teacherService.GetList();
            result.data = listview;
            result.Count = 0;
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetTeacherListByPage(int pageSize, int pageNumber)
        {
            var result = _teacherService.GetListByPage(pageSize,pageNumber);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public async Task<JsonResult> DeleteTeacher(int? DeleteID)
        {
            try
            {
                if (DeleteID == null)
                {
                    return Json(new { code = 1, msg = "教师用户名ID错误" });
                }
                else
                {
                    var teacher = await this._teacherService.GetEditTeacher(DeleteID.Value);

                    if (teacher != null && ModelState.IsValid)
                    {
                        this._teacherService.Delete(DeleteID.Value);
                        return Json(new { code = 0, msg = " 用户" + teacher.TeacherName + "已经删除" });
                    }
                    else
                    {
                        return Json(new { code = 1, msg = "不能找到相应用户" });
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> Create(CreateOrUpdateTeacherDto createOrUpdateTeacherDto)
        {
            CreateOrUpdateTeacherDto addTeacher = new CreateOrUpdateTeacherDto();

            try
            {
                if(!ModelState.IsValid)
                {
                    var c = ModelState;

                    StringBuilder stringBuilder = new StringBuilder();
                    foreach(var key in ModelState.Keys)
                    {
                        var modelstate = ModelState[key];
                        if(modelstate.Errors.Any())
                        {
                            foreach(var item in modelstate.Errors)
                            {
                                stringBuilder.AppendLine(item.ErrorMessage);
                            }
                        }
                    }
                    return Json(new { code = 1, msg = stringBuilder.ToString() });
                }
                else
                {
                    addTeacher.TeacherName = createOrUpdateTeacherDto.TeacherName;
                    addTeacher.Level = createOrUpdateTeacherDto.Level;
                    addTeacher.Department = createOrUpdateTeacherDto.Department;
                    addTeacher.Company = createOrUpdateTeacherDto.Company;
                    addTeacher.Introduction = createOrUpdateTeacherDto.Introduction;
                    addTeacher.AddTime = DateTime.Now;
                    await this._teacherService.Add(addTeacher);

                    return Json(new { code = 0 });
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }


        public async Task<ActionResult> Edit(int id)
        {
            var teacher = await _teacherService.GetEditTeacher(id);
            return View(teacher);
        }

        [HttpPost]

        public async Task<JsonResult> Edit(CreateOrUpdateTeacherDto createOrUpdateTeacherDto)
        {
            //CreateOrUpdateTeacherDto updateTeacher = new CreateOrUpdateTeacherDto();

            try
            {
                if (!ModelState.IsValid)
                {
                    var c = ModelState;

                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var key in ModelState.Keys)
                    {
                        var modelstate = ModelState[key];
                        if (modelstate.Errors.Any())
                        {
                            foreach (var item in modelstate.Errors)
                            {
                                stringBuilder.AppendLine(item.ErrorMessage);
                            }
                        }
                    }
                    return Json(new { code = 1, msg = stringBuilder.ToString() });
                }
                else
                {
                    //updateTeacher.TeacherName = createOrUpdateTeacherDto.TeacherName;
                    //updateTeacher.Level = createOrUpdateTeacherDto.Level;
                    //updateTeacher.Department = createOrUpdateTeacherDto.Department;
                    //updateTeacher.Company = createOrUpdateTeacherDto.Company;
                    //updateTeacher.Introduction = createOrUpdateTeacherDto.Introduction;
                    //updateTeacher.AddTime = DateTime.Now;
                    await this._teacherService.Update(createOrUpdateTeacherDto);
                    return Json(new { code = 0 });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}