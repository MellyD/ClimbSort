using System;
using System.Collections.Generic;

namespace BoolderDataMigration.Models;

public partial class Poi
{
    public int Id { get; set; }

    public string PoiType { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string GoogleUrl { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}
