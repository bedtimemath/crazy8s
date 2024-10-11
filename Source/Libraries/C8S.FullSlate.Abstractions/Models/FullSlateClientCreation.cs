using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

// see: https://app.fullslate.com/api/v2/specification#/
[Serializable]
public class FullSlateClientCreation
{
    //first_name*	string
    //example: Johnny
    //Client's first name
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = default!;

    //last_name*	string
    //example: Walker
    //Client's last name
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = default!;

    //birthday	string
    //example: 1990-12-31
    //Client's Birthday.
    [JsonPropertyName("birthday")]
    public string? BirthdayString { get; set; } = null;

    //no_automatic_email	boolean
    //example: true
    //Client who has indicated that they don't want to be contacted by email automatically.
    [JsonPropertyName("no_automatic_email")]
    public bool? NoAutomaticEmail { get; set; } = null;

    //no_sms	boolean
    //example: true
    //a Boolean to indicating to No sms alerts
    [JsonPropertyName("no_sms")]
    public bool? NoSms { get; set; } = null;

    //mass_email_opt_in	boolean
    //example: true
    //Client who has opted in to email communication.
    [JsonPropertyName("mass_email_opt_in")]
    public bool? MassEmailOptIn { get; set; } = null;

    //sms_reminder_consent	boolean
    //example: true
    //a Boolean to indicate opt in to sms reminder
    [JsonPropertyName("sms_reminder_consent")]
    public bool? SmsReminderConsent { get; set; } = null;

    //phone_number	{}
    [JsonPropertyName("phone_number")]
    public FullSlatePhoneNumber? PhoneNumber { get; set; } = null;

    //address	{}
    [JsonPropertyName("phone_number")]
    public FullSlateAddress? Address { get; set; } = null;

    //email	string
    //example: mr.client@example.com
    //Client's email.
    [JsonPropertyName("email")]
    public string? Email { get; set; } = null;

    //time_zone	string
    //Enum: Array [ 151 ]
    //example: Mountain Time (US & Canada)
    //Client's Time Zone. Options: International Date Line West | American Samoa | Midway Island | Hawaii | Alaska |
    //  Pacific Time (US & Canada) | Tijuana | Arizona | Mazatlan | Mountain Time (US & Canada) | Central America |
    //  Central Time (US & Canada) | Chihuahua | Guadalajara | Mexico City | Monterrey | Saskatchewan | Bogota |
    //  Eastern Time (US & Canada) | Indiana (East) | Lima | Quito | Atlantic Time (Canada) | Caracas | Georgetown |
    //  La Paz | Puerto Rico | Santiago | Newfoundland | Brasilia | Buenos Aires | Montevideo | Greenland |
    //  Mid-Atlantic | Azores | Cape Verde Is. | Edinburgh | Lisbon | London | Monrovia | UTC | Amsterdam | Belgrade |
    //  Berlin | Bern | Bratislava | Brussels | Budapest | Casablanca | Copenhagen | Dublin | Ljubljana | Madrid |
    //  Paris | Prague | Rome | Sarajevo | Skopje | Stockholm | Vienna | Warsaw | West Central Africa | Zagreb | Zurich |
    //  Athens | Bucharest | Cairo | Harare | Helsinki | Jerusalem | Kaliningrad | Kyiv | Pretoria | Riga | Sofia |
    //  Tallinn | Vilnius | Baghdad | Istanbul | Kuwait | Minsk | Moscow | Nairobi | Riyadh | St. Petersburg | Volgograd |
    //  Tehran | Abu Dhabi | Baku | Muscat | Samara | Tbilisi | Yerevan | Kabul | Almaty | Ekaterinburg | Islamabad |
    //  Karachi | Tashkent | Chennai | Kolkata | Mumbai | New Delhi | Sri Jayawardenepura | Kathmandu | Astana | Dhaka |
    //  Urumqi | Rangoon | Bangkok | Hanoi | Jakarta | Krasnoyarsk | Novosibirsk | Beijing | Chongqing | Hong Kong |
    //  Irkutsk | Kuala Lumpur | Perth | Singapore | Taipei | Ulaanbaatar | Osaka | Sapporo | Seoul | Tokyo | Yakutsk |
    //  Adelaide | Darwin | Brisbane | Canberra | Guam | Hobart | Melbourne | Port Moresby | Sydney | Vladivostok |
    //  Magadan | New Caledonia | Solomon Is. | Srednekolymsk | Auckland | Fiji | Kamchatka | Marshall Is. | Wellington |
    //  Chatham Is. | Nuku'alofa | Samoa | Tokelau Is.
    [JsonPropertyName("time_zone")]
    public string? TimeZoneString { get; set; } = null;
}