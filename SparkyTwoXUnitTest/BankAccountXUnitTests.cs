using Moq;
using SparkyTwo;
using Xunit;

namespace SparkyTwoXUnitTest;
public class BankAccountXUnitTests
{

    [Fact]
    public void BankDepositLogFakker_Add100_ReturnTrue()
    {
        //var bankAccont = new BankAccount(new LogFakker());
        //var result = bankAccont.Deposit(100);
        //Assert.True(result);
        //Assert.Equal(100, bankAccont.GetBalance());
    }

    [Fact]
    public void BankDeposit_Add100_ReturnTrue()
    {
        var logMock = new Mock<ILogBook>();
        logMock.Setup(x => x.Message("Deposit invoked"));

        var bankAccont = new BankAccount(logMock.Object);
        var result = bankAccont.Deposit(100);
        Assert.True(result);
        Assert.Equal(100, bankAccont.GetBalance());
    }

    //[Fact]
    [Theory]
    [InlineData(200, 100)]
    [InlineData(200, 150)]
    public void BankWithdraw_Withdraw100With200Balance_ReturnsTrue(int balance, int withdraw)
    {
        var logMock = new Mock<ILogBook>();
        logMock.Setup(u => u.LogToDB(It.IsAny<string>())).Returns(true);
        //logMock.Setup(u => u.LogBalanceAfterWithdrawal(It.IsAny<int>())).Returns(true);
        logMock.Setup(u => u.LogBalanceAfterWithdrawal(It.Is<int>(x => x > 0))).Returns(true);

        var bankAccont = new BankAccount(logMock.Object);
        bankAccont.Deposit(balance);
        var result = bankAccont.Withrdraw(withdraw);
        Assert.True(result);
    }

    [Theory]
    [InlineData(200, 300)]
    public void BankWithdraw_Withdraw300With200Balance_ReturnsFalse(int balance, int withdraw)
    {
        var logMock = new Mock<ILogBook>();
        logMock.Setup(u => u.LogBalanceAfterWithdrawal(It.Is<int>(x => x > 0))).Returns(true);
        //logMock.Setup(u => u.LogBalanceAfterWithdrawal(It.Is<int>(x => x < 0))).Returns(false);
        logMock.Setup(u => u.LogBalanceAfterWithdrawal(It.IsInRange<int>(int.MinValue, -1, Range.Inclusive))).Returns(false);

        var bankAccont = new BankAccount(logMock.Object);
        bankAccont.Deposit(balance);
        var result = bankAccont.Withrdraw(withdraw);
        Assert.False(result);
    }

    [Fact]
    public void BankLogDummy_LogMockString_ReturnTrue()
    {
        var logMock = new Mock<ILogBook>();
        string desiredOutput = "hello";

        logMock.Setup(u => u.MessageWithReturnStr(It.IsAny<string>())).Returns((string str) => str.ToLower());

        Assert.Equal(desiredOutput, logMock.Object.MessageWithReturnStr("HEllo"));
    }


    [Fact]
    public void BankLogDummy_LogMockStringOutputStr_ReturnTrue()
    {
        var logMock = new Mock<ILogBook>();
        string desiredOutput = "hello";

        logMock.Setup(u => u.LogWithOutputResult(It.IsAny<string>(), out desiredOutput)).Returns(true);
        string result = "";

        Assert.True(logMock.Object.LogWithOutputResult("Jose", out result));
        Assert.Equal(desiredOutput, result);
    }

    [Fact]
    public void BankLogDummy_LogRefChecker_ReturnTrue()
    {
        var logMock = new Mock<ILogBook>();
        Customer customer = new();
        Customer customerNotUsed = new();

        logMock.Setup(u => u.LogWithRefObj(ref customer)).Returns(true);

        Assert.True(logMock.Object.LogWithRefObj(ref customer));
        Assert.False(logMock.Object.LogWithRefObj(ref customerNotUsed));
    }



    [Fact]
    public void BankLogDummy_SetAndGetLogTypeAndSeverityMock_MockTest()
    {
        var logMock = new Mock<ILogBook>();
        //logMock.SetupAllProperties();
        logMock.Setup(u => u.LogSeverity).Returns(10);
        logMock.Setup(u => u.LogType).Returns("warning");

        //logMock.Object.LogSeverity = 100;
        Assert.Equal(10, logMock.Object.LogSeverity);
        Assert.Equal("warning", logMock.Object.LogType);

        //callbacks
        string logTemp = "Hello, ";
        logMock.Setup(u => u.LogToDB(It.IsAny<string>())).Returns(true)
            .Callback((string str) => logTemp+=str);
        logMock.Object.LogToDB("Ben");
        Assert.Equal("Hello, Ben", logTemp);

        //callbacks
        int counter = 5;
        logMock.Setup(u => u.LogToDB(It.IsAny<string>()))
            .Callback(() => counter++)
            .Returns(true)
            .Callback(() => counter++);
        logMock.Object.LogToDB("Ben");
        logMock.Object.LogToDB("Ben");
        Assert.Equal(9, counter);

    }

    [Fact]
    public void BankLogDummy_VerifyExample()
    {
        var logMock = new Mock<ILogBook>();
        BankAccount bankAccount = new BankAccount(logMock.Object);
        bankAccount.Deposit(100);
        Assert.Equal(100, bankAccount.GetBalance());

        //verification
        logMock.Verify(u => u.Message(It.IsAny<string>()), Times.Exactly(2));
        logMock.Verify(u => u.Message("Test"), Times.AtLeastOnce);
        logMock.VerifySet(u => u.LogSeverity = 101, Times.Once);
        logMock.VerifyGet(u => u.LogSeverity, Times.Once);
    }

}
