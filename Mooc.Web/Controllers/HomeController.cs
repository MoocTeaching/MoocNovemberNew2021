using Mooc.Services.Interfaces;
using System;
using System.Web.Mvc;

namespace Mooc.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserImageInfoService _userImageInfoService;
        public HomeController(IUserService userService, IUserImageInfoService userImageInfoService)
        {
            this._userService = userService;
            this._userImageInfoService = userImageInfoService;
        }


        public ActionResult Index()
        {
            this._userImageInfoService.Insert(new Dtos.Dtos.UserImageInfoDto()
            {
                MongodbImgId = Guid.NewGuid().ToString(),
                ImageBase64 = "fdsaaaaaaaaaaa"
            });

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}