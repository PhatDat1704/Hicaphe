using hicaphe2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hicaphe2.Controllers
{
    public class ProxyClass
    {
        private readonly IAdminController _adminController;

        public ProxyClass()
        {
            _adminController = new AdminControllerProxy();
        }

        //sử dụng AdminControllerProxy
        public void SomeMethod()
        {
            // Gọi các phương thức thông qua proxy
            _adminController.Index();
            _adminController.Login();
            _adminController.Login(new ADMIN());
        }
    }

}