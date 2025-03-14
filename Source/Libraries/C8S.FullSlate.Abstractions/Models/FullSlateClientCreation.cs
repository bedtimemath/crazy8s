﻿using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

// see: https://app.fullslate.com/api/v2/specification#/
[Serializable]
public class FullSlateClientCreation
{
    //active	boolean
    //example: true
    //Boolean indicating whether the service provider considers the client to be active; Typically set to false when the
    //  client is no longer expected to make appointments
    [JsonPropertyName("active")]
    public bool? Active { get; set; } = null;

    //addresses	{}
    [JsonPropertyName("addresses")]
    public List<FullSlateAddress>? Addresses { get; set; } = null;

    //birthday	string
    //example: 1990-12-31
    //Client's Birthday.
    [JsonPropertyName("birthday")]
    public string? BirthdayString { get; set; } = null;

    //client_since	string
    //example: 2016-12-31
    //Show only clients who started on this date.
    [JsonPropertyName("client_since")]
    public string? ClientSinceDateTimeString { get; set; } = default!;

    [JsonIgnore]
    public DateTimeOffset? ClientSince
    {
        get => String.IsNullOrEmpty(ClientSinceDateTimeString) ? null : DateTimeOffset.Parse(ClientSinceDateTimeString);
        set => ClientSinceDateTimeString = value?.ToString("O");
    }

    //company_name_ext	string
    //example: Comp Ext
    //Company Name Ext
    [JsonPropertyName("company_name_ext")]
    public string? CompanyNameExt { get; set; } = null;

    //email_cc	[number]
    //description: A list of client ID(s) that associated with this client. When an email is send to the client, will
    //  also CC to these clients' emails.
    //example:List [ 12, 15 ]
    [JsonPropertyName("email_cc")]
    public List<int>? EmailCcIds { get; set; } = null;

    //emails	[string]
    //description:List of client's emails. The first position will be the primary email.
    //example:List [ "client1@example.com", "client2@example.com" ]
    [JsonPropertyName("emails")]
    public List<string>? Emails { get; set; } = null;

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

    //mass_email_opt_in	boolean
    //example: true
    //Client who has opted in to email communication.
    [JsonPropertyName("mass_email_opt_in")]
    public bool? MassEmailOptIn { get; set; } = null;

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

    //notes	string
    //example: Prefers to book appointments in the morning
    //Service provider notes, if any (HTML)
    [JsonPropertyName("notes")]
    public string? Notes { get; set; } = null;

    //phone_numbers	{}
    [JsonPropertyName("phone_numbers")]
    public List<FullSlatePhoneNumber>? PhoneNumbers { get; set; } = null;

    //referrer	string
    //example: Referrer
    //Referrer
    [JsonPropertyName("referrer")]
    public string? Referrer { get; set; } = null;

    //sms_reminder_consent	boolean
    //example: true
    //a Boolean to indicate opt in to sms reminder
    [JsonPropertyName("sms_reminder_consent")]
    public bool? SmsReminderConsent { get; set; } = null;

    //source	string
    //example: Source
    //Source
    [JsonPropertyName("source")]
    public string? Source { get; set; } = null;

    //suffix	string
    //example: Jr
    //Suffix
    [JsonPropertyName("suffix")]
    public string? Suffix { get; set; } = null;

    //tags	[string]
    //description:List of Tags
    //example:List [ "car", "truck" ]
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; } = null;

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