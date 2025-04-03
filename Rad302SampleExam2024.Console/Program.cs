using Tracker.WebAPIClient;
using Microsoft.EntityFrameworkCore;
using Rad302SampleExam2024.DataModel;

class Program
{
    static void Main(string[] args)
    {
        // 1. TRACKER CALL
        ActivityAPIClient.Track(
            StudentID: "S00219971",
            StudentName: "Ulas Karamustafaoglu",
            activityName: "Rad302MockExam2025",
            Task: "Querying Programme and Module list");

        // 2. SET UP DB CONTEXT
        var options = new DbContextOptionsBuilder<ProgrammeDbContext>()
            .UseSqlite("Data Source=../Rad302SampleExam2024.WebAPI/businessmodel.db")
            .Options;

        using var db = new ProgrammeDbContext(options);

        // 3. PROMPT USER FOR PROGRAMME CODE
        Console.Write("Enter a programme code (e.g. SG_KCMPU_H08): ");
        var code = Console.ReadLine();

        // 4. QUERY & DISPLAY MODULES
        var modules = db.ProgrammeDeliveries
            .Include(pd => pd.associatedModule)
            .Where(pd => pd.ProgCode == code)
            .Select(pd => new 
            { 
                pd.ModuleCode, 
                pd.associatedModule.ModuleName 
            })
            .ToList();

        Console.WriteLine($"\nModules for programme {code}:");
        foreach (var m in modules)
        {
            Console.WriteLine($" - {m.ModuleCode}: {m.ModuleName}");
        }
    }
}