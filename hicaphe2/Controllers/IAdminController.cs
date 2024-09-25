using hicaphe2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace hicaphe2.Controllers
{
    public interface IAdminController
    {
        ActionResult Index();
        ActionResult Login();
        ActionResult Login(ADMIN admin);
    }
}
