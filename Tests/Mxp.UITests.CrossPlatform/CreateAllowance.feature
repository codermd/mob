Feature: CreateAllowance
	Create and save a new allowance


Scenario: Create allowance
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new allowance in "BE - Belgium"
	And I switch "Breakfast"
	And I switch "Lunch"
	And I switch "Dinner"
	And I save it
	Then The allowance is saved
	And Icon "Breakfast" is set
	And Icon "Lunch" is set
	And Icon "Dinner" is set
