using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BGLOrdersAPI.DataContexts;
using BGLOrdersAPI.Services;
using BGLOrdersAPI.Models;
using BGLOrdersAPI.Reports;
using BGLOrdersAPI.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BGLOrdersAPITests
{
    [TestClass]
    public class ItemLogicTest
    {
        [TestMethod]
        public void WhenItemWithIDIsValidatedWeCheckIfItemAlreadyExists()
        {
            // Arrange
            Mock<IBGLContext> mockBGLContext = new Mock<IBGLContext>();
            Mock<DateTimeService> mockDateTimeService = new Mock<DateTimeService>();

            Item mockItem = new Item();
            mockItem.ItemId = new Guid();

            Mock<DbSet<Item>> mockDbSet = new Mock<DbSet<Item>>();
            ValueTask<Item> getItemAsync = new ValueTask<Item>(mockItem);
            mockDbSet.Setup(d => d.Find(It.IsAny<Guid>())).Returns(mockItem);
            mockBGLContext.Setup(c => c.Items).Returns(mockDbSet.Object);

            Guid tempGuid = new System.Guid();

            var testController = new ItemLogicContext(mockBGLContext.Object, mockDateTimeService.Object);

            // Act
            bool response = testController.Validate(mockItem);

            // Assert
            // We checked if item of given id already exists in database
            mockDbSet.Verify(m => m.Find(tempGuid), Times.Once);
            // Item passed validation
            Assert.AreEqual(true, response);
        }
    }
}
