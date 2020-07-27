using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BGLOrdersAPI.Controllers;
using Microsoft.Extensions.Logging;
using BGLOrdersAPI.DataContexts;
using BGLOrdersAPI.Services;
using BGLOrdersAPI.Models;
using BGLOrdersAPI.Reports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BGLOrdersAPITests
{
    // Class to test the behavior of the ItemsController. A similar test suite would be written for the OrderController as well in reality.
    [TestClass]
    public class ItemsControllerTest
    {
        [TestMethod]
        public void WhenGetItemsCalledWithGuidThenControllerGetsItemBasedOnGUID()
        {
            // Arrange
            Mock<ILogger<ItemsController>> mockLogger = new Mock<ILogger<ItemsController>>();
            Mock<IBGLContext> mockBGLContext = new Mock<IBGLContext>();
            Mock<DateTimeService> mockDateTimeService = new Mock<DateTimeService>();
            Mock<ILogicContext<Item, ItemReport>> mockLogicContext = new Mock<ILogicContext<Item, ItemReport>>();
            Item mockItem = new Item();

            Mock<DbSet<Item>> mockDbSet = new Mock<DbSet<Item>>();
            ValueTask<Item> getItemAsync = new ValueTask<Item>(mockItem);
            mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).Returns(getItemAsync);
            mockBGLContext.Setup(c => c.Items).Returns(mockDbSet.Object);

            Guid tempGuid = new System.Guid();

            var testController = new ItemsController(mockLogger.Object, mockBGLContext.Object, mockDateTimeService.Object, mockLogicContext.Object);

            // Act
            Item response = testController.GetItems(tempGuid).GetAwaiter().GetResult().Value;

            // Assert
            // Attempted to get the record from the DB using the Guid Provided
            mockDbSet.Verify(m => m.FindAsync(tempGuid), Times.Once);
            // Correct item was retrieved
            Assert.AreEqual(mockItem, response);
        }
    }
}
