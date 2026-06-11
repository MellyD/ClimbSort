using System;
using System.Collections.Generic;

namespace BoolderDataMigration.Models;

public partial class Problem
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? NameEn { get; set; }

    public string? NameSearchable { get; set; }

    public string? Grade { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int? CircuitId { get; set; }

    public string? CircuitNumber { get; set; }

    public string? CircuitColor { get; set; }

    public string Steepness { get; set; } = null!;

    public int SitStart { get; set; }

    public int AreaId { get; set; }

    public string? BleauInfoId { get; set; }

    public int Featured { get; set; }

    public int? Popularity { get; set; }

    public int? ParentId { get; set; }
}
