using System;
using Microsoft.EntityFrameworkCore;
using HorasExtras.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace HorasExtras.Data;

public class ProjectHoursContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Extras> Extras { get; set; }


    public ProjectHoursContext(DbContextOptions<ProjectHoursContext> contextOptions)
    {
        Database.EnsureCreated();
    }
    public ProjectHoursContext()
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Filename={GetPath($"{nameof(ProjectHoursContext)}.db3")}");
        SQLitePCL.Batteries_V2.Init();
        // https://sagbansal.medium.com/how-to-update-the-sqlite-database-after-each-app-release-using-entity-framework-xamarin-maui-7b582313f89

    }

    private static string GetPath(string nameDB)
    {
        string pathDbLite = string.Empty;

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            pathDbLite = Path.Combine(FileSystem.AppDataDirectory, nameDB);
        }
        else if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            pathDbLite = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            pathDbLite = Path.Combine(pathDbLite, "..", "Library", nameDB);
        }
        return pathDbLite;
    }

}
