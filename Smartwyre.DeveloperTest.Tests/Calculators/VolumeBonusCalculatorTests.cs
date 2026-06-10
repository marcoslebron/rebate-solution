using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class VolumeBonusCalculatorTests
{
    private readonly VolumeBonusCalculator _sut = new();

    [Fact]
    public void CanCalculate_ReturnsTrue_ForVolumeBonus()
    {
        Assert.True(_sut.CanCalculate(IncentiveType.VolumeBonus));
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_ForOtherTypes()
    {
        Assert.False(_sut.CanCalculate(IncentiveType.FixedCashAmount));
        Assert.False(_sut.CanCalculate(IncentiveType.FixedRateRebate));
        Assert.False(_sut.CanCalculate(IncentiveType.AmountPerUom));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenProductDoesNotSupportIncentive()
    {
        var rebate = new Rebate { Percentage = 0.03m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        Assert.False(_sut.IsValid(rebate, product, new CalculateRebateRequest { Volume = 100 }));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenPercentageIsZero()
    {
        var rebate = new Rebate { Percentage = 0 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.VolumeBonus };

        Assert.False(_sut.IsValid(rebate, product, new CalculateRebateRequest { Volume = 100 }));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenVolumeIsZero()
    {
        var rebate = new Rebate { Percentage = 0.03m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.VolumeBonus };

        Assert.False(_sut.IsValid(rebate, product, new CalculateRebateRequest { Volume = 0 }));
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenAllConditionsAreMet()
    {
        var rebate = new Rebate { Percentage = 0.03m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.VolumeBonus };

        Assert.True(_sut.IsValid(rebate, product, new CalculateRebateRequest { Volume = 100 }));
    }

    [Fact]
    public void Calculate_ReturnsCorrectAmount()
    {
        // 0.03 (percentage) × 10,000 (volume) = 300
        var rebate = new Rebate { Percentage = 0.03m };
        var request = new CalculateRebateRequest { Volume = 10_000m };

        Assert.Equal(300m, _sut.Calculate(rebate, new Product(), request));
    }
}
