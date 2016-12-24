Feature: CreateExpense
	Creating an expense

 @expense
Scenario: Create an expense
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new expense with category "Car Maintenance" and country "BE - Belgium" and "1" quantity of "10" "Euro (EUR)"
	And I select autocomplete "Sagacify" as "Supplier"
	And I enter "Brussels" as "City/Town"
	And I select "oxygen(O)" as "Project"
	And I select "mobile(mobile)" as "Cost centre"
	And I select "456533 - testtravel006" as "Travel request"
	And I save it
	Then The expense is saved
	And "* Category" is "Car Maintenance"
	And "* Country" is "Belgium"
	And "* Amount" is "10 EUR"
	And "Supplier" is "Sagacify"
	And "Project" is "oxygen"
	And "Cost centre" is "mobile(mobile)"
	And "Travel request" is "456533 - testtravel006"
