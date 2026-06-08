using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BoolderDataMigration.Core.ViewModels
{
    public class BoolderCragData
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("features")]
        public List<CragFeature> Features { get; set; } = new();
    }

    public class CragFeature
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("geometry")]
        public CragGeometry Geometry { get; set; } = new();

        [JsonPropertyName("properties")]
        public CragProperties Properties { get; set; } = new();
    }

    public class CragGeometry
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        // Point -> [lon, lat], Polygon -> [[[lon, lat], ...], ...]
        [JsonPropertyName("coordinates")]
        public object Coordinates { get; set; } = null!;
    }

    public class CragProperties
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("areaId")]
        public int AreaId { get; set; }

        [JsonPropertyName("clusterId")]
        public int ClusterId { get; set; }

        [JsonPropertyName("priority")]
        public int? Priority { get; set; }

        [JsonPropertyName("southWestLat")]
        public string? SouthWestLat { get; set; }

        [JsonPropertyName("southWestLon")]
        public string? SouthWestLon { get; set; }

        [JsonPropertyName("northEastLat")]
        public string? NorthEastLat { get; set; }

        [JsonPropertyName("northEastLon")]
        public string? NorthEastLon { get; set; }
    }
}
