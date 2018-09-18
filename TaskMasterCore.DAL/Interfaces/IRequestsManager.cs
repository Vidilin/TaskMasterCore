using System;
using System.Collections.Generic;
using System.Text;
using TaskMasterCore.DAL.Models;

namespace TaskMasterCore.DAL.Interfaces
{
    public interface IRequestsManager
    {
        Request Get(int id);
        IList<Request> GetAll();
        IList<Request> GetChilds(int parentId);
        int AddRequest(Request request);
        void EditRequest(Request request);
        void Delete(int id);
        bool CanComplete(int id);
        bool CanDelete(int id);
    }
}
