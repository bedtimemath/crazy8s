using System.Text.Json.Serialization;

namespace C8S.FullSlate.Abstractions.Models;

[Serializable]
public class FullSlateAppointment
{
    //id  number
    //example: 12
    //Appointment ID
    [JsonPropertyName("id")]
    public int Id { get; set; }

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

    //occurrence_key	string
    //example: 12_w33_2017
    //A unique key of an appointment.
    [JsonPropertyName("occurrence_key")]
    public string OccurrenceKey { get; set; } = default!;

    //employee	{}
    //description:	
    //The ID and name of the employee who provides the service
    [JsonPropertyName("employee")]
    public FullSlateEmployee Employee { get; set; } = default!;

    //client	{}
    //description:	
    //The client who booked the appointment
    [JsonPropertyName("client")]
    public FullSlateClient Client { get; set; } = default!;

    //services	[{}]
    //description:A list of services that will be provided for this particular appointment
    [JsonPropertyName("services")]
    public List<FullSlateOffering> Offerings { get; set; } = new();

    //client_preferred_employee	boolean
    //example: false
    //True if the employee has been chosen by client. If client_preferred_employee value is not provided
    //  but the employee parameter is provided, then the value for client_preferred_employee will be true.

    //notes	string
    //example: Prepare food for the client
    //The appointment's notes (provided by the company's employee)

    //sequence    number
    //example: 1
    //An integer indicating the version of the event. You can use this value to detect changes in the event

    //custom_fields	[]
    //description: You can include any other custom fields configured by the provider. Custom fields
    //  are required if so configured by the provider. What other information would you like to collect
    //  from clients when they book?

    //pending_reschedule_appointment_id	number
    //example: 19
    //Pending-reschedule Appointment ID (the appointment which need to go through review process).
    //  Condition to have pending_reschedule_appointment_id:
    //      Company enable the 'Review appointment requests and accept or decline on a case-by-case basis'
    //      and the appointment is rescheduling by a client
    //      When updating an accepted appointment, a pending-reschedule appointment will be created.

    //recur_end_at	string
    //example: 2017-12-31
    //For bounded recurring appointment, the date of the last occurrence

    //created_at	string
    //example: 2017-08-07T09:00:00-7:00
    //Record created date

    //split_length	string
    //example: 1800
    //For split events, the length of the split window, in seconds

    //client_notes	string
    //example: Hopefully will get the car repaired by tomorrow
    //Extra details provided by the client. Required if so configured by the provider.

    //tentative	boolean
    //example: true
    //The status of the appointment that need to be review for 'accept' or 'decline'.
    //  True when a client books the appointment and the provider has not accepted or declined the event.
    //  This is only applicable for company that has the setting of "Review appointment requests and accept
    //      or decline on a case-by-case basis"

    //recurrence_mode	string
    //Enum: Array [ 2 ]
    //example: RECURS_WEEKLY
    //The appointment recurrence mode. NONE indicates no recurrence. RECURS_WEEKLY indicates the appointment
    //  recurs weekly. RECURS_ICALENDAR indicates recurrence following rules from an iCalendar integration

    //status  string
    //Enum: Array [ 4 ]
    //example: STATUS_BOOKED
    //Current status of the appointment.
    //Options: STATUS_NO_SHOW | STATUS_CHECKED_IN | STATUS_COMPLETE | STATUS_BOOKED

    //recurrence_exceptions	[string]
    //description:The list of dates that are excluded from the recurrence event.
    //example:List [ "2017-08-22" ]

    //recurrence_interval	number
    //example: 1
    //A number that indicate the appointment's recur-interval in weeks. 1 indicates recurs every 1 week,
    //  2 indicates recurs every 2 week and so on

    //duration	string
    //example: 01:00:00
    //The appointment's duration in hours:minutes:seconds

    //updated_at	string
    //example: 2017-08-07T09:00:00-7:00
    //Record last updated date

    //global_id	string
    //example: imCAfJnYF8S5nWkZtH3kN19ytr@fullslate.com
    //The global ID of the appointment used in iCalendar feeds

    //deletion_message_from_client	boolean
    //example: true
    //Boolean value that indicates there is deletion message from client.

    //master_appointment_id	number
    //example: 12
    //The original appointment ID, that is for the pending-reschedule appointment to know its original
    //  appointment ID

    //deleted	string
    //example: false
    //A boolean value that indicates if the record is deleted.

    //api_options	{}
    //description:	
    //The custom options/infos this appointment has, eg. to mark appointments that come from paid search.
    //  For example: { "paid" => true }

    //deleted_at  string
    //example: 2017-10-13T08:00:00-7:00
    //The appointment's cancelled date. If this value is not empty, indicates this record is deleted.

    //to	string
    //example: 2017-08-15T12:00:00-07:00
    //The end date and time of the appointment's first occurrence. The format of the date and time will
    //  be displayed in the Company's timezone.

    //created_by_client	boolean
    //example: true
    //This value will be true if the appointment is created by client, i.e. when you have provided
    //  user_type = 'CLIENT' during appointment creation through API V2.

    //buffer_after	number
    //example: 600
    //The cleanup time of the appointment, in seconds

    //split_start	string
    //example: 1800
    //For split events, the beginning of the split window, in seconds

    //promo_code	string
    //example: HAPPYREPAIR
    //Promotional code to the appointment. Required if so configured by the provider

    //buffer_before	number
    //example: 600
    //The setup time of the appointment, in seconds

    //confirmed	boolean
    //example: true
    //A boolean of appointment confirmed status

}