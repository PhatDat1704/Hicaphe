using hicaphe2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hicaphe2.Controllers
{
    public class AdminControllerProxy : IAdminController
    {
        private readonly AdminController _adminController;

        public AdminControllerProxy()
        {
            _adminController = new AdminController();
        }

        public ActionResult Index()
        {
            // Kiểm tra ở đây nếu cần phải thực hiện một số kiểm tra trước khi gọi phương thức Index() của AdminController
            return _adminController.Index();
        }

        public ActionResult Login()
        {
            // Kiểm tra ở đây nếu cần phải thực hiện một số kiểm tra trước khi gọi phương thức Login() của AdminController
            return _adminController.Login();
        }

        public ActionResult Login(ADMIN admin)
        {
            // Kiểm tra ở đây nếu cần phải thực hiện một số kiểm tra trước khi gọi phương thức Login(ADMIN admin) của AdminController
            return _adminController.Login(admin);
        }
    }

}