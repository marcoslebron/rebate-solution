using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class FixedCashAmountCalculatorTests
{
    private readonly FixedCashAmountCalculator _sut = new();

    [Fact]
    public void CanCalculate_ReturnsTrue_ForFixedCashAmount()
    {
        Assert.True(_sut.CanCalculate(IncentiveType.FixedCashAmount));
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_ForOtherIncentiveTypes()
    {
        Assert.False(_sut.CanCalculate(IncentiveType.AmountPerUom));
        Assert.False(_sut.CanCalculate(IncentiveType.FixedRateRebate));
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenProductSupportsIncentiveAndAmountIsNonZero()
    {
        var rebate = new Rebate { Amount = 10 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var request = new CalculateRebateRequest();

        Assert.True(_sut.IsValid(rebate, product, request));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenProductDoesNotSupportIncentive()
    {
        var rebate = new Rebate { Amount = 100 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var request = new CalculateRebateRequest();

        Assert.False(_sut.IsValid(rebate, product, request));
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenAllConditionsAreMet()
    {
        var rebate = new Rebate { Amount = 100 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        Assert.True(_sut.IsValid(rebate, product, new CalculateRebateRequest()));
    }

    [Fact]
    public void Calculate_ReturnsRebateAmount()
    {
        var rebate = new Rebate { Amount = 250m };

        var result = _sut.Calculate(rebate, new Product(), new CalculateRebateRequest());

        Assert.Equal(250m, result);
    }
}