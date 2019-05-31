using WTCPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace WTCPortal.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>
    {
        public EmployeeRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<Employee> GetAll()
        {
            using (var command = new SqlCommand("HumanResources.EmployeeList"))
            {
                return ExecuteQuery(command);
            }
        }

        public Employee GetById(int BusinessEntityID)
        {
            using (var command = new SqlCommand("HumanResources.EmployeeInformation"))
            {
                command.Parameters.Add(new SqlParameter("@BusinessEntityID", BusinessEntityID));
                return ExecuteSingleQuery(command);
            }
        }


        public void InsertEmployee(Employee employee)
        {
            var result = 0;
            using (SqlCommand command = new SqlCommand("HumanResources.InsertEmployee"))
            {
                command.Parameters.Add(new SqlParameter("@Title", employee.Title));
                command.Parameters.Add(new SqlParameter("@FirstName", employee.FirstName));
                command.Parameters.Add(new SqlParameter("@MiddleName", employee.MiddleName));
                command.Parameters.Add(new SqlParameter("@LastName", employee.LastName));
                command.Parameters.Add(new SqlParameter("@EmailPromotion", employee.EmailPromotion));
                command.Parameters.Add(new SqlParameter("@EmailAddress", employee.EmailAddress.EmailAddress1));
                command.Parameters.Add(new SqlParameter("@PhoneNumber", employee.PersonPhone.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@PhoneNumberTypeID", employee.PersonPhone.PhoneNumberTypeID));
                command.Parameters.Add(new SqlParameter("@NationalIDNumber", employee.NationalIDNumber));
                command.Parameters.Add(new SqlParameter("@JobTitle", employee.JobTitle));
                command.Parameters.Add(new SqlParameter("@Birthdate", employee.BirthDate.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new SqlParameter("@MaritalStatus", employee.MaritalStatus));
                command.Parameters.Add(new SqlParameter("@Gender", employee.Gender));
                command.Parameters.Add(new SqlParameter("@HireDate", employee.HireDate.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new SqlParameter("@DepartmentID", employee.EmployeeDepartmentHistory.DepartmentID));
                command.Parameters.Add(new SqlParameter("@ShiftID", employee.EmployeeDepartmentHistory.ShiftID));
                command.Parameters.Add(new SqlParameter("@AddressTypeID", employee.BusinessEntityAddress.AddressTypeID));
                command.Parameters.Add(new SqlParameter("@AddressLine1", employee.BusinessEntityAddress.Address.AddressLine1));
                command.Parameters.Add(new SqlParameter("@AddressLine2", employee.BusinessEntityAddress.Address.AddressLine2));
                command.Parameters.Add(new SqlParameter("@City", employee.BusinessEntityAddress.Address.City));
                command.Parameters.Add(new SqlParameter("@StateProvinceID", employee.BusinessEntityAddress.Address.StateProvinceID));
                command.Parameters.Add(new SqlParameter("@PostalCode", employee.BusinessEntityAddress.Address.PostalCode));

                result = ExecuteCommand(command);

                if ((result == -99) || (result == -1000))
                    throw new Exception("Something is wrong!");
            }
        }


        public void  UpdateEmployee(Employee employee)
        {
            var result = 0;
            using (SqlCommand command = new SqlCommand("HumanResources.UpdateEmployee"))
            {

                command.Parameters.Add(new SqlParameter("@BusinessEntityID", employee.BusinessEntityID));
                command.Parameters.Add(new SqlParameter("@PersonType", employee.PersonType));
                command.Parameters.Add(new SqlParameter("@NameStyle", employee.NameStyle));
                command.Parameters.Add(new SqlParameter("@Title", employee.Title));
                command.Parameters.Add(new SqlParameter("@FirstName", employee.FirstName));
                command.Parameters.Add(new SqlParameter("@MiddleName", employee.MiddleName));
                command.Parameters.Add(new SqlParameter("@LastName", employee.LastName));
                command.Parameters.Add(new SqlParameter("@Suffix", employee.Suffix));
                command.Parameters.Add(new SqlParameter("@EmailPromotion", employee.EmailPromotion));
                command.Parameters.Add(new SqlParameter("@EmailAddress", employee.EmailAddress.EmailAddress1));
                command.Parameters.Add(new SqlParameter("@PhoneNumber", employee.PersonPhone.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@PhoneNumberTypeID", employee.PersonPhone.PhoneNumberTypeID));
                command.Parameters.Add(new SqlParameter("@JobTitle", employee.JobTitle));
                command.Parameters.Add(new SqlParameter("@MaritalStatus", employee.MaritalStatus));
                command.Parameters.Add(new SqlParameter("@SalariedFlag", employee.SalariedFlag));
                command.Parameters.Add(new SqlParameter("@VacationHours", employee.VacationHours));
                command.Parameters.Add(new SqlParameter("@SickLeaveHours", employee.SickLeaveHours));
                command.Parameters.Add(new SqlParameter("@CurrentFLag", employee.CurrentFlag));
                command.Parameters.Add(new SqlParameter("@DepartmentID", employee.EmployeeDepartmentHistory.DepartmentID));
                command.Parameters.Add(new SqlParameter("@ShiftID", employee.EmployeeDepartmentHistory.ShiftID));
                command.Parameters.Add(new SqlParameter("@Rate", employee.EmployeePayHistory.Rate));
                command.Parameters.Add(new SqlParameter("@PayFrequency", employee.EmployeePayHistory.PayFrequency));
                command.Parameters.Add(new SqlParameter("@AddressTypeID", employee.BusinessEntityAddress.AddressTypeID));
                command.Parameters.Add(new SqlParameter("@AddressLine1", employee.BusinessEntityAddress.Address.AddressLine1));
                command.Parameters.Add(new SqlParameter("@AddressLine2", employee.BusinessEntityAddress.Address.AddressLine2));
                command.Parameters.Add(new SqlParameter("@City", employee.BusinessEntityAddress.Address.City));
                command.Parameters.Add(new SqlParameter("@StateProvinceID", employee.BusinessEntityAddress.Address.StateProvinceID));
                command.Parameters.Add(new SqlParameter("@PostalCode", employee.BusinessEntityAddress.Address.PostalCode));

                result = ExecuteCommand(command);
                if ((result == -99) || (result == -1000))
                    throw new Exception("Something is wrong!");
            }

        }
  
        public override Employee PopulateRecord(SqlDataReader reader)
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
                StateProvinceID = Convert.ToInt32((int)reader["StateProvinceID"]),
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
                PhoneNumberTypeID = Convert.ToInt32((int)reader["PhoneNumberTypeID"]),
                PhoneNumberType = numberType
            };

            Shift shift = new Shift
            {
                Name = reader["ShiftName"].ToString(),
                StartTime = (TimeSpan)reader["StartTime"],
                EndTime = (TimeSpan)reader["EndTime"]
            };

            Department department = new Department
            {
                Name = reader["Department"].ToString(),
                GroupName = reader["GroupName"].ToString()
            };

            EmployeeDepartmentHistory employeeDepartment = new EmployeeDepartmentHistory
            {
                ShiftID = Convert.ToByte((byte)reader["ShiftID"]),
                DepartmentID = Convert.ToInt16((short)reader["DepartmentID"]),
                Department = department,
                Shift = shift
            };

            EmployeePayHistory employeePay = new EmployeePayHistory
            {
                Rate = Convert.ToDecimal((decimal)reader["Rate"]),
                PayFrequency = Convert.ToByte((byte)reader["PayFrequency"])
            };

            return new Employee
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

                NationalIDNumber = reader["NationalIDNumber"].ToString(),
                LoginID = reader["LoginID"].ToString(),
                JobTitle = reader["JobTitle"].ToString(),
                BirthDate =(DateTime)reader["BirthDate"],
                MaritalStatus = reader["MaritalStatus"].ToString(),
                Gender = reader["Gender"].ToString(),
                HireDate = (DateTime)reader["HireDate"],
                SalariedFlag = (bool)reader["SalariedFlag"],
                VacationHours = Convert.ToInt16((short)reader["VacationHours"]),
                SickLeaveHours = Convert.ToInt16((short)reader["SickLeaveHours"]),
                CurrentFlag = (bool)reader["CurrentFlag"],
                
                EmailAddress = emailAddress,
                PersonPhone = phone,
                BusinessEntityAddress = entityAddress,
                EmployeeDepartmentHistory = employeeDepartment,
                EmployeePayHistory = employeePay
                
                
            };
        }
    }
}