using System;
using System.Collections.Generic;

namespace BoolderDataMigration.Models;

public partial class PoiRoute
{
    public int Id { get; set; }

    public int AreaId { get; set; }

    public int PoiId { get; set; }

    public int DistanceInMinutes { get; set; }

    public string Transport { get; set; } = null!;
}
