Feature: UpdateMileageEndOdometerSmallerThanStart
	Set end odometer smaller than start and check it resets the end to same value as start

@mileage
Scenario: Update Mileage End Odometer Smaller Than Start
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new mileage from "Brussels, Belgium" to "Waterloo, Belgium"
	And I set "Start Odometer" to "30"
	And I set "End Odometer" to "10"
	Then "End Odometer" distance is "58"
