using AutoMapper;
using AutoMapper.Features;
using BoolderDataMigration.Core.Interface;
using BoolderDataMigration.Core.ViewModels;
using BoolderDataMigration.Models;
using ClimbSort.Core.Models;
using ClimbSort.Core.Models.Generic;
using ClimbSort.Core.Models.Static;
using ClimbSort.Data;
using ClimbSort.Data.Repository;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml;
using static BoolderDataMigration.Enums;
using static ClimbSort.Core.Enums;
using static System.Net.WebRequestMethods;

namespace BoolderDataMigration.Core.Service
{
    /// <summary>
    /// This is a service class containing methods for the data migration console application to migrate data from the Boolder database to our system.
    /// It also implements website scraping functionality to provide additional tagging information for climbs.
    /// </summary>
    public class MigrationService: IMigrationService
    {
        #region Setup
        private readonly IRepository<ClimbSortDBContext, Climb> _climbRepo;
        private readonly IRepository<ClimbSortDBContext, Crag> _cragRepo;
        private readonly IRepository<ClimbSortDBContext, Grade> _gradeRepo;
        private readonly IRepository<ClimbSortDBContext, Topography> _topographyRepo;
        private readonly IRepository<ClimbSortDBContext, WallType> _wallTypeRepo;
        private readonly IRepository<ClimbSortDBContext, Coordinates> _coordinatesRepo;
        private readonly IRepository<ClimbSortDBContext, Tag> _tagRepo;
        private readonly IRepository<ClimbSortDBContext, TagType> _tagTypeRepo;
        private readonly IRepository<ClimbSortDBContext, ClimbSort.Core.Models.Circuit> _circuitRepo;
        private readonly IRepository<BoolderContext, Area> _boolderAreaRepo;
        private readonly IRepository<BoolderContext, Problem> _boolderProblemRepo;
        private readonly IRepository<BoolderContext, Line> _boolderLineRepo;
        private readonly IRepository<BoolderContext, Topo> _boolderTopoRepo;
        private readonly IRepository<BoolderContext, Models.Circuit> _boolderCircuitRepo;
        private readonly IMapper _mapper;

        public MigrationService(
            IRepository<ClimbSortDBContext, Climb> climbRepo,
            IRepository<ClimbSortDBContext, Crag> cragRepo,
            IRepository<ClimbSortDBContext, Grade> gradeRepo,
            IRepository<ClimbSortDBContext, Topography> topographyRepo,
            IRepository<ClimbSortDBContext, WallType> wallTypeRepo,
            IRepository<ClimbSortDBContext, Coordinates> coordinatesRepo,
            IRepository<ClimbSortDBContext, Tag> tagRepo,
            IRepository<ClimbSortDBContext, TagType> tagTypeRepo,
            IRepository<ClimbSortDBContext, ClimbSort.Core.Models.Circuit> circuitRepo,
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
            _topographyRepo = topographyRepo;
            _wallTypeRepo = wallTypeRepo;
            _coordinatesRepo = coordinatesRepo;
            _tagRepo = tagRepo;
            _tagTypeRepo = tagTypeRepo;
            _circuitRepo = circuitRepo;
            _boolderAreaRepo = boolderAreaRepo;
            _boolderProblemRepo = boolderProblemRepo;
            _boolderLineRepo = boolderLineRepo;
            _boolderTopoRepo = boolderTopoRepo;
            _boolderCircuitRepo = boolderCircuitRepo;
            _mapper = mapper;
        }
        #endregion

