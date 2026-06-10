using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    public Rebate GetRebate(string rebateIdentifier)
    {
        // Hardcoded test data standing in for a real database
        return rebateIdentifier switch
        {
            "CASH-01" => new Rebate { Identifier = "CASH-01", Incentive = IncentiveType.FixedCashAmount, Amount = 500m },
            "RATE-01" => new Rebate { Identifier = "RATE-01", Incentive = IncentiveType.FixedRateRebate, Percentage = 0.05m },
            "UOM-01"  => new Rebate { Identifier = "UOM-01",  Incentive = IncentiveType.AmountPerUom, Amount = 0.08m },
            "VOL-01"  => new Rebate { Identifier = "VOL-01",  Incentive = IncentiveType.VolumeBonus, Percentage = 0.03m },
            _         => null
        };
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
