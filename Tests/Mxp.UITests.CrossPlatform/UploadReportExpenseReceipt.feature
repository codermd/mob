Feature: UploadReportExpenseReceipt
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers


Scenario: Upload Report Expense Receipt
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new report with expense
	And I add a receipt to the report expense
	Then the receipt is saved
