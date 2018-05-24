using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Kho.Models;

namespace QL_Kho.Controllers
{
    public class CTPhieuController : Controller
    {
        // GET: CTPhieu
        KhohangDataContext kho = new KhohangDataContext();
        public ActionResult IndexPN()
        {
            return View(kho.CTPHIEUNHAPs.ToList());
        }

       
        public ActionResult IndexPX()
        {
            return View(kho.CTPHIEUXUATs.ToList());
        }
    }
}