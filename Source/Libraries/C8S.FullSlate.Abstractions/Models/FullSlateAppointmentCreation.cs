using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

// see: https://app.fullslate.com/api/v2/specification#/
[Serializable]
public class FullSlateAppointmentCreation
{
    //at*	string
    //example: 2017-08-15T11:00:00-07:00
    //The service's date and time of the opening to book. The appointment can only be created in time-slots listed in /openings endpoint.
    [JsonPropertyName("at")]
    public string AtDateTimeString { get; set; } = default!;

    [JsonIgnore]
    public DateTimeOffset AtDateTime
    {
        get => DateTimeOffset.Parse(AtDateTimeString); 
        set => AtDateTimeString = value.ToString("O");
    }

    //to	string
    //example: 2017-08-15T12:00:00-07:00
    //This is an optional field, an end time for an appointment. This field is NOT required for user_type = 'CLIENT'. If to value is not
    //provided, we will set based on the service provided.
    [JsonPropertyName("to")]
    public string? ToDateTimeString { get; set; } = null;

    //services*	[number]
    //description:One or more service IDs dictating the set of services to book.
    //example:List [ 2 ]
    [JsonPropertyName("services")]
    public List<int> Services { get; set; } = [];

    //employee	number
    //example: 11
    //The ID of the employee to book, which you can get it from the /api/v2/employees api endpoint. For user_type = "CLIENT", if you omit
    //this parameter, Full Slate will select an employee automatically from those available at the selected time. For user_type =
    //"BUSINESS_USER", this field is a required field.
    [JsonPropertyName("employee")]
    public int Employee { get; set; } = default!;

    //location_id	number
    //example: 3
    //Location ID
    [JsonPropertyName("location_id")]
    public int LocationId { get; set; } = default!;

    //custom_fields	[]
    //description: You can include any other custom fields configured by the provider. Custom fields are required if so configured by
    //the provider. What other information would you like to collect from clients when they book?
    [JsonPropertyName("custom_fields")]
    public List<FullSlateCustom> CustomFields { get; set; } = [];

    //client_with_creation	{
    //    description:
    //    The Client detail. You can only choose to create or update an appointment by providing client information with either
    //      client_with_creation or client parameter. Full Slate will first find the closest matched client. If a closest matched
    //      client is found, we will use the found client to create or update appointment. Besides, we will also update the client
    //      detail if we found some differences in the phone_number, email and/or address based on the information you provided in
    //      client_with_creation parameter. If such record is not found, then Full Slate will create a new client record to create
    //      or update an appointment.
    // }
    [JsonPropertyName("client_with_creation")]
    public FullSlateClientCreation? ClientCreation { get; set; } = null;

    //client	number
    //example: 8
    //An existing client ID, which you can get it from /api/v2/clients API endpoint. You can only choose to create an appointment
    //  by providing client information with either client_with_creation or client parameter.
    [JsonPropertyName("client")]
    public int Client { get; set; } = default!;

    //recurrence_mode	string
    //Enum: Array [ 2 ]
    //example: RECURS_WEEKLY
    //The appointment recurrence mode. NONE indicates no recurrence. RECURS_WEEKLY indicates the appointment recurs weekly.
    //RECURS_ICALENDAR indicates recurrence following rules from an iCalendar integration
    [JsonPropertyName("recurrence_mode")]
    public string? RecurrenceModeString { get; set; } = null;

    //recurrence_interval	number
    //example: 1
    //A number that indicate the appointment's recur-interval in weeks. 1 indicates recurs every 1 week, 2 indicates recurs every
    //2 week and so on
    [JsonPropertyName("recurrence_interval")]
    public int RecurrenceInterval { get; set; } = default!;

    //recur_end_at	string
    //example: 2017-12-31
    //For bounded recurring appointment, the date of the last occurrence
    [JsonPropertyName("recur_end_at")]
    public string? RecurEndAtString { get; set; } = null;

    //notes	string
    //example: Prepare food for the client
    //The appointment's notes (provided by the company's employee)
    [JsonPropertyName("notes")]
    public string? Notes { get; set; } = null;

    //client_notes	string
    //example: Hopefully will get the car repaired by tomorrow
    //Extra details provided by the client. Required if so configured by the provider.
    [JsonPropertyName("client_notes")]
    public string? ClientNotes { get; set; } = null;

    //promo_code	string
    //example: HAPPYREPAIR
    //Promotional code to the appointment. Required if so configured by the provider
    [JsonPropertyName("promo_code")]
    public string? PromoCode { get; set; } = null;

    //status	string
    //Enum: Array [ 4 ]
    //example: STATUS_BOOKED
    //Current status of the appointment. Options: STATUS_NO_SHOW | STATUS_CHECKED_IN | STATUS_COMPLETE | STATUS_BOOKED
    [JsonPropertyName("status")]
    public string? StatusString { get; set; } = null;

    //api_options	{
    //    description:
    //    The custom options/infos this appointment has, eg. to mark appointments that come from paid search.
    //      For example: { "paid" => true }
    //}
    [JsonPropertyName("api_options")]
    public object? ApiOptions { get; set; } = null;

    //client_preferred_employee	boolean
    //example: false
    //True if the employee has been chosen by client. If client_preferred_employee value is not provided but the employee parameter
    //  is provided, then the value for client_preferred_employee will be true.
    [JsonPropertyName("client_preferred_employee")]
    public bool? ClientPreferredEmployee { get; set; } = null;

    //confirmed	boolean
    //example: true
    //A boolean of appointment confirmed status
    [JsonPropertyName("confirmed")]
    public bool? Confirmed { get; set; } = null;

    //send_client_confirmation_email	boolean
    //example: true
    //Whether or not to send an appointment confirmation email to the client. Defaulted to a boolean value of true, which means
    //  to send such confirmation email. However, if the company has set the booking rules to 'Review appointment requests and
    //  accept or decline on a case-by-case basis', then this param will be ignored, and we will always send the email to the
    //  client regarding their appointment is under review status.
    [JsonPropertyName("send_client_confirmation_email")]
    public bool? SendClientConfirmationEmail { get; set; } = null;

    //send_employee_notification_email	boolean
    //example: true
    //Whether or not to send an appointment notification email to the employee. Defaulted to a boolean value of true, which means
    //  to send such notification email. However, if the company has set the booking rules to 'Review appointment requests and accept
    //  or decline on a case-by-case basis', then this param will be ignored, and we will always send the notification email to the
    //  employee to inform them that the appointment needs to be reviewed.
    [JsonPropertyName("send_employee_notification_email")]
    public bool? SendClientNotificationEmail { get; set; } = null;

    //user_type	string
    //Enum: Array [ 2 ]
    //example: BUSINESS_USER
    //Options: 'CLIENT' or 'BUSINESS_USER'. 'CLIENT' - We will apply the company's openings-related booking rules to search for
    //  an opening in order to create, update or cancel an appointment. 'BUSINESS_USER' - It will be like an company's admin or
    //  employee usage, so we will NOT apply any company's openings-related booking rules to search for an opening to create, update
    //  or cancel an appointment. Defaulted to 'CLIENT' if this parameter is omitted.
    [JsonPropertyName("user_type")]
    public string? UserTypeString { get; set; } = null;

    //passphrase	string
    //example: PASSCODE11
    //This is the passphrase set by the company to allow appointment creation
    [JsonPropertyName("passphrase")]
    public string? Passphrase { get; set; } = null;
}