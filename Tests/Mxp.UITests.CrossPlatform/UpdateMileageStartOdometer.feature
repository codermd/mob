Feature: UpdateMileageStartOdometer
	Changing start odometer and check if end odometer is correctly updated

@mileage
Scenario: Update Mileage Start Odometer
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new mileage from "Brussels, Belgium" to "Waterloo, Belgium"
	And I set "Start Odometer" to "2"
	Then "End Odometer" distance is "30"
