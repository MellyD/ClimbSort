using System;
using System.Collections.Generic;

namespace BoolderDataMigration.Models;

public partial class Area
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string NameSearchable { get; set; } = null!;

    public int Priority { get; set; }

    public string? DescriptionFr { get; set; }

    public string? DescriptionEn { get; set; }

    public string? WarningFr { get; set; }

    public string? WarningEn { get; set; }

    public string? Tags { get; set; }

    public double SouthWestLat { get; set; }

    public double SouthWestLon { get; set; }

    public double NorthEastLat { get; set; }

    public double NorthEastLon { get; set; }

    public int Level1Count { get; set; }

    public int Level2Count { get; set; }

    public int Level3Count { get; set; }

    public int Level4Count { get; set; }

    public int Level5Count { get; set; }

    public int Level6Count { get; set; }

    public int Level7Count { get; set; }

    public int Level8Count { get; set; }

    public int ProblemsCount { get; set; }

    public int? ClusterId { get; set; }

    public double DownloadSize { get; set; }
}
