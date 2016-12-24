Feature: Login
	I want to login


Scenario: Login
	Given I am on Login Screen
	When I enter credential "staging.tmobusr1" "Iphon3"
	And I press Login
	Then I should be logged
