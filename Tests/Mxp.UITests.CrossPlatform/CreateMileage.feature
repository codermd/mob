Feature: CreateMileage
	Creating a new mileage

@mileage
Scenario: Creating a mileage
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new mileage from "Brussels, Belgium" to "Waterloo, Belgium"
	And I save it
	Then The mileage is saved
	And "Calculated distance (Km)" distance is "28"
