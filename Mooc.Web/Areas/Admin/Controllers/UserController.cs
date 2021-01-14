using AutoMapper;
using Mooc.Dtos.Base;
using Mooc.Dtos.User;
using Mooc.Services.Interfaces;
using Mooc.Utils;
using Mooc.Web.Areas.Admin.Attribute;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Mooc.Web.Areas.Admin.Controllers
{

    [CheckAdminLogin]
    public class UserController : Controller
    {
        // GET: Admin/User

        public ActionResult Index()
        {
            var list = _userService.GetList();
            return View(list);
        }

        private IUserService _userService;
        private ITeacherService _teacherService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        //public UserController(ITeacherService teacherService)
        //{
        //    this._teacherService = teacherService;
        //}
        public ActionResult bvg()
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            //var list = _teacherService.GetList();

            //List<SelectListItem> teacherDropDownList = new List<SelectListItem>();
            //teacherDropDownList = list.Select(c => new SelectListItem() { Text = c.TeacherName, Value = c.Id.ToString() }).ToList();
            //teacherDropDownList.Add(new SelectListItem { Value = "0", Text = "Select", Selected = true });
            //ViewBag.dropDownList = teacherDropDownList;
            return View();
        }

        [HttpGet]
        public JsonResult GetUserListByPage(int pageSize, int pageNumber)
        {
            var result = _userService.GetListByPage(pageSize, pageNumber);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Add Create user
        [HttpPost]

        public async Task<JsonResult> Create(CreateOrUpdateUserDto createorUpdateUserDto)
        {
            CreateOrUpdateUserDto addusr = new CreateOrUpdateUserDto();

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
                    addusr.UserName = createorUpdateUserDto.UserName;
                    addusr.Email = createorUpdateUserDto.Email;
                    //addusr.RoleType.GetTypeCode
                    addusr.Gender = createorUpdateUserDto.Gender;
                    addusr.Major = createorUpdateUserDto.Major;

                    var originPW = createorUpdateUserDto.PassWord;
                    
                    string pwd = MD5Help.MD5Encoding(originPW, ConfigurationManager.AppSettings["sKey"].ToString());
                    addusr.PassWord = pwd;

                    await this._userService.Add(addusr);

                    return Json(new { code = 0 });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }


        public async Task<JsonResult> DeleteUser(int? DeleteID)
        {
            try
            {
                if (DeleteID == null)
                {
                    return Json(new { code = 1, msg = "用户名ID错误" });
                }
                else
                {
                    var user = await this._userService.GetEditUser(DeleteID.Value);

                    if (user != null && ModelState.IsValid)
                    {
                        this._userService.Delete(DeleteID.Value);
                        return Json(new { code = 0, msg = " 用户" + user.UserName+ "已经删除" });
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

        // Reset Password
        [HttpPost] 
        public async Task<JsonResult> Reset(int? ResetID)
        {
            try
            {
                if (ResetID == null)
                {
                    return Json(new { code = 1, msg = "用户名ID错误" });
                }
                else
                {
                    var user = await this._userService.GetEditUser(ResetID.Value);

                    if (user != null)
                    {
                        //Generate Random 12 digit password
                        string newpassword = Membership.GeneratePassword(12, 1);
                        string pwd = MD5Help.MD5Encoding(newpassword, ConfigurationManager.AppSettings["sKey"].ToString());
                        
                        user.PassWord = pwd;
                        
                        if (await _userService.Update(Mapper.Map<CreateOrUpdateUserDto>(user)))
                        {
                            return Json(new { code = 0, msg = " 密码重置为" + newpassword });
                        }
                        else
                        {
                            return Json(new { code = 1, msg = "密码重置失败，检查数据库连接！" });
                        }
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


        public async Task<ActionResult> Edit(int id)
        {
            var user = await _userService.GetEditUser(id);
            return View(user);
        }


        [HttpPost]

        public async Task<JsonResult> Edit(CreateOrUpdateUserDto createOrUpdateUserDto)
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
                    await this._userService.Update(createOrUpdateUserDto);
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