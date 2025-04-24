namespace SC.Common.Helpers.CQRS.Interfaces;

public interface ICQRSCommand
{
}

public interface ICQRSCommand<TResponse>
    where TResponse: class
{
}