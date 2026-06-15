using AutoMapper;
using AutoMapper.Features;
using BoolderDataMigration.Core.Interface;
using BoolderDataMigration.Core.ViewModels;
using BoolderDataMigration.Models;
using FontRecommender.Core.Models;
using FontRecommender.Core.Models.Generic;
using FontRecommender.Data;
using FontRecommender.Data.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using static BoolderDataMigration.Enums;
using static FontRecommender.Core.Enums;

namespace BoolderDataMigration.Core.Service
{
    public class MigrationService: IMigrationService
    {
        #region Setup
        private readonly IRepository<FontRecommendationDBContext, Climb> _climbRepo;
        private readonly IRepository<FontRecommendationDBContext, Crag> _cragRepo;
        private readonly IRepository<FontRecommendationDBContext, Grade> _gradeRepo;
        private readonly IRepository<FontRecommendationDBContext, GradingSystem> _gradingSystemRepo;
        private readonly IRepository<FontRecommendationDBContext, Topography> _topographyRepo;
        private readonly IRepository<FontRecommendationDBContext, WallType> _wallTypeRepo;
        private readonly IRepository<FontRecommendationDBContext, Coordinates> _coordinatesRepo;
        private readonly IRepository<FontRecommendationDBContext, FontRecommender.Core.Models.Circuit> _circuitRepo;
        private readonly IRepository<BoolderContext, Area> _boolderAreaRepo;
        private readonly IRepository<BoolderContext, Problem> _boolderProblemRepo;
        private readonly IRepository<BoolderContext, Line> _boolderLineRepo;
        private readonly IRepository<BoolderContext, Topo> _boolderTopoRepo;
        private readonly IRepository<BoolderContext, Models.Circuit> _boolderCircuitRepo;
        private readonly IMapper _mapper;

        public MigrationService(
            IRepository<FontRecommendationDBContext, Climb> climbRepo,
            IRepository<FontRecommendationDBContext, Crag> cragRepo,
            IRepository<FontRecommendationDBContext, Grade> gradeRepo,
            IRepository<FontRecommendationDBContext, GradingSystem> gradingSystemRepo,
            IRepository<FontRecommendationDBContext, Topography> topographyRepo,
            IRepository<FontRecommendationDBContext, WallType> wallTypeRepo,
            IRepository<FontRecommendationDBContext, Coordinates> coordinatesRepo,
            IRepository<FontRecommendationDBContext, FontRecommender.Core.Models.Circuit> circuitRepo,
            IRepository<BoolderContext, Area> boolderAreaRepo,
            IRepository<BoolderContext, Problem> boolderProblemRepo,
            IRepository<BoolderContext, Line> boolderLineRepo,
            IRepository<BoolderContext, Topo> boolderTopoRepo,
            IRepository<BoolderContext, Models.Circuit> boolderCircuitRepo,
            IMapper mapper
            ) 
        {
            _climbRepo = climbRepo;
            _cragRepo = cragRepo;
            _gradeRepo = gradeRepo;
            _gradingSystemRepo = gradingSystemRepo;
            _topographyRepo = topographyRepo;
            _wallTypeRepo = wallTypeRepo;
            _coordinatesRepo = coordinatesRepo;
            _circuitRepo = circuitRepo;
            _boolderAreaRepo = boolderAreaRepo;
            _boolderProblemRepo = boolderProblemRepo;
            _boolderLineRepo = boolderLineRepo;
            _boolderTopoRepo = boolderTopoRepo;
            _boolderCircuitRepo = boolderCircuitRepo;
            _mapper = mapper;
        }
        #endregion

