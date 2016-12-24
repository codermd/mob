Feature: UpdateMileageEndOdometer
	Changing end odometer and check if private km is correctly updated

@mileage
Scenario: Update Mileage End Odometer
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new mileage from "Brussels, Belgium" to "Waterloo, Belgium"
	And I set "Start Odometer" to "2"
	And I set "End Odometer" to "35"
	Then "Private (Km)" distance is "5"
