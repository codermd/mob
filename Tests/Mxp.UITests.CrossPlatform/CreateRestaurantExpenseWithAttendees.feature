Feature: CreateRestaurantExpenseWithAttendees
	Create an entertainment expense with attendees.
	Check the attendees

@expense
Scenario: Create Restaurant Expense With Attendees
	Given I am logged with "staging.tmobusr1" "Iphon3"
	When I create a new expense with category "Restaurant" and country "BE - Belgium" and "1" quantity of "20" "Euro (EUR)"
	And I select autocomplete "Sagacify" as "* Supplier"
	And I enter "Brussels" as "* City/Town"
	And I select "mobile(mobile)" as "* Cost centre"
	And I add a business relation "Bill" "Jobs" from "Orange" 
	And I add a spouse "Cersei" "Lannister"
	And I add a HCP "" "" "" "" "Mexico" "c" "" ""
	And I add a HCO "" "" "" "c" "" ""
	And I add a UHCP "Daenerys" "Targaryen" "Dragon Inc" "1" "Slaver's bay" "Essos" "Murder" "12345"
	And I save it
	Then The expense is saved
	And "* Category" is "Restaurant"
	And "* Country" is "Belgium"
	And "* Amount" is "20 EUR"
	And "* Supplier" is "Sagacify"
	And "* Cost centre" is "mobile(mobile)"
	And There are "5" attendees
	And The "attendee" icon is set with "5" counter on expense cell
