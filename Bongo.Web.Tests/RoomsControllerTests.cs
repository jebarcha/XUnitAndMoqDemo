using Bongo.Core.Services.IServices;
using Bongo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bongo.Web.Tests
{
    public class RoomsControllerTests
    {
        private Mock<IStudyRoomService> _studyRoomService;
        private RoomsController _roomsController;

        public RoomsControllerTests()
        {
            _studyRoomService = new Mock<IStudyRoomService>();
            _roomsController = new RoomsController(_studyRoomService.Object);
        }

        [Fact]
        public void IndexPage_CallRequest_VerifyGetAllInvoked()
        {
            var result = _roomsController.Index();

            Assert.IsType<ViewResult>(result);
            _studyRoomService.Verify(x => x.GetAll(), Times.Once());
        }
    }
}
