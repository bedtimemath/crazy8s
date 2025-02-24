using C8S.Fulco.Abstractions.Models;

namespace C8S.Fulco.Services;

public class FulcoService
{
    public Task<FulcoInvoice> GetInvoice(int invoiceId) =>
        throw new NotImplementedException();
    public Task<IEnumerable<FulcoInvoice>> GetInvoices(DateOnly startDate, DateOnly endDate) =>
        throw new NotImplementedException();
}