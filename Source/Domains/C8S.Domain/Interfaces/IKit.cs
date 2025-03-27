using C8S.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8S.Domain.Interfaces;

public interface IKit
{
    public string Year { get; set; }
    public int Season { get; set; }
    public AgeLevel AgeLevel { get; set; }
    public string? Version { get; set; }
}