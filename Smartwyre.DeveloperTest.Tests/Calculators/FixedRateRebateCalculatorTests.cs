using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class FixedRateRebateCalculatorTests
{
    private readonly FixedRateRebateCalculator _sut = new();

    [Fact]
    public void CanCalculate_ReturnsTrue_ForFixedRateRebate()
    {
        Assert.True(_sut.CanCalculate(IncentiveType.FixedRateRebate));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenProductDoesNotSupportIncentive()
    {
        var rebate = new Rebate { Percentage = 0.1m };
        var product = new Product { Price = 100, SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var request = new CalculateRebateRequest { Volume = 10 };

        Assert.False(_sut.IsValid(rebate, product, request));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenPercentageIsZero()
    {
        var rebate = new Rebate { Percentage = 0 };
        var product = new Product { Price = 100, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
        var request = new CalculateRebateRequest { Volume = 10 };

        Assert.False(_sut.IsValid(rebate, product, request));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenPriceIsZero()
    {
        var rebate = new Rebate { Percentage = 0.1m };
        var product = new Product { Price = 0, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
        var request = new CalculateRebateRequest { Volume = 10 };

        Assert.False(_sut.IsValid(rebate, product, request));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenVolumeIsZero()
    {
        var rebate = new Rebate { Percentage = 0.1m };
        var product = new Product { Price = 100, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
        var request = new CalculateRebateRequest { Volume = 0 };

        Assert.False(_sut.IsValid(rebate, product, request));
    }

      [Fact]
    public void IsValid_ReturnsTrue_WhenAllConditionsAreMet()
    {
        var rebate = new Rebate { Percentage = 0.1m };
        var product = new Product { Price = 100, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
        Assert.True(_sut.IsValid(rebate, product, new CalculateRebateRequest { Volume = 10 }));
    }

    [Fact]
    public void Calculate_ReturnsCorrectRebateAmount()
    {
        var rebate = new Rebate { Percentage = 0.1m };
        var product = new Product { Price = 100, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
        var request = new CalculateRebateRequest { Volume = 10 };

        var result = _sut.Calculate(rebate, product, request);
        Assert.Equal(100m, result);
    }
}