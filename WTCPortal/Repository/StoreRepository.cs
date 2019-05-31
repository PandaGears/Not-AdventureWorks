using System;
using System.Collections.Generic;
using WTCPortal.Models;
using System.Data;
using System.Data.SqlClient;
using WTCPortal.ExtensionMethods;


namespace WTCPortal.Repository
{
    public class StoreRepository : GenericRepository<Store>
    {
        public StoreRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<Store> GetStores()
        {
            using (var command = new SqlCommand("Sales.GetCustomerStores"))
            {
                return ExecuteQuery(command);
            }
        }

        public Store GetStore(int BusinessEntityID)
        {
            using (var command = new SqlCommand("Sales.GetSingleStore"))
            {
                command.Parameters.Add(new SqlParameter("@BusinessEntityID", BusinessEntityID));
                return ExecuteSingleQuery(command);
            }
        }

        public void InsertStore(Store store)
        {
            var result = 0;
            using (SqlCommand command = new SqlCommand("Sales.InsertStore"))
            {
                command.Parameters.AddRange(
                    new[]
                    {
                        new SqlParameter("@Name", store.Name),
                        new SqlParameter("@AddressTypeID", store.BusinessEntityAddress.AddressTypeID),
                        new SqlParameter("@AddressLine1", store.BusinessEntityAddress.Address.AddressLine1),
                        new SqlParameter("@AddressLine2", store.BusinessEntityAddress.Address.AddressLine2),
                        new SqlParameter("@City", store.BusinessEntityAddress.Address.City),
                        new SqlParameter("@StateProvinceID", store.BusinessEntityAddress.Address.StateProvinceID),
                        new SqlParameter("@PostalCode", store.BusinessEntityAddress.Address.PostalCode),

                    });
                result = ExecuteCommand(command);
                if ((result == -99) || (result == -1000))
                    throw new Exception("Something is wrong!");
            }
        }

        public void UpdateStore(Store store)
        {
            var result = 0;
            using (SqlCommand command = new SqlCommand("Sales.UpdateStore"))
            {
                command.Parameters.AddRange(
                    new[]
                    {
                        new SqlParameter("@BusinessEntityID", store.BusinessEntityID),
                        new SqlParameter("@Name", store.Name),
                        new SqlParameter("@AddressTypeID", store.BusinessEntityAddress.AddressTypeID),
                        new SqlParameter("@AddressLine1", store.BusinessEntityAddress.Address.AddressLine1),
                        new SqlParameter("@AddressLine2", store.BusinessEntityAddress.Address.AddressLine2),
                        new SqlParameter("@City", store.BusinessEntityAddress.Address.City),
                        new SqlParameter("@StateProvinceID", store.BusinessEntityAddress.Address.StateProvinceID),
                        new SqlParameter("@PostalCode", store.BusinessEntityAddress.Address.PostalCode),

                    });
                result = ExecuteCommand(command);
                if ((result == -99) || (result == -1000))
                    throw new Exception("Something is wrong!");
            }

        }

        public override Store PopulateRecord(SqlDataReader reader)
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
                Name = reader["PhoneType"].ToString(),
            };

            PersonPhone phone = new PersonPhone
            {
                PhoneNumber = (string)reader["PhoneNumber"],
                PhoneNumberTypeID = Convert.ToInt32((int)reader["PhoneNumberTypeID"]),
                PhoneNumberType = numberType
            };

            SalesPerson salesPerson = new SalesPerson
            {
                BusinessEntityID = Convert.ToInt32((int)reader["SPBusinessEntityID"]),
                FirstName = reader["FirstName"].ToString(),
                MiddleName = reader["MiddleName"].ToString(),
                LastName = reader["LastName"].ToString(),
                EmailAddress = emailAddress,
                PersonPhone = phone
            };

            Customer customer = new Customer
            {
                AccountNumber = reader["AccountNumber"].ToString()
            };

            StoreDemographics demographics = new StoreDemographics
            {
                ID = Convert.ToInt32((int)reader["BusinessEntityID"]),
                AnnualRevenue = Convert.ToDecimal((decimal)reader["AnnualRevenue"]),
                AnnualSales  = Convert.ToDecimal((decimal)reader["AnnualSales"]),
                BankName = reader["BankName"].ToString(),
                BusinessType  = reader["BusinessType"].ToString(),
                YearOpened = Convert.ToInt32((int)reader["YearOpened"]),
                Speciality = reader["Specialty"].ToString(),
                SquareFeet = Convert.ToInt32((int)reader["SquareFeet"]),
                Brands = reader["Brands"].ToString(),
                Internet = reader["Internet"].ToString(),
                NumberEmployees  = Convert.ToInt32((int)reader["NumberEmployees"])
            };

            return new Store
            {
                BusinessEntityID = Convert.ToInt32((int)reader["BusinessEntityID"]),
                Name = reader["Name"].ToString(),
                SalesPersonID = Convert.ToInt32((int)reader["SalesPersonID"]),
                Demographics = reader["Demographics"].ToString(),
                rowguid = (Guid)reader["rowguid"],
                ModifiedDate = (DateTime)reader["ModifiedDate"],
                SalesPerson = salesPerson,
                StoreReps = GetStorerReps(Convert.ToInt32((int)reader["BusinessEntityID"])),
                Customer = customer,
                BusinessEntityAddress = entityAddress,
                StoreDemographics = demographics
            };
        }

        ////////////////////////StoreContacts////////////////////////////////////////////////

        public IEnumerable<StoreContacts> GetStorerReps(int BusinessEntityID)
        {
            var list = new List<StoreContacts>();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Sales.StoreContacts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BusinessEntityID", BusinessEntityID);
                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        try
                        {
                            while (reader.Read())
                            {
                                var record = Populate(reader);
                                if (record != null)
                                {
                                    list.Add(record);
                                }
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

            }
            return list;
        }

        public StoreContacts Populate(SqlDataReader reader)
        {
            ContactType contactType = new ContactType
            {
                Name = reader["ContactType"].ToString(),
            };

            BusinessEntityContact businessEntityContact = new BusinessEntityContact
            {
                PersonID = Convert.ToInt32((int)reader["BusinessEntityID"]),
                ContactTypeID = Convert.ToInt32((int)reader["BusinessEntityID"]),
                ContactType = contactType
            };

            EmailAddress emailAddress = new EmailAddress
            {
                EmailAddress1 = (string)reader["EmailAddress"]
            };

            PhoneNumberType numberType = new PhoneNumberType
            {
                Name = reader["PhoneType"].ToString(),
            };

            PersonPhone phone = new PersonPhone
            {
                PhoneNumber = (string)reader["PhoneNumber"],
                PhoneNumberTypeID = Convert.ToInt32((int)reader["PhoneNumberTypeID"]),
                PhoneNumberType = numberType
            };

            return new StoreContacts
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
                BusinessEntityContact = businessEntityContact
            };

        }

        ////////////////////////////////////////////Something to read XML demographics in sales.store////////////////////////////////////
        
      
    }
}