﻿using System;

namespace sc
{
	public class FakeData
	{
		
		public static string products = "[{\"Id\":\"3\",\"Name\":\"Car Maintenance\"},{\"Id\":\"5\",\"Name\":\"Car ownership\"},{\"Id\":\"6\",\"Name\":\"Car rental\"},{\"Id\":\"7\",\"Name\":\"Carwash\"},{\"Id\":\"10\",\"Name\":\"Entertainment v\"},{\"Id\":\"40\",\"Name\":\"FACTURE\"},{\"Id\":\"14\",\"Name\":\"Fuel (Fuel Card)\"},{\"Id\":\"60\",\"Name\":\"Gifts\"},{\"Id\":\"16\",\"Name\":\"Hotel\"},{\"Id\":\"12\",\"Name\":\"Hotel breakfast\"},{\"Id\":\"88\",\"Name\":\"Hotel Lodging\"},{\"Id\":\"147\",\"Name\":\"Hotel Personal\"},{\"Id\":\"57\",\"Name\":\"Hotel Room Service\"},{\"Id\":\"18\",\"Name\":\"Mail and Courier\"},{\"Id\":\"92\",\"Name\":\"Mileage Overtime\"},{\"Id\":\"19\",\"Name\":\"Mileage standard\"},{\"Id\":\"39\",\"Name\":\"NO RECEIPT\"},{\"Id\":\"23\",\"Name\":\"Office supplies, furniture\"},{\"Id\":\"24\",\"Name\":\"Other\"},{\"Id\":\"48\",\"Name\":\"Other supplies\"},{\"Id\":\"25\",\"Name\":\"Parking\"},{\"Id\":\"58\",\"Name\":\"Per diem allowance\"},{\"Id\":\"38\",\"Name\":\"Personal Expenses\"},{\"Id\":\"27\",\"Name\":\"Public transport\"},{\"Id\":\"28\",\"Name\":\"Restaurant\"},{\"Id\":\"31\",\"Name\":\"Taxi In Town\"},{\"Id\":\"119\",\"Name\":\"Taxi Long Distance\"},{\"Id\":\"32\",\"Name\":\"Telecom\"},{\"Id\":\"41\",\"Name\":\"Terminal allowance\"},{\"Id\":\"1903\",\"Name\":\"TEST\"},{\"Id\":\"62\",\"Name\":\"TIME\"},{\"Id\":\"34\",\"Name\":\"Toll\"},{\"Id\":\"36\",\"Name\":\"Travel\"},{\"Id\":\"2\",\"Name\":\"Travel services\"}]";
		public static string countries = "[{\"Name\":\"AF - Afghanistan\",\"Id\":\"4\"},{\"Name\":\"AL - Albania\",\"Id\":\"8\"},{\"Name\":\"DZ - Algeria\",\"Id\":\"12\"},{\"Name\":\"AS - American Samoa\",\"Id\":\"16\"},{\"Name\":\"AD - Andorra\",\"Id\":\"20\"},{\"Name\":\"AO - Angola\",\"Id\":\"24\"},{\"Name\":\"AI - Anguilla\",\"Id\":\"660\"},{\"Name\":\"AQ - Antarctica\",\"Id\":\"10\"},{\"Name\":\"AG - Antigua & Barbuda\",\"Id\":\"28\"},{\"Name\":\"AR - Argentina\",\"Id\":\"32\"},{\"Name\":\"AM - Armenia\",\"Id\":\"51\"},{\"Name\":\"AW - Aruba\",\"Id\":\"533\"},{\"Name\":\"GR - Athens\",\"Id\":\"1155\"},{\"Name\":\"US - Atlanta\",\"Id\":\"1144\"},{\"Name\":\"AU - Australia\",\"Id\":\"36\"},{\"Name\":\"AT - Austria\",\"Id\":\"40\"},{\"Name\":\"AZ - Azerbaijan\",\"Id\":\"31\"},{\"Name\":\"BS - Bahamas\",\"Id\":\"44\"},{\"Name\":\"BH - Bahrain\",\"Id\":\"48\"},{\"Name\":\"BD - Bangladesh\",\"Id\":\"50\"},{\"Name\":\"BB - Barbados\",\"Id\":\"52\"},{\"Name\":\"ES - Barcelona\",\"Id\":\"1019\"},{\"Name\":\"BY - Belarus\",\"Id\":\"112\"},{\"Name\":\"BE - Belgium\",\"Id\":\"56\"},{\"Name\":\"BZ - Belize\",\"Id\":\"84\"},{\"Name\":\"BJ - Benin\",\"Id\":\"204\"},{\"Name\":\"BM - Bermuda\",\"Id\":\"60\"},{\"Name\":\"BT - Bhutan\",\"Id\":\"64\"},{\"Name\":\"BO - Bolivia\",\"Id\":\"68\"},{\"Name\":\"IN - Bombay\",\"Id\":\"1009\"},{\"Name\":\"FR - Bordeaux,Strasbourg\",\"Id\":\"1007\"},{\"Name\":\"BA - Bosnia Herzegovina\",\"Id\":\"70\"},{\"Name\":\"US - Boston\",\"Id\":\"1022\"},{\"Name\":\"BW - Botswana\",\"Id\":\"72\"},{\"Name\":\"BV - Bouvet Island\",\"Id\":\"74\"},{\"Name\":\"BR - Brasilia\",\"Id\":\"1044\"},{\"Name\":\"BR - Brazil\",\"Id\":\"76\"},{\"Name\":\"PL - Breslaw\",\"Id\":\"1012\"},{\"Name\":\"IO - British Indian Ocean Terr\",\"Id\":\"86\"},{\"Name\":\"VG - British Virgin Islands\",\"Id\":\"92\"},{\"Name\":\"BN - Brunei Darussalam\",\"Id\":\"96\"},{\"Name\":\"RO - Bucarest\",\"Id\":\"1015\"},{\"Name\":\"BG - Bulgaria\",\"Id\":\"100\"},{\"Name\":\"BF - Burkina Faso\",\"Id\":\"854\"},{\"Name\":\"BU - Burma\",\"Id\":\"104\"},{\"Name\":\"BI - Burundi\",\"Id\":\"108\"},{\"Name\":\"IN - Calcutta\",\"Id\":\"1362\"},{\"Name\":\"KH - Cambodia\",\"Id\":\"116\"},{\"Name\":\"CM - Cameroon\",\"Id\":\"120\"},{\"Name\":\"CA - Canada\",\"Id\":\"124\"},{\"Name\":\"ES - Canary Island\",\"Id\":\"1020\"},{\"Name\":\"AU - Canberra\",\"Id\":\"1201\"},{\"Name\":\"KI - Canton And Enderbury Is.\",\"Id\":\"128\"},{\"Name\":\"CV - Cape Verde\",\"Id\":\"132\"},{\"Name\":\"ZA - Capetown\",\"Id\":\"1124\"},{\"Name\":\"KY - Cayman Islands\",\"Id\":\"136\"},{\"Name\":\"CF - Central African Republic\",\"Id\":\"140\"},{\"Name\":\"TD - Chad\",\"Id\":\"148\"},{\"Name\":\"CN - Chengdu\",\"Id\":\"10096\"},{\"Name\":\"IN - Chennai\",\"Id\":\"10066\"},{\"Name\":\"US - Chicago\",\"Id\":\"1145\"},{\"Name\":\"CL - Chile\",\"Id\":\"152\"},{\"Name\":\"CN - China, People's Republic\",\"Id\":\"156\"},{\"Name\":\"HK - China-Hong Kong\",\"Id\":\"344\"},{\"Name\":\"CX - Christmas Island\",\"Id\":\"162\"},{\"Name\":\"CC - Cocos (Keeling) Island\",\"Id\":\"166\"},{\"Name\":\"CO - Colombia\",\"Id\":\"170\"},{\"Name\":\"KM - Comoros\",\"Id\":\"174\"},{\"Name\":\"CG - Congo\",\"Id\":\"178\"},{\"Name\":\"CK - Cook Islands\",\"Id\":\"184\"},{\"Name\":\"CR - Costa Rica\",\"Id\":\"188\"},{\"Name\":\"CI - Cote D'Ivoire\",\"Id\":\"384\"},{\"Name\":\"HR - Croatia\",\"Id\":\"191\"},{\"Name\":\"CU - Cuba\",\"Id\":\"192\"},{\"Name\":\"CU - Cuba (CUC)\",\"Id\":\"193\"},{\"Name\":\"CY - Cyprus\",\"Id\":\"196\"},{\"Name\":\"CZ - Czech Republic\",\"Id\":\"203\"},{\"Name\":\"CD - Democratic Republic of Congo\",\"Id\":\"180\"},{\"Name\":\"YE - Democratic Yemen\",\"Id\":\"720\"},{\"Name\":\"DK - Denmark\",\"Id\":\"208\"},{\"Name\":\"DJ - Djibouti\",\"Id\":\"262\"},{\"Name\":\"DM - Dominica\",\"Id\":\"212\"},{\"Name\":\"NQ - Dronning Maud Land\",\"Id\":\"216\"},{\"Name\":\"TP - East Timor\",\"Id\":\"626\"},{\"Name\":\"EC - Ecuador\",\"Id\":\"218\"},{\"Name\":\"EG - Egypt\",\"Id\":\"818\"},{\"Name\":\"SV - El Salvador\",\"Id\":\"222\"},{\"Name\":\"GQ - Equatorial Guinea\",\"Id\":\"226\"},{\"Name\":\"ER - Eriteria\",\"Id\":\"291\"},{\"Name\":\"EE - Estonia\",\"Id\":\"233\"},{\"Name\":\"ET - Ethiopia\",\"Id\":\"230\"},{\"Name\":\"FO - Faeroe Islands\",\"Id\":\"234\"},{\"Name\":\"FK - Falkland Islands-Malvinas\",\"Id\":\"238\"},{\"Name\":\"FJ - Fiji\",\"Id\":\"242\"},{\"Name\":\"FI - Finland\",\"Id\":\"246\"},{\"Name\":\"FR - France\",\"Id\":\"250\"},{\"Name\":\"GF - French Guiana\",\"Id\":\"254\"},{\"Name\":\"PF - French Polynesia\",\"Id\":\"258\"},{\"Name\":\"TF - French Southern Terr\",\"Id\":\"260\"},{\"Name\":\"GA - Gabon\",\"Id\":\"266\"},{\"Name\":\"GM - Gambia\",\"Id\":\"270\"},{\"Name\":\"PL - Gdansk\",\"Id\":\"1357\"},{\"Name\":\"CH - Geneva\",\"Id\":\"10080\"},{\"Name\":\"GE - Georgia\",\"Id\":\"268\"},{\"Name\":\"DE - Germany\",\"Id\":\"280\"},{\"Name\":\"GH - Ghana\",\"Id\":\"288\"},{\"Name\":\"GI - Gibraltar\",\"Id\":\"292\"},{\"Name\":\"GR - Greece\",\"Id\":\"300\"},{\"Name\":\"GL - Greenland\",\"Id\":\"304\"},{\"Name\":\"GD - Grenada\",\"Id\":\"308\"},{\"Name\":\"GP - Guadeloupe\",\"Id\":\"312\"},{\"Name\":\"GU - Guam\",\"Id\":\"316\"},{\"Name\":\"GT - Guatemala\",\"Id\":\"320\"},{\"Name\":\"GG - Guernsey\",\"Id\":\"831\"},{\"Name\":\"GN - Guinea\",\"Id\":\"324\"},{\"Name\":\"GW - Guinea-Bissau\",\"Id\":\"624\"},{\"Name\":\"GY - Guyana\",\"Id\":\"328\"},{\"Name\":\"HT - Haiti\",\"Id\":\"332\"},{\"Name\":\"HM - Heard & Mcdonald Island\",\"Id\":\"334\"},{\"Name\":\"HN - Honduras\",\"Id\":\"340\"},{\"Name\":\"US - Houston\",\"Id\":\"1346\"},{\"Name\":\"HU - Hungary\",\"Id\":\"348\"},{\"Name\":\"IS - Iceland\",\"Id\":\"352\"},{\"Name\":\"IN - India\",\"Id\":\"356\"},{\"Name\":\"ID - Indonesia\",\"Id\":\"360\"},{\"Name\":\"IR - Iran\",\"Id\":\"364\"},{\"Name\":\"IQ - Iraq\",\"Id\":\"368\"},{\"Name\":\"IE - Ireland\",\"Id\":\"372\"},{\"Name\":\"PK - Islamabad\",\"Id\":\"1110\"},{\"Name\":\"IL - Israel\",\"Id\":\"376\"},{\"Name\":\"TR - Istanbul\",\"Id\":\"1139\"},{\"Name\":\"IT - Italy\",\"Id\":\"380\"},{\"Name\":\"TR - Izmir\",\"Id\":\"1325\"},{\"Name\":\"JM - Jamaica\",\"Id\":\"388\"},{\"Name\":\"JP - Japan\",\"Id\":\"392\"},{\"Name\":\"SA - Jeddah\",\"Id\":\"10075\"},{\"Name\":\"JE - Jersey\",\"Id\":\"832\"},{\"Name\":\"ZA - Johannesburg\",\"Id\":\"1125\"},{\"Name\":\"JT - Johnston Island\",\"Id\":\"396\"},{\"Name\":\"JO - Jordan\",\"Id\":\"400\"},{\"Name\":\"KZ - Kazakhstan\",\"Id\":\"398\"},{\"Name\":\"KE - Kenya\",\"Id\":\"404\"},{\"Name\":\"KI - Kiribati\",\"Id\":\"296\"},{\"Name\":\"CS - Kosovo\",\"Id\":\"411\"},{\"Name\":\"PL - Krakau\",\"Id\":\"1358\"},{\"Name\":\"KW - Kuwait\",\"Id\":\"414\"},{\"Name\":\"KG - Kyrgyzstan\",\"Id\":\"417\"},{\"Name\":\"LA - Lao People'S Dem. Rep.\",\"Id\":\"418\"},{\"Name\":\"LV - Latvia\",\"Id\":\"428\"},{\"Name\":\"LB - Lebanon\",\"Id\":\"422\"},{\"Name\":\"LS - Lesotho\",\"Id\":\"426\"},{\"Name\":\"LR - Liberia\",\"Id\":\"430\"},{\"Name\":\"LY - Libyan Arab Jamahiriya\",\"Id\":\"434\"},{\"Name\":\"LI - Liechtenstein\",\"Id\":\"438\"},{\"Name\":\"PT - Lisbon\",\"Id\":\"1014\"},{\"Name\":\"LT - Lithuania\",\"Id\":\"440\"},{\"Name\":\"GB - London\",\"Id\":\"1024\"},{\"Name\":\"US - Los Angeles\",\"Id\":\"1146\"},{\"Name\":\"LU - Luxembourg\",\"Id\":\"442\"},{\"Name\":\"FR - Lyon\",\"Id\":\"1008\"},{\"Name\":\"MO - Macau\",\"Id\":\"446\"},{\"Name\":\"MK - Macedonia\",\"Id\":\"807\"},{\"Name\":\"MG - Madagascar\",\"Id\":\"450\"},{\"Name\":\"ES - Madrid\",\"Id\":\"1126\"},{\"Name\":\"MW - Malawi\",\"Id\":\"454\"},{\"Name\":\"MY - Malaysia\",\"Id\":\"458\"},{\"Name\":\"MV - Maldives\",\"Id\":\"462\"},{\"Name\":\"ML - Mali\",\"Id\":\"466\"},{\"Name\":\"MT - Malta\",\"Id\":\"470\"},{\"Name\":\"IM - Man\",\"Id\":\"833\"},{\"Name\":\"FR - Marseille\",\"Id\":\"2037\"},{\"Name\":\"MH - Marshall Islands\",\"Id\":\"897\"},{\"Name\":\"MQ - Martinique\",\"Id\":\"474\"},{\"Name\":\"MR - Mauritania\",\"Id\":\"478\"},{\"Name\":\"MU - Mauritius\",\"Id\":\"480\"},{\"Name\":\"YT - Mayotte\",\"Id\":\"898\"},{\"Name\":\"MX - Mexico\",\"Id\":\"484\"},{\"Name\":\"US - Miami\",\"Id\":\"1333\"},{\"Name\":\"FM - Micronesia\",\"Id\":\"899\"},{\"Name\":\"UM - Midway Islands\",\"Id\":\"488\"},{\"Name\":\"IT - Milan\",\"Id\":\"1011\"},{\"Name\":\"MD - Moldova\",\"Id\":\"498\"},{\"Name\":\"MC - Monaco\",\"Id\":\"492\"},{\"Name\":\"MN - Mongolia\",\"Id\":\"496\"},{\"Name\":\"ME - Montenegro\",\"Id\":\"499\"},{\"Name\":\"MS - Montserrat\",\"Id\":\"500\"},{\"Name\":\"MA - Morocco\",\"Id\":\"504\"},{\"Name\":\"RU - Moskow\",\"Id\":\"1016\"},{\"Name\":\"MZ - Mozambique\",\"Id\":\"508\"},{\"Name\":\"MM - Myanmar\",\"Id\":\"900\"},{\"Name\":\"NA - Namibia\",\"Id\":\"516\"},{\"Name\":\"NR - Nauru\",\"Id\":\"520\"},{\"Name\":\"NP - Nepal\",\"Id\":\"524\"},{\"Name\":\"NL - Netherlands\",\"Id\":\"528\"},{\"Name\":\"AN - Netherlands Antilles\",\"Id\":\"532\"},{\"Name\":\"NT - Neutral Zone\",\"Id\":\"536\"},{\"Name\":\"NC - New Caledonia\",\"Id\":\"540\"},{\"Name\":\"IN - New Delhi\",\"Id\":\"1010\"},{\"Name\":\"US - New York\",\"Id\":\"1023\"},{\"Name\":\"NZ - New Zealand\",\"Id\":\"554\"},{\"Name\":\"NI - Nicaragua\",\"Id\":\"558\"},{\"Name\":\"NE - Niger\",\"Id\":\"562\"},{\"Name\":\"NG - Nigeria\",\"Id\":\"566\"},{\"Name\":\"NU - Niue\",\"Id\":\"570\"},{\"Name\":\"NF - Norfolk Island\",\"Id\":\"574\"},{\"Name\":\"KP - North Korea\",\"Id\":\"408\"},{\"Name\":\"MP - Northern Mariana Islands\",\"Id\":\"901\"},{\"Name\":\"NO - Norway\",\"Id\":\"578\"},{\"Name\":\"OM - Oman\",\"Id\":\"512\"},{\"Name\":\"CA - Ottawa\",\"Id\":\"1051\"},{\"Name\":\"OB - Overnight Boat\",\"Id\":\"2216\"},{\"Name\":\"OT - Overnight Flights\",\"Id\":\"998\"},{\"Name\":\"PC - Pacific Is. Trust Terr.\",\"Id\":\"582\"},{\"Name\":\"PK - Pakistan\",\"Id\":\"586\"},{\"Name\":\"PW - Palau\",\"Id\":\"902\"},{\"Name\":\"PS - Palestinian Territories (Occupied)\",\"Id\":\"1151\"},{\"Name\":\"ES - Palma de M\",\"Id\":\"1156\"},{\"Name\":\"PA - Panama\",\"Id\":\"590\"},{\"Name\":\"PG - Papua New Guinea\",\"Id\":\"598\"},{\"Name\":\"PY - Paraguay\",\"Id\":\"600\"},{\"Name\":\"FR - Paris\",\"Id\":\"1006\"},{\"Name\":\"CN - Peking\",\"Id\":\"1003\"},{\"Name\":\"PE - Peru\",\"Id\":\"604\"},{\"Name\":\"PH - Philippines\",\"Id\":\"608\"},{\"Name\":\"PN - Pitcairn\",\"Id\":\"612\"},{\"Name\":\"PL - Poland\",\"Id\":\"616\"},{\"Name\":\"PT - Portugal\",\"Id\":\"620\"},{\"Name\":\"PR - Puerto Rico\",\"Id\":\"630\"},{\"Name\":\"QA - Qatar\",\"Id\":\"634\"},{\"Name\":\"DO - Republica Dominicana\",\"Id\":\"214\"},{\"Name\":\"RE - Reunion\",\"Id\":\"638\"},{\"Name\":\"BR - Rio de Janeiro\",\"Id\":\"1001\"},{\"Name\":\"RO - Romania\",\"Id\":\"642\"},{\"Name\":\"IT - Rome\",\"Id\":\"1080\"},{\"Name\":\"RU - Russia\",\"Id\":\"643\"},{\"Name\":\"RW - Rwanda\",\"Id\":\"646\"},{\"Name\":\"SA - Ryad\",\"Id\":\"1018\"},{\"Name\":\"WS - Samoa\",\"Id\":\"882\"},{\"Name\":\"US - San Francisco\",\"Id\":\"1336\"},{\"Name\":\"SM - San Marino\",\"Id\":\"674\"},{\"Name\":\"BR - Sao Paulo\",\"Id\":\"1002\"},{\"Name\":\"ST - Sao Tome And Principe\",\"Id\":\"678\"},{\"Name\":\"SA - Saudi Arabia\",\"Id\":\"682\"},{\"Name\":\"SN - Senegal\",\"Id\":\"686\"},{\"Name\":\"RS - Serbia\",\"Id\":\"688\"},{\"Name\":\"SC - Seychelles\",\"Id\":\"690\"},{\"Name\":\"CN - Shanghai\",\"Id\":\"1004\"},{\"Name\":\"SL - Sierra Leone\",\"Id\":\"694\"},{\"Name\":\"SG - Singapore\",\"Id\":\"702\"},{\"Name\":\"SK - Slovak Republic\",\"Id\":\"703\"},{\"Name\":\"SI - Slovenia\",\"Id\":\"705\"},{\"Name\":\"GS - So. Georgie & So. Sandwich Is.\",\"Id\":\"903\"},{\"Name\":\"SB - Solomon Islands\",\"Id\":\"90\"},{\"Name\":\"SO - Somalia\",\"Id\":\"706\"},{\"Name\":\"ZA - South Africa\",\"Id\":\"710\"},{\"Name\":\"KR - South Korea\",\"Id\":\"410\"},{\"Name\":\"SS - South Sudan\",\"Id\":\"904\"},{\"Name\":\"ES - Spain\",\"Id\":\"724\"},{\"Name\":\"LK - Sri Lanka\",\"Id\":\"144\"},{\"Name\":\"MF - St Martin\",\"Id\":\"84099\"},{\"Name\":\"RU - St Petersburg\",\"Id\":\"1017\"},{\"Name\":\"SH - St. Helena\",\"Id\":\"654\"},{\"Name\":\"KN - St. Kitts-Nevis-Anguilla\",\"Id\":\"658\"},{\"Name\":\"LC - St. Lucia\",\"Id\":\"662\"},{\"Name\":\"PM - St. Pierre And Miquelon\",\"Id\":\"666\"},{\"Name\":\"VC - St. Vincent & Grenadines\",\"Id\":\"670\"},{\"Name\":\"SD - Sudan\",\"Id\":\"736\"},{\"Name\":\"SR - Suriname\",\"Id\":\"740\"},{\"Name\":\"SJ - Svalbard & Jan Mayen Is.\",\"Id\":\"744\"},{\"Name\":\"SZ - Swaziland\",\"Id\":\"748\"},{\"Name\":\"SE - Sweden\",\"Id\":\"752\"},{\"Name\":\"CH - Switzerland\",\"Id\":\"756\"},{\"Name\":\"AU - Sydney\",\"Id\":\"1203\"},{\"Name\":\"SY - Syrian Arab Republic\",\"Id\":\"760\"},{\"Name\":\"TW - Taiwan\",\"Id\":\"158\"},{\"Name\":\"TJ - Tajikistan\",\"Id\":\"762\"},{\"Name\":\"TZ - Tanzania\",\"Id\":\"834\"},{\"Name\":\"TH - Thailand\",\"Id\":\"764\"},{\"Name\":\"TG - Togo\",\"Id\":\"768\"},{\"Name\":\"TK - Tokelau\",\"Id\":\"772\"},{\"Name\":\"JP - Tokio\",\"Id\":\"1027\"},{\"Name\":\"TO - Tonga\",\"Id\":\"776\"},{\"Name\":\"CA - Toronto\",\"Id\":\"1053\"},{\"Name\":\"TT - Trinidad And Tobago\",\"Id\":\"780\"},{\"Name\":\"TN - Tunisia\",\"Id\":\"788\"},{\"Name\":\"TR - Turkey\",\"Id\":\"792\"},{\"Name\":\"TM - Turkmenistan\",\"Id\":\"795\"},{\"Name\":\"TC - Turks & Caicos Islands\",\"Id\":\"796\"},{\"Name\":\"TV - Tuvalu\",\"Id\":\"798\"},{\"Name\":\"VI - U.S. Virgin Islands\",\"Id\":\"850\"},{\"Name\":\"UG - Uganda\",\"Id\":\"800\"},{\"Name\":\"UA - Ukraine\",\"Id\":\"804\"},{\"Name\":\"AE - United Arab Emirates\",\"Id\":\"784\"},{\"Name\":\"GB - United Kingdom\",\"Id\":\"826\"},{\"Name\":\"UY - Uruguay\",\"Id\":\"858\"},{\"Name\":\"PU - Us Misc. Pacific Islands\",\"Id\":\"849\"},{\"Name\":\"US - USA\",\"Id\":\"840\"},{\"Name\":\"UZ - Uzbekistan\",\"Id\":\"860\"},{\"Name\":\"CA - Vancouver\",\"Id\":\"1224\"},{\"Name\":\"VU - Vanuatu\",\"Id\":\"548\"},{\"Name\":\"VA - Vatican City State\",\"Id\":\"336\"},{\"Name\":\"VE - Venezuela\",\"Id\":\"862\"},{\"Name\":\"VN - Vietnam\",\"Id\":\"704\"},{\"Name\":\"WK - Wake Island\",\"Id\":\"872\"},{\"Name\":\"WF - Wallis And Futuna Islands\",\"Id\":\"876\"},{\"Name\":\"PL - Warsaw\",\"Id\":\"1013\"},{\"Name\":\"US - Washington D.C.\",\"Id\":\"1143\"},{\"Name\":\"EH - Western Sahara\",\"Id\":\"732\"},{\"Name\":\"YE - Yemen Arab Republic\",\"Id\":\"886\"},{\"Name\":\"ZM - Zambia\",\"Id\":\"894\"},{\"Name\":\"ZW - Zimbabwe\",\"Id\":\"716\"}]";
		public static string labels = "{\"1090\":\"Expenses\",\"4607\":\"Cancel\",\"4612\":\"SpendCatcher\",\"4613\":\"The following receipts are being processed. They will be soon available in your expenses list.\",\"4614\":\"Sorry, SpendCatcher feature has not been activated for your company.\\r\\nPlease contact your administrator for more details.\",\"4615\":\"Your picture has been sent to MobileXpense.\",\"4616\":\"Open MobileXpense\"}";
		public static bool authenticated = true;
		public static string token = "96bb4dbf-8a31-4773-94bf-0391826572a0";
		public static string api = "https://staging.mobilexpense.com/net/SpendCatcher/InitiateSpendCatcher";
		public static string image = ImageString.image;

	}
}
