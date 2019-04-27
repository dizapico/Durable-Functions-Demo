using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAB2019.Inception.Web.Models;
using GAB2019.Inception.Web.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GAB2019.Inception.Web.Controllers
{
    public class NotificationController : Controller
    {
        private NotificationSettings notificationSettings;
        private CosmosSettings cosmosSettings;

        public NotificationController(IOptions<NotificationSettings> notificationSettings,
                                        IOptions<CosmosSettings> cosmosSettings)
        {
            this.notificationSettings = notificationSettings.Value;
            this.cosmosSettings = cosmosSettings.Value;
        }

        // GET: Notification
        public ActionResult Index()
        {
            var vm = new NotificationsViewModel(this.notificationSettings, this.cosmosSettings);

            return View(vm);
        }

        //// GET: Notification/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Notification/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Notification/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Notification/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Notification/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Notification/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Notification/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}