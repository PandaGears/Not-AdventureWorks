using WTCPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace WTCPortal.Repository
{
    public class PersonRespository : GenericRepository<Person>
    {
       
        public PersonRespository(string connectionString)
           : base(connectionString)
        {
        }

        public IEnumerable<Person> GetAll()
        {
            using (var command = new SqlCommand("Person.PeopleInformation"))
            {
                return ExecuteQuery(command);
            }
        }

        public Person GetById(int BusinessEntityID)
        {

            // PARAMETERIZED QUERIES!
            using (var command = new SqlCommand("Person.PersonInformation"))
            {
                command.Parameters.Add(new SqlParameter("BusinessEntityID", BusinessEntityID));
                return ExecuteSingleQuery(command);
            }
        }

        public void CreatePerson(Person p)
        {
            var result  = 0;
            using (SqlCommand command = new SqlCommand("Person.InsertPerson"))
            {

                command.Parameters.Add(new SqlParameter("@PersonType", p.PersonType));
                command.Parameters.Add(new SqlParameter("@NameStyle", p.NameStyle));
                command.Parameters.Add(new SqlParameter("@Title", p.Title));
                command.Parameters.Add(new SqlParameter("@FirstName", p.FirstName));
                command.Parameters.Add(new SqlParameter("@MiddleName", p.MiddleName));
                command.Parameters.Add(new SqlParameter("@LastName", p.LastName));
                command.Parameters.Add(new SqlParameter("@Suffix",p.Suffix));
                command.Parameters.Add(new SqlParameter("@EmailPromotion", p.EmailPromotion));
                command.Parameters.Add(new SqlParameter("@EmailAddress", p.EmailAddress.EmailAddress1));
                command.Parameters.Add(new SqlParameter("@PhoneNumber", p.PersonPhone.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@PhoneNumberTypeID", p.PersonPhone.PhoneNumberTypeID));
                result = ExecuteCommand(command);
            }
        }

        public void EditPerson(Person person)
        {
            var result = 0;
            using (SqlCommand command = new SqlCommand("Person.EditPerson"))
            {
                command.Parameters.Add(new SqlParameter("@BusinessEntityID", person.BusinessEntityID));
                command.Parameters.Add(new SqlParameter("@PersonType", person.PersonType));
                command.Parameters.Add(new SqlParameter("@NameStyle", person.NameStyle));
                command.Parameters.Add(new SqlParameter("@Title", person.Title));
                command.Parameters.Add(new SqlParameter("@FirstName", person.FirstName));
                command.Parameters.Add(new SqlParameter("@MiddleName", person.MiddleName));
                command.Parameters.Add(new SqlParameter("@LastName", person.LastName));
                command.Parameters.Add(new SqlParameter("@Suffix", person.Suffix));
                command.Parameters.Add(new SqlParameter("@EmailPromotion", person.EmailPromotion));
                command.Parameters.Add(new SqlParameter("@EmailAddress", person.EmailAddress.EmailAddress1));
                command.Parameters.Add(new SqlParameter("@PhoneNumber", person.PersonPhone.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@PhoneNumberTypeID", person.PersonPhone.PhoneNumberTypeID));
                result = ExecuteCommand(command);
            }
        }


        public override Person PopulateRecord(SqlDataReader reader)
        {
            EmailAddress emailAddress = new EmailAddress
            {
                EmailAddress1 = (string)reader["EmailAddress"]
            };
            
            PersonPhone phone = new PersonPhone
            {
                PhoneNumber = (string)reader["PhoneNumber"],
            };
          
            return new Person
            {
                BusinessEntityID = Convert.ToInt32((int)reader["BusinessEntityID"]),
                FirstName = reader["FirstName"].ToString(),
                MiddleName = reader["MiddleName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Suffix = reader["Suffix"].ToString(),
                PersonType = reader["PersonType"].ToString(),
                NameStyle = (bool)reader["NameStyle"],
                Title = reader["Title"].ToString(),
                EmailPromotion = Convert.ToInt32((int)reader["EmailPromotion"]),
                ModifiedDate = (DateTime)reader["ModifiedDate"],
                rowguid = (Guid)reader["rowguid"],
                EmailAddress = emailAddress,
                PersonPhone = phone
            };
        }
    }
}