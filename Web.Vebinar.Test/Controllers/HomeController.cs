using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Vebinar.Test.Models.DTO;
using Web.Vebinar.Test.Models.Repository;
using System.IO;
using System.Web.Routing;

namespace Web.Vebinar.Test.Controllers
{
    public class HomeController : Controller
    {
        private AutoRepository _repository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var pathToJson = Server.MapPath("~/App_Data/database.json");
            _repository = new AutoRepository(pathToJson);
        }

        public ActionResult Index()
        {
            var autoList = _repository.GetAutoList();
            return View(autoList);
        }

        public ActionResult Create()
        {
            return View("Edit", new Auto());
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var auto = _repository.GetAuto(id);
            return View(auto);
        }

        [HttpPost]
        public ActionResult Edit(Auto auto)
        {
            auto = _repository.SaveAuto(auto);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(Guid id)
        {
            _repository.DeleteAuto(id);
            return RedirectToAction("Index", "Home");
        }

    }
}