        // This method was an earlier version of the ScrapeWebsite method, which has been commented out. It was designed to scrape climb information from a website and update the database with tags based on the scraped data.
        // The method iterates through all climbs, loads the corresponding web page, extracts tags from the HTML, and updates the database accordingly. It also keeps track of the number of successful and failed scrapes, printing the results to the console.
        // I am preserving this commented-out code for reference, as it may contain useful logic or ideas for future implementations, but it is not currently in use.
        //public async Task<bool> ScrapeWebsite() 
        //{
        //    try
        //    {
        //        int passed = 0;
        //        int failed = 0;
        //        List<Climb> climbs = _climbRepo.GetAll().ToList();
        //        foreach( Climb climb in climbs)
        //        {
        //            if (!string.IsNullOrEmpty(climb.Link))
        //            {
        //                try
        //                {
        //                    var web = new HtmlWeb();
        //                    var doc = await web.LoadFromWebAsync($"{climb.Link}?locale=en");
        //                    var descriptionNode = doc.DocumentNode.SelectSingleNode("//div[@class='btype']");
        //                    if (descriptionNode != null)
        //                    {
        //                        List<string> tags = descriptionNode.InnerText.Trim().Split(",").Select(t => t.Trim().Replace("\\n","")).ToList();
        //                        foreach(string tag in tags)
        //                        {
        //                            if (!string.IsNullOrEmpty(tag))
        //                            {
        //                                TagType? tagType = await _tagTypeRepo.FindAsync(t => t.Description.ToLower() == tag.ToLower());
        //                                if(tagType == null)
        //                                {
        //                                    tagType = new()
        //                                    {
        //                                        Description = tag
        //                                    };
        //                                    await _tagTypeRepo.CreateAsync(tagType);
        //                                }
        //                                Tag? existing = await _tagRepo.FindAsync(t => t.TagType == tagType && t.Climb == climb);
        //                                if (existing != null)
        //                                    continue;
        //                                Tag newTag = new()
        //                                {
        //                                    TagType = tagType,
        //                                    Climb = climb
        //                                };
        //                                climb.Tags.Add(newTag);
        //                            }
        //                        }
        //                        await _climbRepo.UpdateAsync(climb);
        //                        passed++;
        //                    }

        //                    Console.Write($"\rPassed: {passed} | Failed: {failed}");
        //                }
        //                catch
        //                {
        //                    failed++;

        //                    Console.Write($"\rPassed: {passed} | Failed: {failed}");
        //                    continue;
        //                }
        //            }

        //        }
        //        return true;
        //    }
        //    catch( Exception ex )
        //    {
        //        Console.WriteLine($"ScrapeWebsite failed: {ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task<bool> ScrapeWebsite()
        //{
        //    try
        //    {
        //        int passed = 0;
        //        int failed = 0;

        //        var context = _climbRepo.ClimbSortDBContext();

        //        var web = new HtmlWeb();

        //        // Load all climbs
        //        var climbs = await _climbRepo.FindAllAsync(c => !c.Tags.Any() && !string.IsNullOrEmpty(c.Link));

        //        // Cache all existing TagTypes
        //        var tagTypes = _tagTypeRepo
        //            .GetAll()
        //            .ToDictionary(
        //                t => t.Description,
        //                StringComparer.OrdinalIgnoreCase);

        //        foreach (var climb in climbs)
        //        {
        //            try
        //            {
        //                var doc = await web.LoadFromWebAsync($"{climb.Link}?locale=en");
        //                var descriptionNode = doc.DocumentNode.SelectSingleNode("//div[@class='btype']");

        //                if (descriptionNode == null)
        //                    continue;

        //                var tags = descriptionNode.InnerText
        //                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
        //                    .Select(t => t.Trim())
        //                    .Where(t => !string.IsNullOrWhiteSpace(t));

        //                // HashSet makes lookups O(1)
        //                var existingTagIds = climb.Tags
        //                    .Where(t => t.TagType != null)
        //                    .Select(t => t.TagType.Id)
        //                    .ToHashSet();

        //                foreach (var tag in tags)
        //                {
        //                    // Get existing TagType from cache
        //                    if (!tagTypes.TryGetValue(tag, out var tagType))
        //                    {
        //                        tagType = new TagType
        //                        {
        //                            Description = tag
        //                        };

        //                        await _tagTypeRepo.CreateAsync(tagType);

        //                        tagTypes[tag] = tagType;
        //                    }

        //                    // Skip if climb already has this tag
        //                    if (existingTagIds.Contains(tagType.Id))
        //                        continue;

        //                    climb.Tags.Add(new Tag
        //                    {
        //                        TagType = tagType,
        //                        Climb = climb
        //                    });

        //                    existingTagIds.Add(tagType.Id);
        //                }

        //                //await _climbRepo.UpdateAsync(climb);

        //                passed++;
        //            }
        //            catch (Exception ex)
        //            {
        //                failed++;
        //                Console.WriteLine($"\nFailed to scrape '{climb.Link}': {ex.Message}");
        //            }

        //            Console.WriteLine($"\rPassed: {passed} | Failed: {failed}");
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"ScrapeWebsite failed: {ex.Message}");
        //        throw;
        //    }
        //}

