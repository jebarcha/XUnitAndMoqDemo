using Bongo.Models.ModelValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bongo.Models;
public class DateInFutureAttributeTests
{
    //[Fact]
    [Theory]
    [InlineData(100, true)]
    [InlineData(-100, false)]
    [InlineData(0, false)]
    public void DateValidator_InputExpectedDateRange_DateValidity(int addTime, bool expected)
    {
        DateInFutureAttribute dateInFutureAttribute = new(() => DateTime.Now);

        var result = dateInFutureAttribute.IsValid(DateTime.Now.AddSeconds(addTime));

        Assert.Equal(expected, result);
    }

    [Fact]
    public void DateValidator_AnyDate_ReturnErrorMessage()
    {
        var result = new DateInFutureAttribute();
        Assert.Equal("Date must be in the future", result.ErrorMessage);
    }

}