        public async Task<bool> MigrateAllData()
        {
            try
            {
                #region Crags
                List<Area> areas = _boolderAreaRepo.GetAll().ToList();
                int cragCounter = 0;
                foreach (Area area in areas)
                {
                    try
                    {
                        Crag crag = new()
                        {
                            Name = area.Name,
                            CountryCode = "FRA",
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            Description = area.DescriptionEn,
                            SearchName = area.NameSearchable,
                            Id = Guid.NewGuid()
                        };
                        List<string>? tagStrings = area.Tags?.Split(",").ToList();
                        foreach (string tagString in tagStrings ?? new List<string>())
                        {
                            if (!string.IsNullOrEmpty(tagString))
                            {
                                switch (tagString)
                                {
                                    case "popular":
                                        Tag tag = new()
                                        {
                                            TagType = eTag.Popular
                                        };
                                        crag.Tags.Add(tag);
                                        break;
                                    case "beginner_friendly":
                                        Tag beginnerTag = new()
                                        {
                                            TagType = eTag.BeginnerFriendly
                                        };
                                        crag.Tags.Add(beginnerTag);
                                        break;
                                    case "family_friendly":
                                        Tag familyTag = new()
                                        {
                                            TagType = eTag.FamilyFriendly
                                        };
                                        crag.Tags.Add(familyTag);
                                        break;
                                    case "dry_fast":
                                        Tag dryTag = new()
                                        {
                                            TagType = eTag.DryFast
                                        };
                                        crag.Tags.Add(dryTag);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        Coordinates swCoordinates = new()
                        {
                            CoordinateType = eCoordinateType.SWPoint,
                            Latitude = area.SouthWestLat,
                            Longitude = area.SouthWestLon,
                            Crag = crag
                        };
                        Coordinates neCoordinates = new()
                        {
                            CoordinateType = eCoordinateType.NEPoint,
                            Latitude = area.NorthEastLat,
                            Longitude = area.NorthEastLon,
                            Crag = crag
                        };
                        crag.Coordinates.Add(swCoordinates);
                        crag.Coordinates.Add(neCoordinates);

                        await _cragRepo.CreateAsync(crag);
                        cragCounter++;
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine($"Failed to migrate area {area.Name}: {ex.Message}");
                        continue;
                    }
                }
                Console.WriteLine($"Imported {cragCounter} Crags");
                #endregion

                #region Circuits
                List<Models.Circuit> circuits = _boolderCircuitRepo.GetAll().ToList();

                int circuitCounter = 0;
                foreach (Models.Circuit circuit in circuits)
                {
                    try
                    {
                        FontRecommender.Core.Models.Circuit newCircuit = new()
                        {
                            Colour = circuit.Color,
                            Beginner = circuit.BeginnerFriendly == 1,
                            Dangerous = circuit.Dangerous == 1,
                            Grade = await _gradeRepo.FindAsync(g => g.GradeLabel.ToLower() == circuit.AverageGrade.ToLower()) ?? throw new KeyNotFoundException($"Grade {circuit.AverageGrade} not found in database"),
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            Id = Guid.NewGuid()
                        };
                        Coordinates swCoordinates = new()
                        {
                            CoordinateType = eCoordinateType.SWPoint,
                            Latitude = circuit.SouthWestLat,
                            Longitude = circuit.SouthWestLon,
                            Circuit = newCircuit
                        };
                        Coordinates neCoordinates = new()
                        {
                            CoordinateType = eCoordinateType.NEPoint,
                            Latitude = circuit.NorthEastLat,
                            Longitude = circuit.NorthEastLon,
                            Circuit = newCircuit
                        };
                        newCircuit.Coordinates.Add(swCoordinates);
                        newCircuit.Coordinates.Add(neCoordinates);
                        await _circuitRepo.CreateAsync(newCircuit);
                        circuitCounter++;
                    }
                    catch(Exception ex) {
                        Console.WriteLine($"Failed to migrate circuit {circuit.Color}: {ex.Message}");
                        continue;
                    }
                }
                Console.WriteLine($"Imported {circuitCounter} Circuits");
                #endregion

                #region Climbs
                List<Problem> problems = _boolderProblemRepo.GetAll().ToList();

                int problemCounter = 0;
                foreach (Problem problem in problems)
                {
                    try
                    {
                        string wallTypeName = "";
                        switch (problem.Steepness)
                        {
                            case "wall":
                                wallTypeName = "Vertical";
                                break;
                            case "overhang":
                                wallTypeName = "Steep";
                                break;
                            default:
                                wallTypeName = problem.Steepness;
                                break;
                        }
                        WallType walltype = await _wallTypeRepo.FindAsync(w => w.Description.ToLower() == wallTypeName.ToLower()) ?? throw new KeyNotFoundException($"Wall type {wallTypeName} not found in database");
                        Climb climb = new()
                        {
                            Name = problem.Name ?? "Unknown",
                            Grade = await _gradeRepo.FindAsync(g => problem.Grade != null && g.GradeLabel.ToLower() == problem.Grade.ToLower()) ?? throw new KeyNotFoundException($"Grade {problem.Grade} not found in database"),
                            Popularity = problem.Popularity,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            WallType = walltype,
                            SitStart = problem.SitStart == 1,
                            SearchName = problem.NameSearchable,
                        };

                        Area? areaForSearch = areas.Where(a => a.Id == problem.AreaId).FirstOrDefault();
                        if (areaForSearch != null)
                            climb.Crag = await _cragRepo.FindAsync(c => c.Name == areaForSearch.Name) ?? throw new KeyNotFoundException($"Crag with name {areaForSearch.Name} not found in database");

                        Models.Circuit? circuitforSearch = circuits.Where(c => c.Id == problem.CircuitId).FirstOrDefault();
                        if (circuitforSearch != null)
                            climb.Circuit = await _circuitRepo.FindAsync(c => c.Colour == circuitforSearch.Color) ?? throw new KeyNotFoundException($"Circuit with colour {circuitforSearch.Color} not found in database");

                        Coordinates climbCoordinates = new()
                        {
                            Climb = climb,
                            CoordinateType = eCoordinateType.Point,
                            Latitude = problem.Latitude,
                            Longitude = problem.Longitude
                        };
                        climb.Coordinates.Add(climbCoordinates);
                        await _climbRepo.CreateAsync(climb);
                        problemCounter++;
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine($"Failed to create climb: {ex.Message}");
                        continue;
                    }
                }
                Console.WriteLine($"Imported {problemCounter} Problems");
                #endregion

                #region Topographies
                List<Line> lines = _boolderLineRepo.GetAll().ToList();

                int topoCounter = 0;
                foreach (Line line in lines)
                {
                    try
                    {
                        Problem? problemForSearch = problems.Where(p => p.Id == line.ProblemId).FirstOrDefault();
                        if (problemForSearch != null)
                        {
                            Climb climb = await _climbRepo.FindAsync(c => c.Name == problemForSearch.Name) ?? throw new KeyNotFoundException($"Climb with name {problemForSearch.Name} not found in database");
                            Topography topography = new()
                            {
                                Climb = climb,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                Id = Guid.NewGuid(),
                                FileReference = $"https://assets.boolder.com/proxy/topos/{problemForSearch.BleauInfoId}"
                            };
                            if(line.Coordinates != null)
                            {
                                List<TopoCoordinates> topoCoordinates = JsonSerializer.Deserialize<List<TopoCoordinates>>(line.Coordinates) ?? throw new JsonException("Failed to deserialise topo coordinates");
                                foreach(TopoCoordinates topoCoordinate in topoCoordinates)
                                {
                                    Coordinates topographyCoordinate = new()
                                    {
                                        Topography = topography,
                                        CoordinateType = eCoordinateType.TopographyLine,
                                        Latitude = topoCoordinate.Y,
                                        Longitude = topoCoordinate.X
                                    };
                                    topography.Coordinates.Add(topographyCoordinate);
                                }
                            }
                            await _topographyRepo.CreateAsync(topography);
                            topoCounter++;
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Failed to create topography for line with id {line.Id}: {ex.Message}");
                        continue;
                    }
                }
                Console.WriteLine($"Imported {topoCounter} Topographies");
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> MigrateData(string filePath, eDataType eDataType)
        {
            try
            {

                string contents = "";

                using (StreamReader reader = new(filePath))
                {
                    contents = reader.ReadToEnd();
                }
                switch (eDataType)
                {
                    case eDataType.Climb:
                        BoolderClimbData climbData = JsonSerializer.Deserialize<BoolderClimbData>(contents) ?? throw new JsonException("Failed to deserialise data");
                        foreach (ClimbFeature feature in climbData.Features)
                        {
                            Grade climbGrade = await _gradeRepo.FindAsync(g => g.GradeLabel.ToLower() == feature.Properties.Grade.ToLower()) ?? throw new KeyNotFoundException($"Grade {feature.Properties.Grade} not found in database");
                            string wallTypeName = "";
                            switch (feature.Properties.Steepness)
                            {
                                case "wall":
                                    wallTypeName = "Vertical";
                                    break;
                                case "overhang":
                                    wallTypeName = "Steep";
                                    break;
                                default:
                                    wallTypeName = feature.Properties.Steepness;
                                    break;
                            }
                            WallType walltype = await _wallTypeRepo.FindAsync(w => w.Description.ToLower() == wallTypeName.ToLower()) ?? throw new KeyNotFoundException($"Wall type {wallTypeName} not found in database");
                            Climb climb = new()
                            {
                                Name = feature.Properties.Name,
                                Grade = climbGrade,
                                WallType = walltype,
                                Popularity = feature.Properties.Popularity,
                                Id = Guid.NewGuid(),
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now
                            };
                            await _climbRepo.CreateAsync(climb);

                            Coordinates climbCoordinates = new()
                            {
                                Climb = climb,
                                CoordinateType = eCoordinateType.Point,
                                Latitude = feature.Geometry.Coordinates[1],
                                Longitude = feature.Geometry.Coordinates[0]
                            };
                            await _coordinatesRepo.CreateAsync(climbCoordinates);
                        }
                        return true;
                    case eDataType.Crag:
                        BoolderCragData cragData = JsonSerializer.Deserialize<BoolderCragData>(contents) ?? throw new JsonException("Failed to deserialise data");
                        foreach (CragFeature feature in cragData.Features)
                        {
                            if(!string.IsNullOrEmpty(feature.Properties.Name))
                            {
                                Crag? existingCrag = await _cragRepo.FindAsync(c => c.Name == feature.Properties.Name);
                                if (existingCrag != null)
                                {
                                    if (feature.Geometry.Type == "Point")
                                    {
                                        JsonElement element = (JsonElement)feature.Geometry.Coordinates;

                                        List<double> pointCoords = element
                                            .EnumerateArray()
                                            .Select(x => x.GetDouble())
                                            .ToList();
                                        //double[] pointCoords = (double[])feature.Geometry.Coordinates;
                                        Coordinates cragCoordinates = new()
                                        {
                                            Crag = existingCrag,
                                            CoordinateType = eCoordinateType.Point,
                                            Latitude = pointCoords[1],
                                            Longitude = pointCoords[0]
                                        };
                                        await _coordinatesRepo.CreateAsync(cragCoordinates);

                                        if (!string.IsNullOrEmpty(feature.Properties.SouthWestLat) && !string.IsNullOrEmpty(feature.Properties.SouthWestLon) && !string.IsNullOrEmpty(feature.Properties.NorthEastLat) && !string.IsNullOrEmpty(feature.Properties.NorthEastLon))
                                        {
                                            bool cornersSaved = await SaveCornerCoords(existingCrag, feature.Properties.SouthWestLat, feature.Properties.SouthWestLon, feature.Properties.NorthEastLat, feature.Properties.NorthEastLon);

                                            if (!cornersSaved)
                                            {
                                                Console.WriteLine($"Failed to save corner coordinates for crag {existingCrag.Name} with id {existingCrag.Id}");
                                            }
                                        }
                                    }
                                    else if (feature.Geometry.Type == "Polygon")
                                    {
                                        JsonElement element = (JsonElement)feature.Geometry.Coordinates;

                                        List<List<List<double>>> values = element
                                            .EnumerateArray()
                                            .Select(level2 => level2
                                                .EnumerateArray()
                                                .Select(level3 => level3
                                                    .EnumerateArray()
                                                    .Select(coord => coord.GetDouble())
                                                    .ToList())
                                                .ToList())
                                            .ToList();
                                        foreach (List<double> coordinatePair in values[0])
                                        {
                                            Coordinates cragCoordinates = new()
                                            {
                                                Crag = existingCrag,
                                                CoordinateType = eCoordinateType.CragPolygon,
                                                Latitude = coordinatePair[1],
                                                Longitude = coordinatePair[0]
                                            };
                                            await _coordinatesRepo.CreateAsync(cragCoordinates);
                                        }

                                        if (!string.IsNullOrEmpty(feature.Properties.SouthWestLat) && !string.IsNullOrEmpty(feature.Properties.SouthWestLon) && !string.IsNullOrEmpty(feature.Properties.NorthEastLat) && !string.IsNullOrEmpty(feature.Properties.NorthEastLon))
                                        {
                                            bool cornersSaved = await SaveCornerCoords(existingCrag, feature.Properties.SouthWestLat, feature.Properties.SouthWestLon, feature.Properties.NorthEastLat, feature.Properties.NorthEastLon);

                                            if (!cornersSaved)
                                            {
                                                Console.WriteLine($"Failed to save corner coordinates for crag {existingCrag.Name} with id {existingCrag.Id}");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Crag crag = new()
                                    {
                                        Name = feature.Properties.Name ??= "Unknown",
                                        Id = Guid.NewGuid(),
                                        CreatedDate = DateTime.Now,
                                        ModifiedDate = DateTime.Now,
                                        CountryCode = "FRA"// All crags in the Boolder data are in France, so we can hardcode this value
                                    };
                                    await _cragRepo.CreateAsync(crag);

                                    if (feature.Geometry.Type == "Point")
                                    {
                                        JsonElement element = (JsonElement)feature.Geometry.Coordinates;

                                        List<double> pointCoords = element
                                            .EnumerateArray()
                                            .Select(x => x.GetDouble())
                                            .ToList();
                                        Coordinates cragCoordinates = new()
                                        {
                                            Crag = crag,
                                            CoordinateType = eCoordinateType.Point,
                                            Latitude = pointCoords[1],
                                            Longitude = pointCoords[0]
                                        };
                                        await _coordinatesRepo.CreateAsync(cragCoordinates);

                                        if (!string.IsNullOrEmpty(feature.Properties.SouthWestLat) && !string.IsNullOrEmpty(feature.Properties.SouthWestLon) && !string.IsNullOrEmpty(feature.Properties.NorthEastLat) && !string.IsNullOrEmpty(feature.Properties.NorthEastLon))
                                        {
                                            bool cornersSaved = await SaveCornerCoords(crag, feature.Properties.SouthWestLat, feature.Properties.SouthWestLon, feature.Properties.NorthEastLat, feature.Properties.NorthEastLon);

                                            if (!cornersSaved)
                                            {
                                                Console.WriteLine($"Failed to save corner coordinates for crag {crag.Name} with id {crag.Id}");
                                            }
                                        }
                                    }
                                    else if (feature.Geometry.Type == "Polygon")
                                    {
                                        JsonElement element = (JsonElement)feature.Geometry.Coordinates;

                                        List<List<List<double>>> values = element
                                            .EnumerateArray()
                                            .Select(level2 => level2
                                                .EnumerateArray()
                                                .Select(level3 => level3
                                                    .EnumerateArray()
                                                    .Select(coord => coord.GetDouble())
                                                    .ToList())
                                                .ToList())
                                            .ToList();
                                        foreach (List<double> coordinatePair in values[0])
                                        {
                                            Coordinates cragCoordinates = new()
                                            {
                                                Crag = crag,
                                                CoordinateType = eCoordinateType.CragPolygon,
                                                Latitude = coordinatePair[1],
                                                Longitude = coordinatePair[0]
                                            };
                                            await _coordinatesRepo.CreateAsync(cragCoordinates);
                                        }

                                        if (!string.IsNullOrEmpty(feature.Properties.SouthWestLat) && !string.IsNullOrEmpty(feature.Properties.SouthWestLon) && !string.IsNullOrEmpty(feature.Properties.NorthEastLat) && !string.IsNullOrEmpty(feature.Properties.NorthEastLon))
                                        {
                                            bool cornersSaved = await SaveCornerCoords(crag, feature.Properties.SouthWestLat, feature.Properties.SouthWestLon, feature.Properties.NorthEastLat, feature.Properties.NorthEastLon);

                                            if (!cornersSaved)
                                            {
                                                Console.WriteLine($"Failed to save corner coordinates for crag {crag.Name} with id {crag.Id}");
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        return true;
                    case eDataType.Combine:
                        List<Climb> climbs = _climbRepo.GetAll().ToList();
                        List<Crag> crags = _cragRepo.GetAll().ToList();
                        foreach( Climb climb in climbs )
                        {

                            if(climb.Coordinates != null && climb.Coordinates.Count > 0)
                            {
                                Crag? crag = crags.Where(c => ContainsCoordinate(
                                                                c,
                                                                climb.Coordinates[0].Longitude,
                                                                climb.Coordinates[0].Latitude)).FirstOrDefault();

                                if (crag == null)
                                    continue;

                                climb.Crag = crag;
                                climb.ModifiedDate = DateTime.Now;
                                await _climbRepo.UpdateAsync(climb);


                                //Crag? crag = await _cragRepo.FindAsync(c =>
                                //    c.Coordinates.Any(sw =>
                                //        sw.CoordinateType == eCoordinateType.SWPoint &&
                                //        climb.Coordinates[0].Longitude >= sw.Longitude &&
                                //        climb.Coordinates[0].Latitude >= sw.Latitude)                                    //&&
                                //c.Coordinates.Any(ne =>
                                //    ne.CoordinateType == eCoordinateType.NEPoint &&
                                //    climb.Coordinates[0].Longitude <= ne.Longitude &&
                                //    climb.Coordinates[0].Latitude <= ne.Latitude));
                                //if (crag == null)
                                //    crag = await _cragRepo.FindAsync(c =>
                                //        c.Coordinates.Any(p =>
                                //            p.CoordinateType == eCoordinateType.Point &&
                                //            p.Longitude == climb.Coordinates[0].Longitude &&
                                //            p.Latitude == climb.Coordinates[0].Latitude));
                                //if(crag == null)
                                //{
                                //    crag = await _cragRepo.FindAsync(c =>  ContainsCoordinate(
                                //                                c,
                                //                                climb.Coordinates[0].Longitude,
                                //                                climb.Coordinates[0].Latitude));
                                //}
                            }
                        }
                        return true;

                    case eDataType.Enrich:
                        BoolderClimbData extraClimbData = JsonSerializer.Deserialize<BoolderClimbData>(contents) ?? throw new JsonException("Failed to deserialise data");
                        foreach (ClimbFeature feature in extraClimbData.Features)
                        {
                            Climb climb = await _climbRepo.FindAsync(c => c.Name == feature.Properties.Name) ?? throw new KeyNotFoundException($"Climb {feature.Properties.Name} not found in database");
                            Topography topography = new()
                            {
                                CreatedDate = DateTime.Now,
                                Climb = climb,
                                Id = Guid.NewGuid(),
                                ModifiedDate = DateTime.Now,
                                FileReference = $"https://assets.boolder.com/proxy/topos/{feature.Properties.Id}"
                            };
                            await _topographyRepo.CreateAsync(topography);


                        }

                        return true;
                    default:
                        return true;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        private static bool ContainsCoordinate(Crag crag, double longitude, double latitude)
        {
            var point = crag.Coordinates
                .FirstOrDefault(x => x.CoordinateType == eCoordinateType.Point);

            if (point != null)
            {
                return point.Longitude == longitude &&
                       point.Latitude == latitude;
            }

            var sw = crag.Coordinates
                .FirstOrDefault(x => x.CoordinateType == eCoordinateType.SWPoint);

            var ne = crag.Coordinates
                .FirstOrDefault(x => x.CoordinateType == eCoordinateType.NEPoint);

            if (sw != null && ne != null)
            {
                return longitude >= sw.Longitude &&
                       longitude <= ne.Longitude &&
                       latitude >= sw.Latitude &&
                       latitude <= ne.Latitude;
            }

            var polygon = crag.Coordinates
                .Where(x => x.CoordinateType == eCoordinateType.CragPolygon)
                .Select(x => (x.Longitude, x.Latitude))
                .ToList();

            if (polygon.Count > 2)
            {
                return PointInPolygon(longitude, latitude, polygon);
            }

            return false;
        }
        public static bool PointInPolygon(double longitude, double latitude, List<(double Longitude, double Latitude)> polygon)
        {
            bool inside = false;

            int j = polygon.Count - 1;

            for (int i = 0; i < polygon.Count; i++)
            {
                if (((polygon[i].Latitude > latitude) != (polygon[j].Latitude > latitude))
                    &&
                    (longitude <
                        (polygon[j].Longitude - polygon[i].Longitude)
                        * (latitude - polygon[i].Latitude)
                        / (polygon[j].Latitude - polygon[i].Latitude)
                        + polygon[i].Longitude))
                {
                    inside = !inside;
                }

                j = i;
            }

            return inside;
        }

        private async Task<bool> SaveCornerCoords(Crag crag, string unParsedSwLat, string unParsedSwLon, string unParsedNeLat, string unParsedNeLon)
        {
            Coordinates swCoordinates = new()
            {
                CoordinateType = eCoordinateType.SWPoint,
                Latitude = double.TryParse(unParsedSwLat, out double swLat) ? swLat : 0,
                Longitude = double.TryParse(unParsedSwLon, out double swLon) ? swLon : 0,
                Crag = crag
            };

            if (swCoordinates.Latitude == 0 || swCoordinates.Longitude == 0)
                return false;

            await _coordinatesRepo.CreateAsync(swCoordinates);

            Coordinates neCoordinates = new()
            {
                CoordinateType = eCoordinateType.NEPoint,
                Latitude = double.TryParse(unParsedNeLat, out double neLat) ? neLat : 0,
                Longitude = double.TryParse(unParsedNeLon, out double neLon) ? neLon : 0,
            };

            if (neCoordinates.Latitude == 0 || neCoordinates.Longitude == 0)
                return false;

            await _coordinatesRepo.CreateAsync(neCoordinates);
            return true;
        }
    }
}
