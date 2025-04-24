namespace SC.Common.Helpers.CQRS.Interfaces;

public interface ICQRSCommandHandler<in TCommand>
    where TCommand: class, ICQRSCommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}
public interface ICQRSCommandHandler<in TCommand, TResponse>
    where TCommand: class, ICQRSCommand<TResponse>
    where TResponse: class
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
}