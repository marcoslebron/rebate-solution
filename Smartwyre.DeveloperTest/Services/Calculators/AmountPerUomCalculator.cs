using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;
public class AmountPerUomCalculator : IRebateCalculator
{
    public bool CanCalculate(IncentiveType incentiveType) =>
        incentiveType == IncentiveType.AmountPerUom;

    public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request) =>
        product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) 
        && rebate.Amount != 0
        && request.Volume != 0;
        
    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request) =>
        rebate.Amount * request.Volume;
}