using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WTCPortal.Controllers;
using WTCPortal.Models;
using WTCPortal.Repository;

namespace WTCPortal.ViewModel
{
    public class PersonPageViewModel 
    {
        private const string ConnectionString = "AdventureWorks";
        PersonController _controller = new PersonController();
        Person _person = new Person();

        public PersonPageViewModel()
        {

        }

        public PersonPageViewModel(PersonController controller)
        {
            _controller = controller;
        }
        
        public IEnumerable<Person> Build()
        {
            var repository = new PersonRespository(ConnectionString);
            var people = repository.GetAll();
            return people;
        }
        
        public Person SinglePerson(int BusinessEntityID)
        {
            var repo = new PersonRespository(ConnectionString);
            _person = repo.GetById(BusinessEntityID);
            return _person;
        }

        public void  InsertPerson(Person p)
        {
            try
            {
                PersonRespository repo = new PersonRespository(ConnectionString);
                repo.CreatePerson(p);
            }
            catch (Exception e)
            {
                throw e;
            }
          
        }

        public void UpdatePerson(Person person)
        {
            try
            {
                PersonRespository repo = new PersonRespository(ConnectionString);
                repo.EditPerson(person);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
    }
}