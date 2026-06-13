using System;
using System.Collections.Generic;

namespace BoolderDataMigration.Models;

public partial class Circuit
{
    public int Id { get; set; }

    public string Color { get; set; } = null!;

    public string AverageGrade { get; set; } = null!;

    public int BeginnerFriendly { get; set; }

    public int Dangerous { get; set; }

    public double SouthWestLat { get; set; }

    public double SouthWestLon { get; set; }

    public double NorthEastLat { get; set; }

    public double NorthEastLon { get; set; }
}
