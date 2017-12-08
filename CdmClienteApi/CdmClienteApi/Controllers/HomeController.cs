using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CdmClienteApi.Controllers
{
    public class HomeController :  Controller
    {

        //Dúvidas me chama no whats: (11) 98559-0116


        // GET: Default
        public ActionResult Index()
        {            
            return View();
        }


    }
}