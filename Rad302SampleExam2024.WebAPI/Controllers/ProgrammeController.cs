// ProgrammeController.cs - Version BEFORE using IGenericDataService

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rad302SampleExam2024.DataModel;
using Tracker.WebAPIClient;

namespace Rad302SampleExam2024.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Registrar")] // ✅ This is still required
    public class ProgrammeController : ControllerBase
    {
        private readonly ProgrammeDbContext _context;

        public ProgrammeController(ProgrammeDbContext context)
        {
            ActivityAPIClient.Track(
                StudentID: "S00219971",
                StudentName: "Ulas Karamustafaoglu",
                activityName: "Rad302 Mock Exam 2025",
                Task: "Testing Programme Controller"
            );

            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var programmes = await _context.Programmes.ToListAsync();
            return Ok(programmes);
        }
    }
}