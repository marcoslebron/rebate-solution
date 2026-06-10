using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    public Product GetProduct(string productIdentifier)
    {
        // Hardcoded test data standing in for a real database
        return productIdentifier switch
        {
            "OIL-01" => new Product
            {
                Identifier = "OIL-01",
                Price = 2.00m,
                Uom = "litre",
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount
                                    | SupportedIncentiveType.FixedRateRebate
                                    | SupportedIncentiveType.AmountPerUom
                                    | SupportedIncentiveType.VolumeBonus
            },
            "SEED-01" => new Product
            {
                Identifier = "SEED-01",
                Price = 45.00m,
                Uom = "kg",
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount
                                    | SupportedIncentiveType.VolumeBonus
            },
            _ => null
        };
    }
}
