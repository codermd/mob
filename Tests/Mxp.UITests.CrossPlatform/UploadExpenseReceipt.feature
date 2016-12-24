Feature: UploadExpenseReceipt
	Test upload of receipt in an expense


Scenario: Upload Expense Receipt
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create an expense
	And I upload an image
	And I press save
	Then the receipt is saved
