Feature: DeleteReport
	Delete a report


Scenario: Delete a report
    Given I am logged with "staging.tmobusr1" "Iphon3"
	When There is a report
	And I select it
	And I delete it
	Then The report is deleted
