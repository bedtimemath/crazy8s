using SC.Common.Models;

namespace C8S.Applications.Models;

public class ProcessApplicationsResponse
{
    public int TotalProcessed { get; set; }
    public int TotalSuccessful { get; set; }
    public List<SerializableException> Errors { get; set; } = new();
}
