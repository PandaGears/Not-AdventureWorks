using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WTCPortal.Models;
using System.Data.SqlClient;

namespace WTCPortal.Repository
{
    public class ProductRepo : AdoRepository<Product>
    {
        public ProductRepo(string connectionString) : base(connectionString)
        {

        }

        public IEnumerable<Product> GetAll()
        {
            using (var command = new SqlCommand("HumanResources.SelectProductCategory"))
            {

                return ExecuteStoredProc(command);
            }
        }

        public IEnumerable<Product> GetAllStuff()
        {
            using (var command = new SqlCommand("SELECT ProductID, Name, ProductNumber, MakeFlag, FinishedGoodsFlag, ReorderPoint, StandardCost, ListPrice FROM Production.Product"))
            {
                return GetRecords(command);
            }
        }

        public Product GetById(int ProductID)
        {
            using (var command = new SqlCommand("Production.usp_ProductCategory"))
            {
                command.Parameters.AddWithValue("@ProductID", ProductID);

                return ExcStoreProcs(command);
            }
        }

        public void DeleEmp(int BusinessEntityID)
        {
            using (var command = new SqlCommand("HumanResources.DeleteEmployeeInfo"))
            {
                command.Parameters.AddWithValue("@BusinessEntityID", BusinessEntityID);

                ExcStoreProcss(command);
            }
        }

        public override Product PopulateRecord(SqlDataReader reader)
        {
            int result = 0;

            int.TryParse("One", out result);

            return new Product
            {
                ProductID = (int)reader["ProductID"],
                Name5 = (string)reader["ProductName"],
                ProductNumber = (string)reader["ProductNumber"],
                MakeFlag = (bool)reader["MakeFlag"],
                FinishedGoodsFlag = (bool)reader["FinishedGoodsFlag"],
                Color = Convert.ToString(reader["Color"]),
                SafetyStockLevel = (short)reader["SafetyStockLevel"],
                ReorderPoint = (short)reader["ReorderPoint"],
                StandardCost = (decimal)reader["StandardCost"],
                ListPrice = (decimal)reader["ListPrice"],
                Name1 = Convert.ToString(reader["CategoryName"]),
                LargePhoto = (byte[])reader["LargePhoto"],
                Class = Convert.ToString(reader["Class"]),
                Size = Convert.ToString(reader["Size"]),
                Name3 = (string)reader["VendorName"],
                City = (string)reader["City"],
                StateProvinceName = (string)reader["StateProvinceName"]
            };

        }
    }
}