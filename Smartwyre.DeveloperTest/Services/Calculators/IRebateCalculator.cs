using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public interface IRebateCalculator
{
    bool CanCalculate(IncentiveType incentiveType);
    bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request);
    decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
}