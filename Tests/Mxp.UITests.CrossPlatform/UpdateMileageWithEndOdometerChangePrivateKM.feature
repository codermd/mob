Feature: UpdateMileageWithEndOdometerChangePrivateKM
	Set start and end odometer in a mileage
	Private KM should be set
	Change it, make it smaller. The km removed should be added to business KM

@mileage
Scenario: Update Mileage With End Odometer Change Private KM
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new mileage from "Brussels, Belgium" to "Waterloo, Belgium"
	And I set "Start Odometer" to "1"
	And I set "End Odometer" to "49"
	And I set "Private (Km)" to "10"
	Then "Business (Km)" distance is "38"
