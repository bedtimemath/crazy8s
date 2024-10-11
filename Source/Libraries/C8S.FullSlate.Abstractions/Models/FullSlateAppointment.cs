using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateAppointment
{
    //api_options	{}
    //description:	
    //The custom options/infos this appointment has, eg. to mark appointments that come from paid search.
    //  For example: { "paid" => true }
    [JsonPropertyName("api_options")]
    public object? ApiOptions { get; set; } = null;

    //at	string
    //example: 2017-08-15T11:00:00-07:00
    //The date and time at which the appointment first occurs. The format of the date and time will be
    //  displayed in the Company's timezone.
    [JsonPropertyName("at")]
    public string? AtDateTimeString { get; set; } = default!;

    [JsonIgnore]
    public DateTimeOffset? AtDateTime
    {
        get => String.IsNullOrEmpty(AtDateTimeString) ? null : DateTimeOffset.Parse(AtDateTimeString); 
        set => AtDateTimeString = value?.ToString("O");
    }

    //buffer_after	number
    //example: 600
    //The cleanup time of the appointment, in seconds
    [JsonPropertyName("buffer_after")]
    public int? BufferAfter { get; set; } = null;

    //buffer_before	number
    //example: 600
    //The setup time of the appointment, in seconds
    [JsonPropertyName("buffer_before")]
    public int? BufferBefore { get; set; } = null;

    //client	{}
    //description:	
    //The client who booked the appointment
    [JsonPropertyName("client")]
    public FullSlateAppointmentClient Client { get; set; } = default!;

    //client_notes	string
    //example: Hopefully will get the car repaired by tomorrow
    //Extra details provided by the client. Required if so configured by the provider.
    [JsonPropertyName("client_notes")]
    public string? ClientNotes { get; set; } = null;

    //client_preferred_employee	boolean
    //example: false
    //True if the employee has been chosen by client. If client_preferred_employee value is not provided
    //  but the employee parameter is provided, then the value for client_preferred_employee will be true.
    [JsonPropertyName("client_preferred_employee")]
    public bool? ClientPreferredEmployee { get; set; } = null;

    //confirmed	boolean
    //example: true
    //A boolean of appointment confirmed status
    [JsonPropertyName("confirmed")]
    public bool? Confirmed { get; set; } = null;

    //created_at	string
    //example: 2017-08-07T09:00:00-7:00
    //Record created date
    [JsonPropertyName("created_at")]
    public string? CreatedAtDateTimeString { get; set; } = null;

    [JsonIgnore]
    public DateTimeOffset? CreatedAt
    {
        get => String.IsNullOrEmpty(CreatedAtDateTimeString) ? null : DateTimeOffset.Parse(CreatedAtDateTimeString); 
        set => CreatedAtDateTimeString = value?.ToString("O");
    }

    //created_by_client	boolean
    //example: true
    //This value will be true if the appointment is created by client, i.e. when you have provided
    //  user_type = 'CLIENT' during appointment creation through API V2.
    [JsonPropertyName("created_by_client")]
    public bool? CreatedByClient { get; set; } = null;

    //custom_fields	[]
    //description: You can include any other custom fields configured by the provider. Custom fields
    //  are required if so configured by the provider. What other information would you like to collect
    //  from clients when they book?
    [JsonPropertyName("custom_fields")]
    public List<FullSlateCustom> CustomFields { get; set; } = [];

    //deleted	string
    //example: false
    //A boolean value that indicates if the record is deleted.
    [JsonPropertyName("deleted")]
    public string? DeletedString { get; set; } = null;

    //deleted_at  string
    //example: 2017-10-13T08:00:00-7:00
    //The appointment's cancelled date. If this value is not empty, indicates this record is deleted.
    [JsonPropertyName("deleted_at")]
    public string? DeletedAtDateTimeString { get; set; } = null;

    [JsonIgnore]
    public DateTimeOffset? DeletedAt
    {
        get => String.IsNullOrEmpty(DeletedAtDateTimeString) ? null : DateTimeOffset.Parse(DeletedAtDateTimeString); 
        set => DeletedAtDateTimeString = value?.ToString("O");
    }

    //deletion_message_from_client	boolean
    //example: true
    //Boolean value that indicates there is deletion message from client.
    [JsonPropertyName("deletion_message_from_client")]
    public bool? DeletionMessageFromClient { get; set; } = null;

    //duration	string
    //example: 01:00:00
    //The appointment's duration in hours:minutes:seconds
    [JsonPropertyName("duration")]
    public string? DurationString { get; set; } = null;

    //employee	{}
    //description:	
    //The ID and name of the employee who provides the service
    [JsonPropertyName("employee")]
    public FullSlateEmployee Employee { get; set; } = default!;

    //global_id	string
    //example: imCAfJnYF8S5nWkZtH3kN19ytr@fullslate.com
    //The global ID of the appointment used in iCalendar feeds
    [JsonPropertyName("global_id")]
    public string? GlobalId { get; set; } = null;

    //id  number
    //example: 12
    //Appointment ID
    [JsonPropertyName("id")]
    public int Id { get; set; }

    //master_appointment_id	number
    //example: 12
    //The original appointment ID, that is for the pending-reschedule appointment to know its original
    //  appointment ID
    [JsonPropertyName("master_appointment_id")]
    public int? MasterAppointmentId { get; set; }

    //notes	string
    //example: Prepare food for the client
    //The appointment's notes (provided by the company's employee)
    [JsonPropertyName("notes")]
    public string? Notes { get; set; } = null;

    //occurrence_key	string
    //example: 12_w33_2017
    //A unique key of an appointment.
    [JsonPropertyName("occurrence_key")]
    public string OccurrenceKey { get; set; } = default!;

    //pending_reschedule_appointment_id	number
    //example: 19
    //Pending-reschedule Appointment ID (the appointment which need to go through review process).
    //  Condition to have pending_reschedule_appointment_id:
    //      Company enable the 'Review appointment requests and accept or decline on a case-by-case basis'
    //      and the appointment is rescheduling by a client
    //      When updating an accepted appointment, a pending-reschedule appointment will be created.
    [JsonPropertyName("pending_reschedule_appointment_id")]
    public int? PendingRescheduleAppointmentId { get; set; }

    //promo_code	string
    //example: HAPPYREPAIR
    //Promotional code to the appointment. Required if so configured by the provider
    [JsonPropertyName("promo_code")]
    public string? PromoCode { get; set; } = null;

    //recur_end_at	string
    //example: 2017-12-31
    //For bounded recurring appointment, the date of the last occurrence
    [JsonPropertyName("recur_end_at")]
    public string? RecurEndAtString { get; set; } = null;

    //recurrence_exceptions	[string]
    //description:The list of dates that are excluded from the recurrence event.
    //example:List [ "2017-08-22" ]
    [JsonPropertyName("recurrence_exceptions")]
    public List<string>? RecurrenceExceptions{ get; set; } = null;

    //recurrence_interval	number
    //example: 1
    //A number that indicate the appointment's recur-interval in weeks. 1 indicates recurs every 1 week,
    //  2 indicates recurs every 2 week and so on
    [JsonPropertyName("recurrence_interval")]
    public int RecurrenceInterval { get; set; } = default!;

    //recurrence_mode	string
    //Enum: Array [ 2 ]
    //example: RECURS_WEEKLY
    //The appointment recurrence mode. NONE indicates no recurrence. RECURS_WEEKLY indicates the appointment
    //  recurs weekly. RECURS_ICALENDAR indicates recurrence following rules from an iCalendar integration
    [JsonPropertyName("recurrence_mode")]
    public string? RecurrenceModeString { get; set; } = null;

    //sequence    number
    //example: 1
    //An integer indicating the version of the event. You can use this value to detect changes in the event
    [JsonPropertyName("sequence")]
    public int? Sequence { get; set; }

    //services	[{}]
    //description:A list of services that will be provided for this particular appointment
    [JsonPropertyName("services")]
    public List<FullSlateOffering> Offerings { get; set; } = new();

    //split_length	string
    //example: 1800
    //For split events, the length of the split window, in seconds
    [JsonPropertyName("split_length")]
    public string? SplitLength { get; set; } = null;

    //split_start	string
    //example: 1800
    //For split events, the beginning of the split window, in seconds
    [JsonPropertyName("split_start")]
    public string? SplitStart { get; set; } = null;

    //status  string
    //Enum: Array [ 4 ]
    //example: STATUS_BOOKED
    //Current status of the appointment.
    //Options: STATUS_NO_SHOW | STATUS_CHECKED_IN | STATUS_COMPLETE | STATUS_BOOKED
    [JsonPropertyName("status")]
    public string? StatusString { get; set; } = null;

    //tentative	boolean
    //example: true
    //The status of the appointment that need to be review for 'accept' or 'decline'.
    //  True when a client books the appointment and the provider has not accepted or declined the event.
    //  This is only applicable for company that has the setting of "Review appointment requests and accept
    //      or decline on a case-by-case basis"
    [JsonPropertyName("tentative")]
    public bool? Tentative { get; set; } = null;

    //to	string
    //example: 2017-08-15T12:00:00-07:00
    //The end date and time of the appointment's first occurrence. The format of the date and time will
    //  be displayed in the Company's timezone.
    [JsonPropertyName("to")]
    public string? ToDateTimeString { get; set; } = null;

    [JsonIgnore]
    public DateTimeOffset? To
    {
        get => String.IsNullOrEmpty(ToDateTimeString) ? null : DateTimeOffset.Parse(ToDateTimeString); 
        set => ToDateTimeString = value?.ToString("O");
    }

    //updated_at	string
    //example: 2017-08-07T09:00:00-7:00
    //Record last updated date
    [JsonPropertyName("updated_at")]
    public string? UpdatedAtDateTimeString { get; set; } = null;

    [JsonIgnore]
    public DateTimeOffset? UpdatedAt
    {
        get => String.IsNullOrEmpty(UpdatedAtDateTimeString) ? null : DateTimeOffset.Parse(UpdatedAtDateTimeString); 
        set => UpdatedAtDateTimeString = value?.ToString("O");
    }
}