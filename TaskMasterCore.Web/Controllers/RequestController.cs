using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskMasterCore.DAL.Interfaces;
using TaskMasterCore.DAL.Models;
using TaskMasterCore.Web.Models.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace TaskMasterCore.Web.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestsManager _db;
        //private readonly IStringLocalizer<RequestController> _localizer;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public RequestController (IRequestsManager db, 
                   IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _db = db;
            //_localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
        }

        public IActionResult Index(int id)
        {
            var item = _db.Get(id);
           
            ViewBag.allRequests = _db.GetAll().Select(o => new RequestNav(o)).ToList();

            return View(item);
        }

        public IActionResult AllRequests()
        {
            var requests = _db.GetAll();
            return PartialView(requests.Select(o => new RequestNav(o)).ToList());
        }

        public IActionResult Create()
        {
            Request newReq = new Request { Id = 0, StartDate = DateTime.Now, Deadline = DateTime.Now};

            ViewBag.allRequests = _db.GetAll().Select(o => new RequestNav(o)).ToList();

            return View("Edit", newReq);
        }

        [HttpPost]
        public IActionResult Create(int parentId)
        {
            var parent = _db.Get(parentId);

            Request newReq = new Request { Id = 0, ParentId = parentId, StartDate = parent.StartDate, Deadline = parent.Deadline };

            ViewBag.allRequests = _db.GetAll().Select(o => new RequestNav(o)).ToList();

            return View("Edit", newReq);
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            var editReq = _db.Get(id);

            ViewBag.allRequests = _db.GetAll().Select(o => new RequestNav(o)).ToList();

            return View("Edit", editReq);
        }

        [HttpPost]
        public IActionResult Save(Request req)
        {
            var editedReq = _db.Get(req.Id);

            if(editedReq != null)
            {
                if (req.Status == DAL.Models.Enums.Statuses.Completed && editedReq.Status == DAL.Models.Enums.Statuses.Assigned)
                {
                    ModelState.AddModelError("", _sharedLocalizer["errCantCloseAssignet"]);
                }

                if (req.Status == DAL.Models.Enums.Statuses.Completed)
                {
                    if (!_db.CanComplete(editedReq.Id))
                        ModelState.AddModelError("", _sharedLocalizer["errCantCloseChild"]);                    
                }
            }

            if (req.Deadline < req.StartDate)
            {
                ModelState.AddModelError("", _sharedLocalizer["errStartAfterEnd"]);
            }

            if (req.ParentId != null)
            {
                var parentReq = _db.Get((int)req.ParentId);

                if (parentReq.StartDate > req.StartDate)
                    ModelState.AddModelError("", _sharedLocalizer["errStartAfterParent"]);

                if (req.Deadline > parentReq.Deadline)
                    ModelState.AddModelError("", _sharedLocalizer["errEndAfterParent"]);
            }

            if (ModelState.IsValid)
            {
                int id = req.Id;
                if (req.Id == 0)
                {
                    id = _db.AddRequest(req);
                }
                else
                {
                    _db.EditRequest(req);
                }
                TempData["message"] = string.Format("{0} {1} {2}", _sharedLocalizer["Request"], req.Name, _sharedLocalizer["Saved"]);
                return RedirectToAction("Index", id);
            }
            else
            {
                ViewBag.allRequests = _db.GetAll().Select(o => new RequestNav(o)).ToList();
                return View("Edit", req);
            }
        }

        [HttpPost]
        public IActionResult DeleteTask(int id)
        {
            if (_db.CanDelete(id))
            {
                var item = _db.Get(id);

                if (item != null) TempData["message"] = string.Format("{0} {1}", item.Name, _sharedLocalizer["Deleted"]);

                _db.Delete(id);

                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = _sharedLocalizer["CannotDel"];
                ViewBag.allRequests = _db.GetAll().Select(o => new RequestNav(o)).ToList();
                return View("Edit", _db.Get(id));
            }
        }
    }
}