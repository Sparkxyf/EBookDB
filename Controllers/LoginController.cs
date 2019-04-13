using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleSQLCommand;

namespace EBookDB.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.LoginType = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.UserLogin login)
        {
            string user_name = Request.Form["UserName"];
            string user_password = Request.Form["Password"];
            Login_ login_ = new Login_();
            int login_type = login_.ByIdentity(user_name, user_password);
            ViewBag.LoginType = login_type;
            if (login_type > 0)
            {
                //Session.Add("Login", login_type);
                Session["Login"] = login_type;
                return Content("<script>alert('登录成功，跳转到个人信息页面');window.location.href='/Information'</script>");
            }
            return View();
        }
    }
}