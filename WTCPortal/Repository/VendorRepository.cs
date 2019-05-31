using System;
using System.Collections.Generic;
using WTCPortal.Models;
using System.Data;
using System.Data.SqlClient;

namespace WTCPortal.Repository
{
    public class VendorRepository : GenericRepository<Vendor>
    {
        public VendorRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<Vendor> GetVendors()
        {
            using (var command = new SqlCommand("Purchasing.GetVendors"))
            {
                return ExecuteQuery(command);
            }
        }

        public Vendor GetVendor(int BusinessEntityID)
        {
            using (var command = new SqlCommand("Purchasing.VendorInformation"))
            {
                command.Parameters.Add(new SqlParameter("@BusinessEntityID", BusinessEntityID));
                return ExecuteSingleQuery(command);
            }
        }

        public void InsertVendor(Vendor vendor)
        {
            var result = 0;
            using (SqlCommand command = new SqlCommand("Purchasing.InsertVendor"))
            {
                command.Parameters.AddRange(
                    new[]
                    {
                        new SqlParameter("@AccountNumber", vendor.AccountNumber),
                        new SqlParameter("@Name", vendor.Name),
                        new SqlParameter("@CreditRating", vendor.CreditRating),
                        new SqlParameter("@PreferredVendorStatus", vendor.PreferredVendorStatus),
                        new SqlParameter("@ActiveFlag", vendor.ActiveFlag),
                        new SqlParameter("@PurchasingWebServiceURL", vendor.PurchasingWebServiceURL),
                        new SqlParameter("@AddressTypeID", vendor.BusinessEntityAddress.AddressTypeID),
                        new SqlParameter("@AddressLine1", vendor.BusinessEntityAddress.Address.AddressLine1),
                        new SqlParameter("@AddressLine2", vendor.BusinessEntityAddress.Address.AddressLine2),
                        new SqlParameter("@City", vendor.BusinessEntityAddress.Address.City),
                        new SqlParameter("@StateProvinceID", vendor.BusinessEntityAddress.Address.StateProvinceID),
                        new SqlParameter("@PostalCode", vendor.BusinessEntityAddress.Address.PostalCode),

                    });
                result = ExecuteCommand(command);
                if ((result == -99) || (result == -1000))
                    throw new Exception("Something is wrong!");
            }
        }

        public void UpdateVendor(Vendor vendor)
        {
            var result = 0;
            using (SqlCommand command = new SqlCommand("Purchasing.UpdateVendor"))
            {
                command.Parameters.AddRange(
                    new[]
                    {
                        new SqlParameter("@BusinessEntityID", vendor.BusinessEntityID),
                        new SqlParameter("@AccountNumber", vendor.AccountNumber),
                        new SqlParameter("@Name", vendor.Name),
                        new SqlParameter("@CreditRating", vendor.CreditRating),
                        new SqlParameter("@PreferredVendorStatus", vendor.PreferredVendorStatus),
                        new SqlParameter("@ActiveFlag", vendor.ActiveFlag),
                        new SqlParameter("@PurchasingWebServiceURL", vendor.PurchasingWebServiceURL),
                        new SqlParameter("@AddressTypeID", vendor.BusinessEntityAddress.AddressTypeID),
                        new SqlParameter("@AddressLine1", vendor.BusinessEntityAddress.Address.AddressLine1),
                        new SqlParameter("@AddressLine2", vendor.BusinessEntityAddress.Address.AddressLine2),
                        new SqlParameter("@City", vendor.BusinessEntityAddress.Address.City),
                        new SqlParameter("@StateProvinceID", vendor.BusinessEntityAddress.Address.StateProvinceID),
                        new SqlParameter("@PostalCode", vendor.BusinessEntityAddress.Address.PostalCode),

                    });
                result = ExecuteCommand(command);
                if ((result == -99) || (result == -1000))
                    throw new Exception("Something is wrong!");
            }

        }
        
        public override Vendor PopulateRecord(SqlDataReader reader)
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

            return new Vendor
            {
                BusinessEntityID = Convert.ToInt32((int)reader["BusinessEntityID"]),
                Name = reader["Name"].ToString(),
                AccountNumber = reader["AccountNumber"].ToString(),
                CreditRating = (byte)reader["CreditRating"],
                PreferredVendorStatus = (bool)reader["PreferredVendorStatus"],
                ActiveFlag = (bool)reader["ActiveFlag"],
                PurchasingWebServiceURL = reader["PurchasingWebServiceURL"].ToString(),
                ModifiedDate =(DateTime)reader["ModifiedDate"],
                VendorReps = GetVendorReps(Convert.ToInt32((int)reader["BusinessEntityID"])),
                BusinessEntityAddress = entityAddress
            };
        }

        
        ////////////////////////////////////////////////////////////////////////

        //Vendor ReprisentativeInfo//
        public IEnumerable<VendorContacts> GetVendorReps(int BusinessEntityID)
        {
            var list = new List<VendorContacts>();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Purchasing.VendorContacts", conn))
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

        public VendorContacts Populate(SqlDataReader reader)
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

            return new VendorContacts
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
        
    }

}