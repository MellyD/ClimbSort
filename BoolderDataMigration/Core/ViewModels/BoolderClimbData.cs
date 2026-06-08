using System.Text.Json.Serialization;

namespace BoolderDataMigration.Core.ViewModels
{

    public class BoolderClimbData
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("features")]
        public List<ClimbFeature> Features { get; set; } = [];
    }

    public class ClimbFeature
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("geometry")]
        public ClimbGeometry Geometry { get; set; } = new();

        [JsonPropertyName("properties")]
        public ClimbProperties Properties { get; set; } = new();
    }

    public class ClimbGeometry
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; set; } = [];
    }

    public class ClimbProperties
    {
        [JsonPropertyName("grade")]
        public string Grade { get; set; } = string.Empty;

        [JsonPropertyName("steepness")]
        public string Steepness { get; set; } = string.Empty;

        [JsonPropertyName("featured")]
        public bool Featured { get; set; }

        [JsonPropertyName("popularity")]
        public int? Popularity { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("circuitColor")]
        public string? CircuitColor { get; set; }

        [JsonPropertyName("circuitId")]
        public int? CircuitId { get; set; }

        [JsonPropertyName("circuitNumber")]
        public string? CircuitNumber { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("nameEn")]
        public string NameEn { get; set; } = string.Empty;
    }
}
