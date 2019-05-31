using System;
using System.Collections.Generic;
using WTCPortal.Controllers;
using WTCPortal.Models;
using WTCPortal.Repository;


namespace WTCPortal.ViewModel
{
    public class CustomerPageViewModel
    {
        private const string ConnectionString = "AdventureWorks";
        CustomerController _controller = new CustomerController();
       IndividualCustomer _person = new IndividualCustomer();

        public CustomerPageViewModel()
        {

        }

        public CustomerPageViewModel(CustomerController controller)
        {
            _controller = controller;
        }

        public IEnumerable<IndividualCustomer> Build()
        {
            var repo = new CustomerRepository(ConnectionString);
            var customers = repo.GetAll();
            return customers;
        }

        public IndividualCustomer SingleCustomer(int BusinessEntityID)
        {
            var repo = new CustomerRepository(ConnectionString);
            _person = repo.GetById(BusinessEntityID);
            return _person;
        }

        public void InsertCustomer(IndividualCustomer person)
        {
            try
            {
                CustomerRepository repo = new CustomerRepository(ConnectionString);
                repo.InsertCustomer(person);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateCustomer(IndividualCustomer person)
        {
            try
            {
                CustomerRepository repo = new CustomerRepository(ConnectionString);
                repo.UpdateCustomer(person);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}