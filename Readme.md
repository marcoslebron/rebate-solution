# Smartwyre Developer Test Instructions

You have been selected to complete our candidate coding exercise. Please follow the directions in this readme.

Clone, **DO NOT FORK**, this repository to your account on the online Git resource of your choosing (GitHub, BitBucket, GitLab, etc.). Your solution should retain previous commit history and you should utilize best practices for committing your changes to the repository.

You are welcome to use whatever tools you normally would when coding — including documentation, libraries, frameworks, or AI tools (such as ChatGPT or Copilot).

However, it is important that you fully understand your solution. As part of the interview process, we will review your code with you in detail. You should be able to:

- Explain the design choices you made.
- Walk us through how your solution works.
- Make modifications or extensions to your code during the review.

Please note: if your submission appears to have been generated entirely by an AI agent or another third party, without your own understanding or contribution, it will not meet our evaluation criteria.

# The Exercise

In the 'RebateService.cs' file you will find a method for calculating a rebate. At a high level the steps for calculating a rebate are:

 1. Lookup the rebate that the request is being made against.
 2. Lookup the product that the request is being made against.
 2. Check that the rebate and request are valid to calculate the incentive type rebate.
 3. Store the rebate calculation.

What we'd like you to do is refactor the code with the following things in mind:

 - Adherence to SOLID principles
 - Testability
 - Readability
 - Currently there are 3 known incentive types. In the future the business will want to add many more incentive types. Your solution should make it easy for developers to add new incentive types in the future.

We'd also like you to 
 - Add some unit tests to the Smartwyre.DeveloperTest.Tests project to show how you would test the code that you've produced 
 - Run the RebateService from the Smartwyre.DeveloperTest.Runner console application accepting inputs (either via command line arguments or via prompts is fine)

The only specific "rules" are:

- The solution must build
- All tests must pass

You are free to use any frameworks/NuGet packages that you see fit. You should plan to spend around 1 hour completing the exercise.

Feel free to use code comments to describe your changes. You are also welcome to update this readme with any important details for us to consider.

Once you have completed the exercise either ensure your repository is available publicly or contact the hiring manager to set up a private share.

---

# Solution Notes

## What was changed and why

The original `RebateService` had all of its logic in a single method — it fetched data, validated inputs, calculated amounts, and stored results, all in one place. Every incentive type was handled by a separate `case` in a switch statement, which meant adding a new incentive type required opening and editing that class directly. The data stores were also instantiated inside the method with `new`, making it impossible to write unit tests that controlled what data came back.

The refactor addresses all of that. The key decisions were:

**Strategy pattern for incentive types.** Each incentive type is now its own class implementing a shared `IRebateCalculator` interface. `RebateService` holds a collection of all registered calculators and picks the right one at runtime using `CanCalculate`. Adding a new incentive type means writing a new class — no existing code changes.

**Constructor injection for dependencies.** The data stores are passed into `RebateService` via its constructor rather than created inside it. This makes the service testable — in unit tests, fake implementations can be swapped in to control what data is returned without hitting any database.

**Interfaces for the data layer.** `IRebateDataStore` and `IProductDataStore` were extracted so the service depends on abstractions rather than concrete classes.

**Dependency injection container.** The runner wires everything together using `Microsoft.Extensions.DependencyInjection`, which is the same container used in ASP.NET Core. All three calculators are registered under `IRebateCalculator` and the container automatically resolves them as a collection when building `RebateService`.

---

## Running the application

```
dotnet run --project Smartwyre.DeveloperTest.Runner
```

The runner will prompt for three inputs: a rebate identifier, a product identifier, and a volume. It will then calculate the rebate and print whether it succeeded.

### Available test data

**Rebates**

| Identifier | Type             | Value              |
|------------|------------------|--------------------|
| CASH-01    | FixedCashAmount  | £500 flat          |
| RATE-01    | FixedRateRebate  | 5% of spend        |
| UOM-01     | AmountPerUom     | £0.08 per unit     |
| VOL-01     | VolumeBonus      | 3% of volume       |

**Products**

| Identifier | Price    | Unit   | Supported incentives                        |
|------------|----------|--------|---------------------------------------------|
| OIL-01     | £2.00    | litre  | All four types                              |
| SEED-01    | £45.00   | kg     | FixedCashAmount, VolumeBonus                |

### Example runs

```
Rebate identifier:  RATE-01
Product identifier: OIL-01
Volume:             10000
→ Rebate calculated successfully.   (£2.00 × 5% × 10,000 = £1,000)

Rebate identifier:  CASH-01
Product identifier: SEED-01
Volume:             500
→ Rebate calculated successfully.   (£500 flat, volume ignored)

Rebate identifier:  RATE-01
Product identifier: SEED-01
Volume:             500
→ Calculation failed.               (SEED-01 does not support FixedRateRebate)
```

---

## Running the tests

```
dotnet test
```

To run a single test by name:

```
dotnet test --filter "FullyQualifiedName~Calculate_ReturnsSuccess"
```

---

## Adding a new incentive type

1. Add the new value to `IncentiveType` (and a matching flag to `SupportedIncentiveType`)
2. Create a class implementing `IRebateCalculator` in `Services/Calculators/`
3. Register it with `services.AddScoped<IRebateCalculator, YourNewCalculator>()` in `Program.cs`
4. Add test data to the stubs in `RebateDataStore` and `ProductDataStore`
5. Write tests in `Smartwyre.DeveloperTest.Tests/Calculators/`

`RebateService` itself does not need to change.
