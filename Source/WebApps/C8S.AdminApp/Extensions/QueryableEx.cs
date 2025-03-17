using System.Linq.Dynamic.Core;
using C8S.Domain.EFCore.Models;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Extensions;

public static class QueryableEx
{
    public static IQueryable<PersonDb> FilterSortPersonsQueryable(
        this IQueryable<PersonDb> queryable, IListQuery query)
    {
        /*** QUERY ***/
        if (!String.IsNullOrEmpty(query.Query))
        {
            queryable = queryable
                .Where(r => (!String.IsNullOrEmpty(r.FirstName) && 
                             r.FirstName.Contains(query.Query)) ||
                            r.LastName.Contains(query.Query) ||
                            (!String.IsNullOrEmpty(r.Email) && 
                             r.Email.Contains(query.Query)));
        }

        /*** SORT ***/
        if (!String.IsNullOrEmpty(query.SortDescription))
            queryable = queryable.OrderBy(query.SortDescription);
        return queryable;
    }

    public static IQueryable<PersonDb> SkipAndTakePersonsQueryable(
        this IQueryable<PersonDb> queryable, IListQuery query)
    {
        if (query.StartIndex != null) queryable = queryable.Skip(query.StartIndex.Value);
        if (query.Count != null) queryable = queryable.Take(query.Count.Value);

        return queryable;
    }
}