﻿using C8S.Domain.Models;

namespace C8S.Domain.Queries;

public abstract class DomainListResults<TModel>
    where TModel : class
{
    public List<TModel> Items { get; set; } = [];
    public int Total { get; set; }
}

public class RequestListResults : DomainListResults<RequestListDisplay>;