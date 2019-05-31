using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WTCPortal.Controllers;
using WTCPortal.Models;
using WTCPortal.Repository;

namespace WTCPortal.ViewModel
{
    public class EmployeePageViewModel
    {
        private const string ConnectionString = "AdventureWorks";
        EmployeeController _controller = new EmployeeController();
        Employee _employee =  new Employee();

        public EmployeePageViewModel()
        {

        }

        public EmployeePageViewModel(EmployeeController controller)
        {
            _controller = controller;
        }

        public IEnumerable<Employee> Build()
        {
            EmployeeRepository repo = new EmployeeRepository(ConnectionString);
            var employees = repo.GetAll();
            return employees;
        }

        public Employee SingleEmployee(int BusinessEntityID)
        {
            EmployeeRepository repo = new EmployeeRepository(ConnectionString);
            _employee = repo.GetById(BusinessEntityID);
            return _employee;
        }

        public void InsertEmployee(Employee employee)
        {
            try
            {
                EmployeeRepository repo = new EmployeeRepository(ConnectionString);
                repo.InsertEmployee(employee);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            try
            {
                EmployeeRepository repo = new EmployeeRepository(ConnectionString);
                repo.UpdateEmployee(employee);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}