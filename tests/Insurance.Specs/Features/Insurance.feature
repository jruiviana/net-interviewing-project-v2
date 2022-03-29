Feature: Insurance
calculates the total cost of insurance 

@mytag
Scenario:  No insurance required.
	Given product id 1
	When the Insurance is calculated
	Then no insurance required
	
@mytag
Scenario Outline:  Should Add Thousand Euros To InsuranceCost.
	Given product id <productId>
	When the Insurance is calculated
	Then the insurance should be <insurance>
	Examples: 
	| productId | insurance |
	| 2         | 1000      |
	| 7         | 1000      |	
	