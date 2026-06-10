using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class AmountPerUomCalculatorTests
{
    private readonly AmountPerUomCalculator _sut = new();

    [Fact]
    public void CanCalculate_ReturnsTrue_ForAmountPerUom()
    {
        Assert.True(_sut.CanCalculate(IncentiveType.AmountPerUom));
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_ForOtherTypes()
    {
        Assert.False(_sut.CanCalculate(IncentiveType.FixedCashAmount));
        Assert.False(_sut.CanCalculate(IncentiveType.FixedRateRebate));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenProductDoesNotSupportIncentive()
    {
        var rebate = new Rebate { Amount = 5 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        Assert.False(_sut.IsValid(rebate, product, new CalculateRebateRequest { Volume = 10 }));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenAmountIsZero()
    {
        var rebate = new Rebate { Amount = 0 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };

        Assert.False(_sut.IsValid(rebate, product, new CalculateRebateRequest { Volume = 10 }));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenVolumeIsZero()
    {
        var rebate = new Rebate { Amount = 5 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };

        Assert.False(_sut.IsValid(rebate, product, new CalculateRebateRequest { Volume = 0 }));
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenAllConditionsAreMet()
    {
        var rebate = new Rebate { Amount = 5 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };

        Assert.True(_sut.IsValid(rebate, product, new CalculateRebateRequest { Volume = 10 }));
    }

    [Fact]
    public void Calculate_ReturnsCorrectAmount()
    {
        // 5 (amount) × 10 (volume) = 50
        var rebate = new Rebate { Amount = 5m };
        var request = new CalculateRebateRequest { Volume = 10m };

        Assert.Equal(50m, _sut.Calculate(rebate, new Product(), request));
    }
}
