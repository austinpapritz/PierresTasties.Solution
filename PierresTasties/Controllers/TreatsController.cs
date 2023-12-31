using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public IActionResult Details(int id)
    {
        Treat model = _db.Treats
            .Include(t => t.Votes)
            .Include(t => t.FlavorTreats)
            .ThenInclude(fl => fl.Flavor)
            .FirstOrDefault(t => t.TreatId == id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.Flavors = new SelectList(_db.Flavors, "FlavorId", "Name");

        // Both Create and Edit routes use `Form.cshtml`
        ViewBag.FormAction = "Create";
        ViewBag.SubmitButton = "Add Treat";
        return View("Form");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Description")] Treat treat, int flavorId1, int? flavorId2)
    {
        if (ModelState.IsValid)
        {
            _db.Treats.Add(treat);
            _db.SaveChanges();

            FlavorTreat ft1 = new FlavorTreat() { FlavorId = flavorId1, TreatId = treat.TreatId };
            _db.FlavorTreats.Add(ft1);
            _db.SaveChanges();

            if (flavorId2.HasValue && flavorId2 != flavorId1)
            {
                FlavorTreat ft2 = new FlavorTreat() { FlavorId = flavorId2.Value, TreatId = treat.TreatId };
                _db.FlavorTreats.Add(ft2);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        ViewBag.Flavors = new SelectList(_db.Flavors, "FlavorId", "Name");
        ViewBag.FormAction = "Create";
        ViewBag.SubmitButton = "Add Treat";
        return View("Form");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        Treat treatToBeEdited = _db.Treats
            .Include(t => t.FlavorTreats)
            .ThenInclude(ft => ft.Flavor)
            .FirstOrDefault(t => t.TreatId == id);

        if (treatToBeEdited == null)
        {
            return NotFound();
        }

        List<int> selectedFlavorIds = treatToBeEdited.FlavorTreats.Select(ft => ft.FlavorId).ToList();

        ViewBag.Flavors = new SelectList(_db.Flavors, "FlavorId", "Name");
        ViewBag.SelectedFlavor1 = selectedFlavorIds.FirstOrDefault();
        ViewBag.SelectedFlavor2 = selectedFlavorIds.Skip(1).FirstOrDefault();

        ViewBag.FormAction = "Edit";
        ViewBag.SubmitButton = "Update Treat";

        return View("Form", treatToBeEdited);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("TreatId,Name,Description")] Treat treat, int flavorId1, int? flavorId2)
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

                // Remove existing `FlavorTreat` relationships for this treat.
                var existingFlavorTreats = _db.FlavorTreats.Where(ft => ft.TreatId == treat.TreatId);
                _db.FlavorTreats.RemoveRange(existingFlavorTreats);
                _db.SaveChanges();

                // Update the treat itself.
                _db.Update(treat);
                _db.SaveChanges();

                // Update treat with new flavor(s);
                FlavorTreat ft1 = new FlavorTreat() { FlavorId = flavorId1, TreatId = treat.TreatId };
                _db.FlavorTreats.Add(ft1);
                _db.SaveChanges();

                if (flavorId2.HasValue && flavorId2 != flavorId1)
                {
                    FlavorTreat ft2 = new FlavorTreat() { FlavorId = flavorId2.Value, TreatId = treat.TreatId };
                    _db.FlavorTreats.Add(ft2);
                    _db.SaveChanges();
                }
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
        ViewBag.FormAction = "Edit";
        ViewBag.SubmitButton = "Update Treat";
        return RedirectToAction("edit", new { id = treat.TreatId });
    }

    // Handled by site.js.
    [Authorize(Roles = "Admin")]
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
