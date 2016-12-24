Feature: DeleteExpense
	Deleting an expense

@expense
Scenario: Delete an expense
    Given I am logged with "staging.tmobusr1" "Iphon3"
    When There is an expense
	And I select it
	And I delete it
	Then The expense is deleted
