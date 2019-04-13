using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleSQLCommand;

namespace EBookDB.Controllers
{
    public class InformationController : Controller
    {
        // GET: Information
        /*public SelectListItem GenerateListItem(string text,string value,bool selected)
        {

        }*/
        List<SelectListItem> deptSelectItems = new List<SelectListItem>();


        public ActionResult Index()
        {
            if (Session["Login"] is null)
            {
                return Content("<script>alert('未登录！跳转到登录页面。');window.location.href='/Login'</script>");
            }
            ViewBag.LoginID = Session["Login"];
            Get_Information getinfo = new Get_Information();
            Models.UserInformation info = new Models.UserInformation();
            info = getinfo.USER_INFORMATION_BYID(ViewBag.LoginID);
            Session["NickName"] = info.NickName;
            ViewBag.UserNickName = Session["NickName"];

            //此块代码定义dropdownlist的数据集

            deptSelectItems.Add(new SelectListItem() { Text = "请选择", Value = "-1", Selected = true });
            deptSelectItems.Add(new SelectListItem() { Text = "Male", Value = "male", Selected = false });
            deptSelectItems.Add(new SelectListItem() { Text = "Female", Value = "female", Selected = false });
            deptSelectItems.Add(new SelectListItem() { Text = "None", Value = "none", Selected = false });
            deptSelectItems.Add(new SelectListItem() { Text = "Other", Value = "other", Selected = false });

            ViewData["deptSelectItems"] = deptSelectItems;

            return View(info);
        }

        [HttpPost]
        public ActionResult Index(Models.UserInformation info)
        {
            ViewBag.LoginID = Session["Login"];
            Update update = new Update();
            string nick_name = Request.Form["NickName"];
            string sex = Request.Form["Sex"];
            string location = Request.Form["Location"];

            Session["NickName"] = nick_name;
            ViewBag.UserNickName = Session["NickName"];

            update.TABLE_USER_INFORMATION(ViewBag.LoginID, nick_name, sex, location);

            deptSelectItems.Add(new SelectListItem() { Text = "请选择", Value = "-1", Selected = true });
            deptSelectItems.Add(new SelectListItem() { Text = "Male", Value = "male", Selected = false });
            deptSelectItems.Add(new SelectListItem() { Text = "Female", Value = "female", Selected = false });
            deptSelectItems.Add(new SelectListItem() { Text = "None", Value = "none", Selected = false });
            deptSelectItems.Add(new SelectListItem() { Text = "Other", Value = "other", Selected = false });
            ViewData["deptSelectItems"] = deptSelectItems;

            ViewBag.Changed = 1;
            return View();
        }
    }
}