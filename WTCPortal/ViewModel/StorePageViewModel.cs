using System;
using System.Collections.Generic;
using WTCPortal.Controllers;
using WTCPortal.Models;
using WTCPortal.Repository;

namespace WTCPortal.ViewModel
{
    public class StorePageViewModel
    {
        private const string ConnectionString = "AdventureWorks";
        StoreController _controller = new StoreController();
        Store _store = new Store();

        public StorePageViewModel()
        {

        }

        public StorePageViewModel(StoreController controller)
        {
            _controller = controller;
        }

        public IEnumerable<Store> Build()
        {
            var repo = new StoreRepository(ConnectionString);
            var stores = repo.GetStores();
            return stores;
        }

        public Store SingleStore(int BusinessEntityID)
        {
            var repo = new StoreRepository(ConnectionString);
            _store = repo.GetStore(BusinessEntityID);
            return _store;
        }

        public void InsertStore(Store store)
        {
            try
            {
                StoreRepository repo = new StoreRepository(ConnectionString);
                repo.InsertStore(store);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateStore(Store store)
        {
            try
            {
                StoreRepository repo = new StoreRepository(ConnectionString);
                repo.UpdateStore(store);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}