using Bongo.Core.Services.IServices;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using Bongo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace Bongo.Web.Tests
{
    public class RoomBookingControllerTests
    {
        private Mock<IStudyRoomBookingService> _studyRoomBookingService;
        private RoomBookingController _bookingController;

        public RoomBookingControllerTests()
        {
            _studyRoomBookingService = new Mock<IStudyRoomBookingService>();
            _bookingController = new RoomBookingController(_studyRoomBookingService.Object);
        }

        [Fact]
        public void IndexPage_CallRequest_VerifyGetAllInvoked()
        {
            var result = _bookingController.Index();
            
            Assert.IsType<ViewResult>(result);
            _studyRoomBookingService.Verify(x => x.GetAllBooking(), Times.Once());
        }

        [Fact]
        public void BookRoomCheck_ModelStateInvalid_ReturnView()
        {
            _bookingController.ModelState.AddModelError("test-key", "test-errorMessage");

            var result = _bookingController.Book(new StudyRoomBooking());

            ViewResult viewResult = result as ViewResult;
            Assert.Equal("Book", viewResult.ViewName);
        }

        [Fact]
        public void BookRoomCheck_NotSuccessful_NoRoomCode()
        {
            _studyRoomBookingService.Setup(x => x.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
                .Returns(new StudyRoomBookingResult() {
                    Code = StudyRoomBookingCode.NoRoomAvailable
            });

            var result = _bookingController.Book(new StudyRoomBooking());

            Assert.IsType<ViewResult>(result);
            ViewResult viewResult = result as ViewResult;
            Assert.Equal("No Study Room available for selected date", viewResult.ViewData["Error"]);
        }

        [Fact]
        public void BookRoomCheck_Successful_SuccessCodeAndRedirect()
        {
            //arrange
            _studyRoomBookingService.Setup(x => x.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
                .Returns((StudyRoomBooking booking) => new StudyRoomBookingResult()
                {
                    Code = StudyRoomBookingCode.Success,
                    FirstName = booking.FirstName,
                    LastName = booking.LastName,
                    Date = booking.Date,
                    Email = booking.Email
                });

            //act
            var result = _bookingController.Book(new StudyRoomBooking()
            {
                Date = DateTime.Now,
                Email = "demo@gmail.com",
                FirstName = "Demo",
                LastName = "DemoLN",
                StudyRoomId = 1
            });

            //assert
            Assert.IsType<RedirectToActionResult>(result);
            RedirectToActionResult actionResult = result as RedirectToActionResult;
            Assert.Equal("Demo", actionResult.RouteValues["FirstName"]);
            Assert.Equal(StudyRoomBookingCode.Success, actionResult.RouteValues["Code"]);
        }
    }
}