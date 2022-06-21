using Bongo.DataAccess;
using Bongo.DataAccess.Repository;
using Bongo.DataAccess.Repository.IRepository;
using Bongo.Models.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bongo.Core
{
    public class StudyRoomServiceTests
    {
        private List<StudyRoom> _availableStudyRoom;
        private DbContextOptions<ApplicationDbContext> options;
        private Mock<IStudyRoomRepository> _studyRoomRepoMock;

        public StudyRoomServiceTests()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "temp_Bongo").Options;

            _availableStudyRoom = new List<StudyRoom>()
            {
                new StudyRoom
                {
                    Id = 10,
                    RoomName = "Michigan",
                    RoomNumber = "A202"
                },
                new StudyRoom()
                {
                    Id = 2,
                    RoomName = "Room A2",
                    RoomNumber = "A2"
                }
            };
          
        }

        [Fact]
        public void GetAll_StudyRoom_OneAndTwo_CheckBothStudyRoomsFromDatabase()
        {
            _studyRoomRepoMock = new Mock<IStudyRoomRepository>();
            _studyRoomRepoMock.Setup(x => x.GetAll()).Returns(_availableStudyRoom);

            var result = _studyRoomRepoMock.Object.GetAll();
            Assert.True(result.Count() > 0);
        }
    }
}
