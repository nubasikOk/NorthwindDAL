using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindDAL.Entities;
using NorthwindDAL.Entities.Enums;
using NorthwindDAL.Repositories;

namespace NorthwindDAL.Tests
{
    [TestClass]
    public class Tests
    {
        const string ConnectionString =
            @"data source=EPBYGROW0110;initial catalog=Northwind;integrated security=True;MultipleActiveResultSets=True";

        const string ProviderName = "System.Data.SqlClient";

        IOrderRepository repository;

        [TestInitialize]
        public void Test_Initialization()
        {
            repository = new OrderRepository(ConnectionString, ProviderName);
        }

        [TestMethod]
        public void Test_IsTrueResult_For_GetAllOrders()
        {

            var actualResult = repository.GetAll();
            
             Assert.IsTrue(actualResult.Any());
        }

        [TestMethod]
        public void Test_IsNotNullResult_GetOrderByID()
        {

            var actualResult = repository.GetByID(10280);

            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void Test_IsNotNullResult_CreateNewOrder()
        {

            var newItem = new Order
            {
                Freight = 100500,
                CustomerId = "OTTIK",
                ShipPostalCode = "123",
                ShipName = "testname"
            };

            newItem = repository.Create(newItem);
            var actualResult = repository.GetByID(newItem.Id);
            Assert.IsNotNull(actualResult);
        }


        [TestMethod]
        public void Test_AreEqualResult_For_Update_Order()
        {
            var expectedResult = repository.GetByID(repository.GetAll().First(x => x.Status == OrderStatus.New).Id);
            expectedResult.ShipName = "test";
            var actualResult = repository.Update(expectedResult);
            Assert.AreEqual("test", actualResult.ShipName);
            expectedResult = actualResult;
            expectedResult.OrderDate = DateTime.Now;
            actualResult = repository.Update(expectedResult);
            Assert.IsTrue(actualResult.OrderDate == null);
            expectedResult = actualResult;
            expectedResult.ShippedDate = DateTime.Now;
            actualResult = repository.Update(expectedResult);
            Assert.IsTrue(actualResult.ShippedDate == null);
        }

        [TestMethod]
        public void Test_IsTrueResult_For_MarkShipped_Order()
        {

            var item = repository.GetAll().LastOrDefault();

            if (item != null)
            {
                Assert.IsTrue(repository.MarkShipped(item.Id));
            }
        }

        [TestMethod]
        public void Test_IsTrueResult_For_MarkArrived_Order()
        {

            var item = repository.GetAll().LastOrDefault();

            if (item != null)
            {
                Assert.IsTrue(repository.MarkArrived(item.Id));
            }
        }

        [TestMethod]
        public void Test_IsTrueGettingStatistics_ForOrder_By_StoredProcedure_CustOrderHist()
        {

            var orders = repository.CustOrderHist("BOTTM");

            Assert.IsTrue(orders.Any());
        }

        [TestMethod]
        public void Test_IsTrueGettingStatistics_ForOrder_By_StoredProcedure_OrderDetails()
        {

            var ordersDetails = repository.OrderDetails(10260);

            Assert.IsTrue(ordersDetails.Any());
        }

        [TestMethod]
        public void Test_IsNullResult_For_Delete_Order()
        {


            var item = repository.GetAll().FirstOrDefault();

            if (item != null)
            {
                repository.Delete(item);
                var actualResult = repository.GetByID(item.Id);
                Assert.IsNull(actualResult);
            }
        }
    }
}
