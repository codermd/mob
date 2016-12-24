Feature: UpdateMileageSetStartEndOdometerThenChangeStart
	Set start and end odometer then change start. Check if end odometer was updated

@mileage
Scenario: Update Mileage Set Start End Odometer Then Change Start
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new mileage from "Brussels, Belgium" to "Waterloo, Belgium"
	And I set "Start Odometer" to "5"
	And I set "End Odometer" to "35"
	And I set "Start Odometer" to "3"
	Then "End Odometer" distance is "31"
	And "Private (Km)" distance is "2"
