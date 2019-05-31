using System;
using System.Collections.Generic;
using WTCPortal.Controllers;
using WTCPortal.Models;
using WTCPortal.Repository;


namespace WTCPortal.ViewModel
{
    public class VendorPageViewModel
    {
        private const string ConnectionString = "AdventureWorks";
        VendorController _controller = new VendorController();
        Vendor _vendor = new Vendor();

        public VendorPageViewModel()
        {

        }

        public VendorPageViewModel(VendorController controller)
        {
            _controller = controller;
        }

        public IEnumerable<Vendor> Build()
        {
            var repo = new VendorRepository(ConnectionString);
            var vendors = repo.GetVendors();
            return vendors;
        }

        public Vendor SingleVendor(int BusinessEntityID)
        {
            var repo = new VendorRepository(ConnectionString);
            _vendor = repo.GetVendor(BusinessEntityID);
            return _vendor;
        }

        public void InsertVendor(Vendor vendor)
        {
            try
            {
                VendorRepository repo = new VendorRepository(ConnectionString);
                repo.InsertVendor(vendor);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateVendor(Vendor vendor)
        {
            try
            {
                VendorRepository repo = new VendorRepository(ConnectionString);
                repo.UpdateVendor(vendor);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}