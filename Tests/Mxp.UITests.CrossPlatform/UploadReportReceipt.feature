Feature: UploadReportReceipt
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Upload Report Receipt
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a report
	And I upload an image
	And I press save
	Then the report is saved
