using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbServices
{
    public interface IAuditService
    {
        bool Insert(Audit audit);
        List<Audit> GetAll();
        List<Audit> GetByUser(string userId);

    }
}
