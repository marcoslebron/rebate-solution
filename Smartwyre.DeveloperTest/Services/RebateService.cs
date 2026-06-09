using System.Collections.Generic;
using System.Linq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;   
    private readonly IEnumerable<IRebateCalculator> _calculators;

    public RebateService(
        IRebateDataStore rebateDataStore,
        IProductDataStore productDataStore,
        IEnumerable<IRebateCalculator> calculators)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _calculators = calculators;
    }
    
    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = _productDataStore.GetProduct(request.ProductIdentifier);
        
        if (rebate == null || product == null)
            return new CalculateRebateResult { Success = false };

        var calculator = _calculators.FirstOrDefault(c => c.CanCalculate(rebate.Incentive));
        
        if (calculator == null || !calculator.IsValid(rebate, product, request))
            return new CalculateRebateResult { Success = false };

        var amount = calculator.Calculate(rebate, product, request);
        _rebateDataStore.StoreCalculationResult(rebate, amount);
        
        return new CalculateRebateResult { Success = true };

    }
}
