using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WTCPortal.Models;
using WTCPortal.Repository;

namespace WTCPortal.Repository
{
    public class UnitOfWork : IDisposable
    {
        private AdventureWorks _adventure = new AdventureWorks();
        
        public void Save()
        {
            _adventure.SaveChanges();
        }

        public bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _adventure.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}