using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class VolumeBonusCalculator : IRebateCalculator
{
    public bool CanCalculate(IncentiveType incentiveType) =>
        incentiveType == IncentiveType.VolumeBonus;

    public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request) =>
        product.SupportedIncentives.HasFlag(SupportedIncentiveType.VolumeBonus)
        && rebate.Percentage != 0
        && request.Volume != 0;

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request) =>
        rebate.Percentage * request.Volume;
}
