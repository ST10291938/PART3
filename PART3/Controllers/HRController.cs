using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PART3.Data;
using PART3.Models;


namespace PART3.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HR
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lecturers.ToListAsync());
        }

        // GET: HR/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // GET: HR/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HR/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,lecture_ID,lecturer_Name,lecturer_Surname,lecturer_Email,lecturer_Contact,Program,Module_Code,Hours_Worked,Hourly_Rate,Date_Of_Session,UploadedFileName,Status")] Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lecturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

        // GET: HR/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        // POST: HR/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,lecture_ID,lecturer_Name,lecturer_Surname,lecturer_Email,lecturer_Contact,Program,Module_Code,Hours_Worked,Hourly_Rate,Date_Of_Session,UploadedFileName,Status")] Lecturer lecturer)
        {
            if (id != lecturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerExists(lecturer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

     
    

        private bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(e => e.Id == id);
        }

        //Method for generating HR reports
        public async Task<IActionResult> GenerateReport()
        {
            // Fetch all approved claims
            var approvedClaims = await _context.Lecturers
                .Where(l => l.Status == "Approved")
                .ToListAsync();

            if (!approvedClaims.Any())
            {
                TempData["Message"] = "No approved claims found to generate the report.";
                return RedirectToAction(nameof(Index));
            }

            // Create the report content
            var reportBuilder = new System.Text.StringBuilder();
            reportBuilder.AppendLine("Approved Claims Report");
            reportBuilder.AppendLine(new string('-', 50));

            foreach (var claim in approvedClaims)
            {
                reportBuilder.AppendLine($"Lecturer Name: {claim.lecturer_Name} {claim.lecturer_Surname}");
                reportBuilder.AppendLine($"Email: {claim.lecturer_Email}");
                reportBuilder.AppendLine($"Hours Worked: {claim.Hours_Worked}");
                reportBuilder.AppendLine($"Hourly Rate: {claim.Hourly_Rate:C}");
                reportBuilder.AppendLine($"Total Payment: {claim.Total_Payment:C}");
                reportBuilder.AppendLine($"Date of Session: {claim.Date_Of_Session:yyyy-MM-dd}");
                reportBuilder.AppendLine($"Supporting Document: {claim.UploadedFileName}");
                reportBuilder.AppendLine(new string('-', 50));
            }

            // Convert the report content to a byte array
            var reportBytes = System.Text.Encoding.UTF8.GetBytes(reportBuilder.ToString());

            // Return the file for download
            return File(reportBytes, "text/plain", "ApprovedClaimsReport.txt");
        }

    }
}
