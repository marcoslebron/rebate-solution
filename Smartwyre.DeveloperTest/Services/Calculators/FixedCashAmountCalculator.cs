using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class FixedCashAmountCalculator : IRebateCalculator
{
    public bool CanCalculate(IncentiveType incentiveType) =>
        incentiveType == IncentiveType.FixedCashAmount;

    public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request) =>
        product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) && rebate.Amount != 0;
        // Implement validation logic specific to fixed cash amount rebatess
    
    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request) =>
        // For a fixed cash amount rebate, the calculation is straightforward
        rebate.Amount;
}