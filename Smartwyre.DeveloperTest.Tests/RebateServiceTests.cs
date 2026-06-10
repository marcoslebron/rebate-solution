using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    private readonly Mock<IRebateDataStore> _rebateStoreMock = new();
    private readonly Mock<IProductDataStore> _productStoreMock = new();

    private RebateService BuildSut(params IRebateCalculator[] calculators) =>
        new(_rebateStoreMock.Object, _productStoreMock.Object, calculators);

    [Fact]
    public void Calculate_ReturnsFailure_WhenRebateIsNotFound()
    {
        _rebateStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns((Rebate)null);

        var result = BuildSut().Calculate(new CalculateRebateRequest());

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ReturnsFailure_WhenProductIsNotFound()
    {
        _rebateStoreMock.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(new Rebate());
        _productStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns((Product)null);

        var result = BuildSut().Calculate(new CalculateRebateRequest());

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ReturnsFailure_WhenNoCalculatorSupportsIncentiveType()
    {
        _rebateStoreMock.Setup(s => s.GetRebate(It.IsAny<string>()))
            .Returns(new Rebate { Incentive = IncentiveType.FixedCashAmount });
        _productStoreMock.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(new Product());

        // Pass no calculators — simulates an unrecognised incentive type
        var result = BuildSut().Calculate(new CalculateRebateRequest());

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ReturnsSuccess_AndStoresResult_WhenCalculationIsValid()
    {
        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 100m
        };
        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        _rebateStoreMock.Setup(s => s.GetRebate("R1")).Returns(rebate);
        _productStoreMock.Setup(s => s.GetProduct("P1")).Returns(product);

        var result = BuildSut(new FixedCashAmountCalculator()).Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "R1",
            ProductIdentifier = "P1"
        });

        Assert.True(result.Success);
        _rebateStoreMock.Verify(s => s.StoreCalculationResult(rebate, 100m), Times.Once);
    }

    [Fact]
    public void Calculate_ReturnsFailure_AndDoesNotStore_WhenCalculationIsInvalid()
    {
        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 0     // ← zero amount makes IsValid return false
        };
        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        _rebateStoreMock.Setup(s => s.GetRebate("R1")).Returns(rebate);
        _productStoreMock.Setup(s => s.GetProduct("P1")).Returns(product);

        var result = BuildSut(new FixedCashAmountCalculator()).Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "R1",
            ProductIdentifier = "P1"
        });

        Assert.False(result.Success);
        _rebateStoreMock.Verify(s => s.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }
}