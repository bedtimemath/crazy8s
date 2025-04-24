namespace SC.Common.Base;

public interface ICoreDb: IBaseDb
{
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset? ModifiedOn { get; set; }
}