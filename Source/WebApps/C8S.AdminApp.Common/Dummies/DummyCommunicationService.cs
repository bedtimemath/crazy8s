﻿using C8S.AdminApp.Common.Interfaces;
using SC.Audit.Abstractions.Models;

namespace C8S.AdminApp.Common.Dummies;

public class DummyCommunicationService: ICommunicationService
{
    public event EventHandler<DataChangedEventArgs>? DataChanged;
    public Task InitializeAsync() => Task.CompletedTask;
}