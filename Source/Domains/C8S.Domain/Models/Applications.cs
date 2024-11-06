using C8S.Domain.Enums;

namespace C8S.Domain.Models;

public class ApplicationBase
{
    #region Id Property
    public int ApplicationId { get; set; }
    public ApplicationStatus Status { get; set; }
    public string ApplicantLastName { get; set; } = default!;
    public string ApplicantEmail { get; set; } = default!;
    #endregion
}