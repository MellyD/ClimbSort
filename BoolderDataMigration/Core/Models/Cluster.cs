using System;
using System.Collections.Generic;

namespace BoolderDataMigration.Models;

public partial class Cluster
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int MainAreaId { get; set; }
}