        /// <summary>
        /// This method scrapes climb information from the bleau.info website and updates our database with tags based on the "tag" section of each climb.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ScrapeWebsite()
        {
            try
            {
                //I've opted to save the changes in batches to improve performance and reduce the number of database calls. The BatchSize constant defines how many climbs will be processed before saving changes to the database.
                const int BatchSize = 100;

                int passed = 0;
                int failed = 0;
                int processedSinceSave = 0;

                //I've also decided to do database calls via the context, in order to reduce the number of calls to the repository and improve performance. This is a trade-off between performance and encapsulation, but in this case, I believe it's justified.
                //It also allows me to make changes without saving to the database immediately, which is useful for batch processing.
                var context = _climbRepo.ClimbSortDBContext();

                var web = new HtmlWeb();

                //We only want to scrape climbs that don't have any tags yet and have a valid link, so we filter the climbs accordingly.
                var climbs = await _climbRepo.FindAllAsync(
                    c => !c.Tags.Any() && !string.IsNullOrEmpty(c.Link));

                //We cache all existing TagTypes in a dictionary for quick lookups, which improves performance when checking if a tag already exists.
                var tagTypes = _tagTypeRepo
                    .GetAll()
                    .ToDictionary(
                        t => t.Description,
                        StringComparer.OrdinalIgnoreCase);

                //We then iterate through all of the climbs in the list, scraping the website for each climb's tags and updating the database accordingly.
                foreach (var climb in climbs)
                {
                    try
                    {
                        //The climb's corresponding page's HTML is loaded using HtmlAgilityPack.
                        var doc = await web.LoadFromWebAsync($"{climb.Link}?locale=en");

                        //The "tag" section is extracted from the HTML, this section of css class 'btype' contains the tags for the climb.
                        var descriptionNode = doc.DocumentNode.SelectSingleNode("//div[@class='btype']");

                        //If there is nothing in the "tag" section, we skip to the next climb.
                        if (descriptionNode == null)
                            continue;

                        //The section is then split into individual tags, which are trimmed of whitespace and filtered to remove any empty or whitespace-only strings.
                        var tags = descriptionNode.InnerText
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(t => t.Trim())
                            .Where(t => !string.IsNullOrWhiteSpace(t));

                        //We create a HashSet of the existing tag IDs for the climb, which allows for O(1) lookups when checking if a tag already exists for the climb.
                        var existingTagIds = climb.Tags
                            .Where(t => t.TagType != null)
                            .Select(t => t.TagType.Id)
                            .ToHashSet();

                        //We then iterate through all of the tags for the climb, checking if each tag already exists in the database and creating it if it doesn't. If the tag already exists for the climb, we skip to the next tag.
                        foreach (var tag in tags)
                        {
                            if (!tagTypes.TryGetValue(tag, out var tagType))
                            {
                                tagType = new TagType
                                {
                                    Description = tag
                                };

                                context.Add(tagType);

                                tagTypes[tag] = tagType;
                            }

                            if (existingTagIds.Contains(tagType.Id))
                                continue;

                            climb.Tags.Add(new Tag
                            {
                                TagType = tagType,
                                Climb = climb
                            });

                            existingTagIds.Add(tagType.Id);
                        }

                        //We iterate the counters.
                        passed++;
                        processedSinceSave++;

                        //If we've processed enough climbs to reach the batch size, we save the changes to the database and clear the change tracker to free up memory.
                        if (processedSinceSave >= BatchSize)
                        {
                            await context.SaveChangesAsync();
                            context.ChangeTracker.Clear();

                            processedSinceSave = 0;

                            Console.WriteLine($"\nSaved batch ({passed} climbs processed)");
                        }
                    }
                    //If an exception occurs during the scraping or database update process, we catch it, increment the failed counter, and print an error message to the console.
                    catch (Exception ex)
                    {
                        failed++;
                        Console.WriteLine($"\nFailed to scrape '{climb.Link}': {ex.Message}");
                    }

                    //This acts as a live progress indicator, showing the number of successful and failed scrapes in real-time on the console.
                    Console.Write($"\rPassed: {passed} | Failed: {failed}");
                }

                // Save anything remaining
                if (processedSinceSave > 0)
                {
                    await context.SaveChangesAsync();
                    context.ChangeTracker.Clear();
                }

                Console.WriteLine();
                Console.WriteLine($"Finished. Passed: {passed}, Failed: {failed}");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ScrapeWebsite failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// This method migrates all data from the Boolder sqllite database to our system, including crags, circuits, climbs, and topographies. 
        /// It handles the mapping of data between the two systems and ensures that all necessary relationships are maintained.
        /// This is done in sections, starting with crags, then circuits, climbs, and finally topographies. Each section is processed in a try-catch block to ensure that if an error occurs in one section, it does not prevent the migration of the other sections.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> MigrateAllData()
        {
            try
            {
                #region Crags
                //We use repositories similar to the ones we use to access our own database. Using a context we set to access the sqllite database, we can access the Boolder database and retrieve all of the areas (crags) to be migrated.
                //This context and the corresponding models were all built using the ef core console tools.
                List<Area> areas = _boolderAreaRepo.GetAll().ToList();
                int cragCounter = 0;
                foreach (Area area in areas)
                {
                    try
                    {
                        //New crag item is created based on the Boolder area data, and the necessary properties are set. The country code is hardcoded to "FRA" since all climbs in the Boolder database are in Fontainebleau, France.
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

                        //Tags for the crags are included in the database, so they are split into a list and added to the crag's tags. If a tag type does not already exist in our database, it is created.
                        List<string>? tagStrings = area.Tags?.Split(",").ToList();
                        foreach (string tagString in tagStrings ?? new List<string>())
                        {
                            if (!string.IsNullOrEmpty(tagString))
                            {
                                //Since we have some predefined tag types in our system, we can map the Boolder tags to our tag types. If a tag type does not exist, it is created.
                                //The mapping was mostly needed to keep the naming convention more clean, which the boolder database had some inconsistencies with.
                                switch (tagString)
                                {
                                    case "popular":
                                        Tag tag = new()
                                        {
                                            TagType = _tagTypeRepo.FindAsync(t => t.Description.ToLower() == "popular").Result ?? new TagType { Description = "Popular" }
                                        };
                                        crag.Tags.Add(tag);
                                        break;
                                    case "beginner_friendly":
                                        Tag beginnerTag = new()
                                        {
                                            TagType = _tagTypeRepo.FindAsync(t => t.Description.ToLower() == "beginner friendly").Result ?? new TagType { Description = "Beginner Friendly" }
                                        };
                                        crag.Tags.Add(beginnerTag);
                                        break;
                                    case "family_friendly":
                                        Tag familyTag = new()
                                        {
                                            TagType = _tagTypeRepo.FindAsync(t => t.Description.ToLower() == "family friendly").Result ?? new TagType { Description = "Family Friendly" }
                                        };
                                        crag.Tags.Add(familyTag);
                                        break;
                                    case "dry_fast":
                                        Tag dryTag = new()
                                        {
                                            TagType = _tagTypeRepo.FindAsync(t => t.Description.ToLower() == "dry fast").Result ?? new TagType { Description = "Dry Fast" }
                                        };
                                        crag.Tags.Add(dryTag);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        //Coordinates are then added for the crag, including the south-west and north-east points. These are used to define the bounding box for the crag on a map.
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

                        //Counter is incremented after crag is created.
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

                //Same is done for circuits.
                #region Circuits
                List<Models.Circuit> circuits = _boolderCircuitRepo.GetAll().ToList();

                int circuitCounter = 0;
                foreach (Models.Circuit circuit in circuits)
                {
                    try
                    {
                        ClimbSort.Core.Models.Circuit newCircuit = new()
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

                //Some logic is different for climbs, any differences are explained in the comments below.
                #region Climbs
                List<Problem> problems = _boolderProblemRepo.GetAll().ToList();

                int problemCounter = 0;
                foreach (Problem problem in problems)
                {
                    try
                    {
                        string wallTypeName = "";
                        //Some manual mapping is performed for wall types. This is due to naming inconsitencies between my preferred naming convention and the boolder database's.
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
                        //Wall type is fetched based on this name.
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
                            SearchName = problem.NameSearchable
                        };

                        //Crag is fetched based on the area id of the problem, finding the crag based on the area
                        Area? areaForSearch = areas.Where(a => a.Id == problem.AreaId).FirstOrDefault();
                        if (areaForSearch != null)
                            climb.Crag = await _cragRepo.FindAsync(c => c.Name == areaForSearch.Name) ?? throw new KeyNotFoundException($"Crag with name {areaForSearch.Name} not found in database");

                        //Circuit is fetched based on a similar precident. If either cannot be found, an exception is thrown.
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

                //Topographies are handled slightly differently.
                //I treat the x and y coordinates meant to be placed on images as latitude and longitude coordinates, which is not technically correct, but it allows me to use the same coordinate system for all of my data.
                //I then save the reference for the image to the topography, using the assets link assembled from the bleau info id of the problem.
                //With both image and coordinates, the line for the route can then be displayed on the image in the front-end.
                //This is a trade-off between accuracy and simplicity, but I believe it's justified in this case.
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

        /// <summary>
        /// This method imports links for climbs using the logic of the bleau.info website. it fetches the bleau.info Id of the climb from the boolder sqllite database,
        /// matches it with the endpoint of the corresponding crag using the imported list of all crag endpoints read out of the html file provided (via the filePath input).
        /// This is then used to assemble the link for the climb, which is then saved against the climb to the database.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<bool> ImportLinks(string filePath)
        {
            int passed = 0;
            int failed = 0;
            try
            {
                string contents = "";

                using (StreamReader reader = new(filePath))
                {
                    contents = reader.ReadToEnd();
                }

                var doc = new HtmlDocument();
                doc.LoadHtml(contents);

                var links = doc.DocumentNode.SelectNodes("//a");

                //We filter the links to only include those that have a href attribute, a non-empty inner text, and do not contain the string "toggle_favarea" in the href.
                //We then group the links by their inner text and create a dictionary where the key is the inner text and the value is the href attribute value (with the leading '/' removed).
                var dict = links?.Where(a =>
                                {
                                    var href = a.Attributes["href"]?.Value;
                                    var text = a.InnerText?.Trim();

                                    return href != null
                                        && text != null
                                        && href.StartsWith("/")
                                        && !string.IsNullOrWhiteSpace(text)
                                        && !href.Contains("toggle_favarea");
                                })
                                .GroupBy(a => a.InnerText.Trim())
                                .ToDictionary(
                                    g => g.Key,
                                    g => g.First()?.Attributes["href"]?.Value?.TrimStart('/'));
                if (dict == null || !dict.Any())
                    throw new KeyNotFoundException("Failed to find crag extensions.");

                List<Problem> problems = _boolderProblemRepo.GetAll().ToList();
                foreach (var problem in problems)
                {
                    try
                    {
                        Climb? climb = await _climbRepo.FindAsync(c => c.Name == problem.Name);
                        if (climb == null)
                        {
                            failed++;
                            continue;
                        }

                        //We fetch the crag name from the climb's crag property. If the crag name is null or empty, we increment the failed counter and continue to the next problem.
                        string? cragName = climb.Crag?.Name;
                        if (string.IsNullOrEmpty(cragName))
                        {
                            failed++;
                            continue;
                        }

                        //We then try to get the crag extension from the dictionary we created earlier. If the crag extension is null or empty, we increment the failed counter and continue to the next problem.
                        dict.TryGetValue(cragName, out string? cragExtension);
                        if (string.IsNullOrEmpty(cragExtension))
                        {
                            failed++;
                            continue;
                        }

                        //We assemble the link.
                        climb.Link = $"https://bleau.info/{cragExtension}/{problem.BleauInfoId}.html";
                        await _climbRepo.UpdateAsync(climb);
                        passed++;
                    }
                    catch
                    {
                        failed++;
                        continue;
                    }
                    Console.Write($"\rPassed: {passed} | Failed: {failed}");
                }
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine($"Import links failed: {ex.Message}");
                throw;
            } 
        }

        /// <summary>
        /// This was the original migration method, using the json provided in the BoolderDb github project. It is no longer used, but I have kept it here for reference.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="eDataType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This is a helper method used to determine if a given coordinate (longitude, latitude) is contained within the bounds of a crag.
        /// It checks for three types of coordinate representations: a single point, a bounding box defined by southwest and northeast points, and a polygon defined by multiple points. 
        /// The method returns true if the coordinate is contained within any of these representations, and false otherwise.
        /// This is in order to place a climb within a crag based on the climb's coordinates and the crag's coordinates. Less necessary now that the Boolder data has been enriched with crag information, but still useful for future-proofing.
        /// </summary>
        /// <param name="crag"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This is a further helper method used to determine if a given coordinate (longitude, latitude) is contained within a polygon defined by a list of coordinates (longitude, latitude).
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This is a helper method used to save the southwest and northeast corner coordinates of a crag. It takes in the crag object and the unparsed string representations of the southwest and northeast latitude and longitude coordinates.
        /// Used entirely by the more deprecated migration method, but still useful for reference.
        /// </summary>
        /// <param name="crag"></param>
        /// <param name="unParsedSwLat"></param>
        /// <param name="unParsedSwLon"></param>
        /// <param name="unParsedNeLat"></param>
        /// <param name="unParsedNeLon"></param>
        /// <returns></returns>
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
