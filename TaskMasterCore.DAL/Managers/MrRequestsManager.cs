using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TaskMasterCore.DAL.Models;
using TaskMasterCore.DAL.Models.DbModels;

namespace TaskMasterCore.DAL.Managers
{
    public class MrRequestsManager : Abstracts.DataManagerBase, Interfaces.IRequestsManager
    {
        private readonly string connectionString;

        public MrRequestsManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Request Get(int id)
        {
            using (var db = GetConnect(connectionString))
            {
                //var res = db.Requests.Where(o => o.Id == id).SingleOrDefault();
                var req = db.Requests.Where(o => o.Id == id).Select(GetRequest).SingleOrDefault();

                if (req != null)
                {
                    var parent = db.Requests.Where(o => o.Id == req.ParentId).SingleOrDefault();
                    if (parent != null) req.ParentName = parent.Name;

                    req.ChildsNames = db.Requests.Where(o => o.ParentId == req.Id).Select(o => o.Name).ToList();
                }
               
                return req;
            }
        }

        public IList<Request> GetAll()
        {
            using (var db = GetConnect(connectionString))
            {
                return db.Requests.Select(GetRequest).ToList();
            }
        }

        public IList<Request> GetChilds(int parentId)
        {
            using (var db = GetConnect(connectionString))
            {
                return db.Requests.Where(o => o.ParentId == parentId).Select(GetRequest).ToList();
            }
        }

        public int AddRequest(Request request)
        {
            using (var db = GetConnect(connectionString))
            {
                request.CreateDate = DateTime.Now;
                request.Status = Models.Enums.Statuses.Assigned;
                var newRequest = GetDbRequest(request);
                db.Requests.Add(newRequest);
                db.SaveChanges();
                return newRequest.Id;
            }
        }

        public void EditRequest(Request request)
        {
            using (var db = GetConnect(connectionString))
            {
                var editRequest = db.Requests.SingleOrDefault(o => o.Id == request.Id);

                if (editRequest != null)
                {
                    editRequest.Name = request.Name;
                    editRequest.Comment = request.Comment;
                    editRequest.Performers = request.Performers;
                    editRequest.Deadline = request.Deadline;
                    editRequest.StartDate = request.StartDate;

                    //Если закрываем незакрытую
                    if (request.Status == Models.Enums.Statuses.Completed && editRequest.Status != (int)Models.Enums.Statuses.Completed)
                    {
                        if (CanComplete(db, editRequest))
                        {
                            CompleteRequest(db, editRequest);
                        }
                    }
                    else
                    {
                        editRequest.Status = (int)request.Status;
                    }

                    db.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var db = GetConnect(connectionString))
            {
                var delRequest = db.Requests.Single(o => o.Id == id);

                if (!db.Requests.Where(o => o.ParentId == id).Any())
                {
                    db.Requests.Remove(delRequest);
                    db.SaveChanges();
                }
            }
        }

        public bool CanDelete(int id)
        {
            using (var db = GetConnect(connectionString))
            {
                var delRequest = db.Requests.Single(o => o.Id == id);

                if (delRequest == null) return true;
                return !db.Requests.Where(o => o.ParentId == id).Any();
            }
        }

        public bool CanComplete(int id)
        {
            using (var db = GetConnect(connectionString))
            {
                var req = db.Requests.SingleOrDefault(o => o.Id == id);

                if (req != null)
                {
                    return CanComplete(db, req);
                }
                else
                {
                    return false;
                }
            }
        }

        private void CompleteRequest(DbContext db, DbRequest req)
        {
            if (req.Status != (int)Models.Enums.Statuses.Completed)
            {
                req.EndDate = DateTime.Now;
                req.Status = (int)Models.Enums.Statuses.Completed;
            }            
            var childs = db.Requests.Where(o => o.ParentId == req.Id);
            foreach (var childReq in childs)
            {
                CompleteRequest(db, childReq);
            }
        }

        /// <summary>
        /// Проверка на возможность закрытия задачи
        /// </summary>
        /// <param name="db">Соединение с базой</param>
        /// <param name="req">Проверяемая задача</param>
        /// <returns></returns>
        private bool CanComplete(DbContext db, DbRequest req)
        {
            var checkedIds = GetChildsIds(db, req);
            //checkedIds.Add(req.Id);

            //Если в подзадачах есть хоть одна задача со статусом назначена, закрывать нельзя
            return !db.Requests.Where(o => checkedIds.Contains(o.Id)).Any(o => o.Status == (int)Models.Enums.Statuses.Assigned);
           
            //return !(req.InverseParent.Any(o => o.Status == (int)Models.Enums.Statuses.Assigned)
                //&& req.InverseParent.Select(o => CanComplete(db, o.Id)).Any(o => o == false));
                //|| db.Requests.Where(o => checkedIds.Contains(o.Id)).Any(o => o.Status == (int)Models.Enums.Statuses.Assigned));
        }

        /// <summary>
        /// Получить id всех потомков 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private IList<int> GetChildsIds(DbContext db, DbRequest req)
        {
            var childs = db.Requests.Where(o => o.ParentId == req.Id);
            var result = childs.Select(o => o.Id).ToList();

            foreach (var childReq in childs)
            {
                result.AddRange(GetChildsIds(db, childReq));
            }

            return result;
        }

        private Request GetRequest (DbRequest obj)
        {
            return new Request
            {
                Id = obj.Id,
                ParentId = obj.ParentId,
                Name = obj.Name,
                Comment = obj.Comment,
                Performers = obj.Performers,
                CreateDate = obj.CreateDate,
                StartDate = obj.StartDate,
                EndDate = obj.EndDate,
                Deadline = obj.Deadline,
                Status = (Models.Enums.Statuses)obj.Status,
                //ParentName = obj.Parent != null ? obj.Parent.Name : null,
                //ChildsNames = obj.InverseParent.Select(o => o.Name).ToList(),
            };
        }

        private DbRequest GetDbRequest (Request obj)
        {
            return new DbRequest
            {
                Id = obj.Id,
                ParentId = obj.ParentId,
                Name = obj.Name,
                Comment = obj.Comment,
                Performers = obj.Performers,
                CreateDate = obj.CreateDate,
                StartDate = obj.StartDate,
                EndDate = obj.EndDate,
                Deadline = obj.Deadline,
                Status = (int)obj.Status,         
            };
        }
    }
}
