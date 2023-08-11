using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Putting SelectList in ViewBag
using Microsoft.EntityFrameworkCore;
using PierresTasties.Models;
using System.Collections.Generic;
using System.Diagnostics;

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

    public IActionResult Details(int id)
    {
        Treat model = _db.Treats.FirstOrDefault(t => t.TreatId == id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    public IActionResult Create()
    {
        // Both Create and Edit routes use `Form.cshtml`
        ViewData["FormAction"] = "Create";
        ViewData["SubmitButton"] = "Add Treat";
        return View("Form");
    }

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

    // Method to validate model in db.
    private bool TreatExists(int id)
    {
        return _db.Treats.Any(t => t.TreatId == id);
    }
}
