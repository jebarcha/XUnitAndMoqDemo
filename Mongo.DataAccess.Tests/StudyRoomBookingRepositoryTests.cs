using Bongo.DataAccess.Repository;
using Bongo.Models.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using XUnit.Project.Attributes;

namespace Bongo.DataAccess
{
    [TestCaseOrderer("XUnit.Project.Orderers.PriorityOrderer", "XUnit.Project")]
    public class StudyRoomBookingRepositoryTests
    {
        private StudyRoomBooking studyRoombooking_One;
        private StudyRoomBooking studyRoombooking_Two;
        private DbContextOptions<ApplicationDbContext> options;

        public StudyRoomBookingRepositoryTests()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "temp_Bongo").Options;

            studyRoombooking_One = new StudyRoomBooking()
            {
                FirstName = "Ben1",
                LastName = "Spark1",
                Date = new DateTime(2023, 1, 1),
                Email = "ben1@gmail.com",
                BookingId = 11,
                StudyRoomId = 1
            };

            studyRoombooking_Two = new StudyRoomBooking()
            {
                FirstName = "Ben2",
                LastName = "Spark2",
                Date = new DateTime(2023, 2, 2),
                Email = "ben2@gmail.com",
                BookingId = 22,
                StudyRoomId = 2
            };
        }

        [Fact, TestPriority(1)]
        public void SaveBooking_Booking_One_CheckTheValuesFromDatabase()
        {
            //arrange
            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //    .UseInMemoryDatabase(databaseName: "temp_Bongo").Options;

            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoombooking_One);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var bookingFromDb = context.StudyRoomBookings.FirstOrDefault(u => u.BookingId == 11);
                Assert.Equal(bookingFromDb.BookingId, studyRoombooking_One.BookingId);
                Assert.Equal(bookingFromDb.FirstName, studyRoombooking_One.FirstName);
                Assert.Equal(bookingFromDb.LastName, studyRoombooking_One.LastName);
                Assert.Equal(bookingFromDb.Email, studyRoombooking_One.Email);
                Assert.Equal(bookingFromDb.Date, studyRoombooking_One.Date);
            }
        }


        [Fact, TestPriority(2)]
        public void GetAll_Booking_OneAndTwo_CheckBothBookingFromDatabase()
        {
            //arrange
            var expectedResult = new List<StudyRoomBooking> { studyRoombooking_One, studyRoombooking_Two };

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoombooking_One);
                repository.Book(studyRoombooking_Two);
            }

            //act
            List<StudyRoomBooking> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new StudyRoomBookingRepository(context);
                actualList = repository.GetAll(null).ToList();
            }

            //assert  (Using fluentAssertions package)
            expectedResult.Should().BeEquivalentTo(actualList);
        }

        //private class BookingCompare : IComparer
        //{
        //    public int Compare(object? x, object? y)
        //    {
        //        var booking1 = (StudyRoomBooking)x;
        //        var booking2 = (StudyRoomBooking)y;
        //        return (booking1.BookingId != booking2.BookingId) ? 1 : 0;
        //    }
        //}

    }
}
