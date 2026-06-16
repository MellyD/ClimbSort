using System;
using System.Collections.Generic;
using System.Text;
using static BoolderDataMigration.Enums;

namespace BoolderDataMigration.Core.Interface
{
    public interface IMigrationService
    {
        Task<bool> MigrateData(string filePath, eDataType eDataType);
        Task<bool> MigrateAllData();
        Task<bool> ImportLinks(string filePath);
    }
}
