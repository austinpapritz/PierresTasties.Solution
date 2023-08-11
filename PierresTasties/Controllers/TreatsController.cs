using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PierresTasties.Models;

namespace PierresTasties.Controllers;

public class TreatsController : Controller
{
    private readonly PierresTastiesContext _db;
    public TreatsController(PierresTastiesContext db)
    {
        _db = db;
    }

    public ActionResult Index()
    {
        List<Treat> model = _db.Treats.ToList();
        return View(model);
    }

    [Authorize(Roles = "Admin,Patron")]
    public IActionResult Details(int id)
    {
        Treat model = _db.Treats.FirstOrDefault(t => t.TreatId == id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        // Both Create and Edit routes use `Form.cshtml`
        ViewData["FormAction"] = "Create";
        ViewData["SubmitButton"] = "Add Treat";
        return View("Form");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name")] Treat Treat)
    {
        if (ModelState.IsValid)
        {
            _db.Treats.Add(Treat);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        ViewData["FormAction"] = "Create";
        ViewData["SubmitButton"] = "Add Treat";
        return View("Form");
    }

    public IActionResult Edit(int id)
    {
        Treat treatToBeEdited = _db.Treats.FirstOrDefault(t => t.TreatId == id);

        if (treatToBeEdited == null)
        {
            return NotFound();
        }

        // Both Create and Edit routes use `Form.cshtml`.
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update Treat";

        return View("Form", treatToBeEdited);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("TreatId,Name")] Treat treat)
    {
        // Ensure id from form and url match.
        if (id != treat.TreatId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Try to update changes, catch any ConcurrencyExceptions.
            try
            {
                _db.Update(treat);
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TreatExists(treat.TreatId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("details", "Treats", new { id = treat.TreatId });
        }

        // Otherwise reload form.
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update Treat";
        return RedirectToAction("edit", new { id = treat.TreatId });
    }

    // Handled by site.js.
    [HttpPost]
    public IActionResult Delete(int id)
    {
        Treat treatToBeDeleted = _db.Treats.FirstOrDefault(e => e.TreatId == id);

        if (treatToBeDeleted == null)
        {
            return NotFound();
        }

        _db.Treats.Remove(treatToBeDeleted);
        _db.SaveChanges();

        // Return HTTP 200 OK to AJAX request, signalling successful deletion.
        return Ok();
    }


    // Method to validate model in db.
    private bool TreatExists(int id)
    {
        return _db.Treats.Any(t => t.TreatId == id);
    }
}
