using WTCPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace WTCPortal.Repository
{
    public class CustomerRepository : GenericRepository<IndividualCustomer>
    {
        public CustomerRepository(string connectionString)
            :base (connectionString)
        {
        }

        public IEnumerable<IndividualCustomer> GetAll()
        {
            using (var command = new SqlCommand("Sales.GetOnlineCustomerList"))
            {
                return ExecuteQuery(command);
            }
        }

        public IndividualCustomer GetById(int BusinessEntityID)
        {
            using (var command = new SqlCommand("Sales.GetOnlineCustomer"))
            {
                command.Parameters.Add(new SqlParameter("@BusinessEntityID", BusinessEntityID));
                return ExecuteSingleQuery(command);
            }
        }

        public void InsertCustomer(IndividualCustomer person)
        {
            var result = 0;
            using (SqlCommand command = new SqlCommand("Sales.InsertCustomer"))
            {
                command.Parameters.AddRange(
                    new[]
                    {
                        new SqlParameter("@PersonType", person.PersonType),
                        new SqlParameter("@NameStyle", person.NameStyle),
                        new SqlParameter("@Title", person.Title),
                        new SqlParameter("@FirstName", person.FirstName),
                        new SqlParameter("@MiddleName", person.MiddleName),
                        new SqlParameter("@LastName", person.LastName),
                        new SqlParameter("@Suffix", person.Suffix),
                        new SqlParameter("@EmailPromotion", person.EmailPromotion),
                        new SqlParameter("@EmailAddress", person.EmailAddress.EmailAddress1),
                        new SqlParameter("@PhoneNumber", person.PersonPhone.PhoneNumber),
                        new SqlParameter("@PhoneNumberTypeID", person.PersonPhone.PhoneNumberTypeID),

                        new SqlParameter("@CardType", person.PersonCreditCard.CreditCard.CardType),
                        new SqlParameter("@CardNumber", person.PersonCreditCard.CreditCard.CardNumber),
                        new SqlParameter("@ExpMonth", person.PersonCreditCard.CreditCard.ExpMonth),
                        new SqlParameter("@ExpYear", person.PersonCreditCard.CreditCard.ExpYear),

                        new SqlParameter("@AddressTypeID", person.BusinessEntityAddress.AddressTypeID),
                        new SqlParameter("@AddressLine1", person.BusinessEntityAddress.Address.AddressLine1),
                        new SqlParameter("@AddressLine2", person.BusinessEntityAddress.Address.AddressLine2),
                        new SqlParameter("@City", person.BusinessEntityAddress.Address.City),
                        new SqlParameter("@StateProvinceID", person.BusinessEntityAddress.Address.StateProvinceID),
                        new SqlParameter("@PostalCode", person.BusinessEntityAddress.Address.PostalCode),

                    });
                result = ExecuteCommand(command);
                if ((result == -99) || (result == -1000))
                    throw new Exception("Something is wrong!");
            }
        }

        public void UpdateCustomer(IndividualCustomer person)
        {
            var result = 0;
            using (SqlCommand command = new SqlCommand("Sales.InsertCustomer"))
            {
                command.Parameters.AddRange(
                    new[]
                    {
                        new SqlParameter("@BusinessEntityID", person.BusinessEntityID),
                        new SqlParameter("@FirstName", person.FirstName),
                        new SqlParameter("@MiddleName", person.MiddleName),
                        new SqlParameter("@LastName", person.LastName),
                        new SqlParameter("@EmailPromotion", person.EmailPromotion),
                        new SqlParameter("@EmailAddress", person.EmailAddress.EmailAddress1),
                        new SqlParameter("@PhoneNumber", person.PersonPhone.PhoneNumber),
                        new SqlParameter("@PhoneNumberTypeID", person.PersonPhone.PhoneNumberTypeID),

                        new SqlParameter("@CardType", person.PersonCreditCard.CreditCard.CardType),
                        new SqlParameter("@CardNumber", person.PersonCreditCard.CreditCard.CardNumber),
                        new SqlParameter("@ExpMonth", person.PersonCreditCard.CreditCard.ExpMonth),
                        new SqlParameter("@ExpYear", person.PersonCreditCard.CreditCard.ExpYear),

                        new SqlParameter("@AddressTypeID", person.BusinessEntityAddress.AddressTypeID),
                        new SqlParameter("@AddressLine1", person.BusinessEntityAddress.Address.AddressLine1),
                        new SqlParameter("@AddressLine2", person.BusinessEntityAddress.Address.AddressLine2),
                        new SqlParameter("@City", person.BusinessEntityAddress.Address.City),
                        new SqlParameter("@StateProvinceID", person.BusinessEntityAddress.Address.StateProvinceID),
                        new SqlParameter("@PostalCode", person.BusinessEntityAddress.Address.PostalCode),

                    });
                result = ExecuteCommand(command);
                if ((result == -99) || (result == -1000))
                    throw new Exception("Something is wrong!");
            }
        }

        public override IndividualCustomer PopulateRecord(SqlDataReader reader)
        {

            AddressType addressType = new AddressType
            {
                Name = reader["AddressType"].ToString()
            };

            CountryRegion country = new CountryRegion
            {
                Name = reader["CountryRegionName"].ToString()
            };

            StateProvince state = new StateProvince
            {
                Name = reader["StateProvinceName"].ToString(),
                CountryRegion = country
            };

            Address _address = new Address
            {
                AddressLine1 = reader["AddressLine1"].ToString(),
                AddressLine2 = reader["AddressLine2"].ToString(),
                City = reader["City"].ToString(),
                PostalCode = reader["PostalCode"].ToString(),
                StateProvince = state
            };
            BusinessEntityAddress entityAddress = new BusinessEntityAddress
            {
                Address = _address,
                AddressType = addressType
            };

            EmailAddress emailAddress = new EmailAddress
            {
                EmailAddress1 = (string)reader["EmailAddress"]
            };
            PhoneNumberType numberType = new PhoneNumberType
            {
                Name = reader["PhoneType"].ToString()
            };

            PersonPhone phone = new PersonPhone
            {
                PhoneNumber = (string)reader["PhoneNumber"],
                PhoneNumberType = numberType
            };


            CreditCard credit = new CreditCard
            {
                CreditCardID = Convert.ToInt32((int)reader["CreditCardID"]),
                CardType = reader["CardType"].ToString(),
                CardNumber = reader["CardNumber"].ToString(),
                ExpMonth = Convert.ToByte((byte)reader["ExpMonth"]),
                ExpYear = Convert.ToInt16((short)reader["ExpYear"]),
            };

            PersonCreditCard personCreditCard = new PersonCreditCard
            {
                CreditCardID = Convert.ToInt32((int)reader["CreditCardID"]),
                CreditCard = credit
            };

            Customer customer = new Customer
            {
                AccountNumber = reader["AccountNumber"].ToString()
            };
            return new IndividualCustomer
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
                PersonPhone = phone,
                BusinessEntityAddress = entityAddress,
                Customer = customer,
                PersonCreditCard = personCreditCard
            };
        }
    }
}