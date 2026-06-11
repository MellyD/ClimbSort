using System;
using System.Collections.Generic;

namespace BoolderDataMigration.Models;

public partial class Line
{
    public int Id { get; set; }

    public int ProblemId { get; set; }

    public int TopoId { get; set; }

    public string? Coordinates { get; set; }
}
