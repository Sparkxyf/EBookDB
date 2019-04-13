using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleSQLCommand;

namespace EBookDB.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]              //这段代码会在点击提交按钮后执行
        public ActionResult Index(Models.UserRegister user)
        {
            //ViewBag.RegisterType传递注册情况
            string user_name = Request.Form["UserName"];
            string user_password = Request.Form["Password"];
            string ensure_password = Request.Form["EnsurePassword"];
            Check_If_Exist check = new Check_If_Exist();
            if (check.TYPE_STRING("USER_IDENTITY", "USER_NAME", user_name))
            {
                ViewBag.RegisterType = "Exist";
                return View();
            }
            if(user_password != ensure_password)
            {
                ViewBag.RegisterType = "NotMatch";
                return View();
            }
            Create_New_User create = new Create_New_User();
            create.Create(user_name, user_password, 1);
            ViewBag.RegisterType = "Finish";
            return View();
        }

        //这段代码会在进入页面时执行
        public ActionResult Test()
        {
            return Redirect("Index");
        }


    }
}