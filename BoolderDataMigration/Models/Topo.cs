using System;
using System.Collections.Generic;

namespace BoolderDataMigration.Models;

public partial class Topo
{
    public int Id { get; set; }

    public int AreaId { get; set; }

    public int? BoulderId { get; set; }

    public int? Position { get; set; }
}
