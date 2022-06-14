using Moq;
using SparkyTwo;
using Xunit;

namespace SparkyTwoXUnitTest;

public class ProductXUnitTest
{
    [Fact]
    public void GetProductPrice_PlatinumCustomer_ReturnPriceWith20Discount()
    {
        Product product = new Product() { Price = 50 };

        var result = product.GetPrice(new Customer() { IsPlatinum = true });

        Assert.Equal(40, result);
    }


    [Fact]
    public void GetProductPriceMOQAbuse_PlatinumCustomer_ReturnPriceWith20Discount()
    {
        var customer = new Mock<ICustomer>();
        customer.Setup(u => u.IsPlatinum).Returns(true);

        Product product = new Product() { Price = 50 };

        var result = product.GetPrice(customer.Object);

        Assert.Equal(40, result);
    }



}
