Feature: PullToRefreshExpenseList
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@expense

Scenario: Pull To Refresh Expense List
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I pull to refresh expenses list
	Then Expenses list is refreshed
