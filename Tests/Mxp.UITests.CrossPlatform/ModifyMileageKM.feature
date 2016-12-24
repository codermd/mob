Feature: ModifyMileageKM
	Change the private and commuting KM and see the Business KM are well recalculated

@mileage
Scenario: Modify Mileage KM
	Given I am logged with "staging.tmobusr1" "Iphon3"
	And there is a mileage from "Brussels, Belgium" to "Waterloo, Belgium" with "50" "Business (Km)"
	When I set "Commuting (Km)" to "5"
	And I set "Private (Km)" to "7"
	Then "Business (Km)" distance is "38"
