using System;
using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;

var services = new ServiceCollection();

services.AddScoped<IRebateDataStore, RebateDataStore>();
services.AddScoped<IProductDataStore, ProductDataStore>();
services.AddScoped<IRebateCalculator, FixedCashAmountCalculator>();
services.AddScoped<IRebateCalculator, FixedRateRebateCalculator>();
services.AddScoped<IRebateCalculator, AmountPerUomCalculator>();
services.AddScoped<IRebateCalculator, VolumeBonusCalculator>();
services.AddScoped<IRebateService, RebateService>();

var provider = services.BuildServiceProvider();
var rebateService = provider.GetRequiredService<IRebateService>();

Console.Write("Rebate identifier: ");
var rebateId = Console.ReadLine();

Console.Write("Product identifier: ");
var productId = Console.ReadLine();

Console.Write("Volume: ");
var volume = decimal.Parse(Console.ReadLine()!);

var result = rebateService.Calculate(new CalculateRebateRequest
{
    RebateIdentifier = rebateId,
    ProductIdentifier = productId,
    Volume = volume
});

Console.WriteLine(result.Success
    ? "Rebate calculated successfully."
    : "Calculation failed — check rebate/product identifiers and inputs.");