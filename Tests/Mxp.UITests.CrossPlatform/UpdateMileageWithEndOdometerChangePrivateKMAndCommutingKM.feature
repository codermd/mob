Feature: UpdateMileageWithEndOdometerChangePrivateKMAndCommutingKM
		Set start and end odometer in a mileage
		Private KM should be set
		Change it, make it smaller 10 km. Set Commuting (Km) to 10km. The business km should be the distance km

@mileage
Scenario: Update Mileage With End Odometer Change Private KM And Commuting KM
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new mileage from "Brussels, Belgium" to "Waterloo, Belgium"
	And I set "Start Odometer" to "1"
	And I set "End Odometer" to "49"
	And I set "Private (Km)" to "10"
	And I set "Commuting (Km)" to "10"
	Then "Business (Km)" distance is "28"
