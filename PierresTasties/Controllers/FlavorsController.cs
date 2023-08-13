using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PierresTasties.Models;

namespace PierresTasties.Controllers;

public class FlavorsController : Controller
{
    private readonly PierresTastiesContext _db;
    public FlavorsController(PierresTastiesContext db)
    {
        _db = db;
    }

    public ActionResult Index()
    {
        List<Flavor> model = _db.Flavors.ToList();
        return View(model);
    }

    public IActionResult Details(int id)
    {
        Flavor model = _db.Flavors
            .Include(f => f.FlavorTreats)
            .ThenInclude(ft => ft.Treat)
            .FirstOrDefault(f => f.FlavorId == id);

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
        ViewBag.FormAction = "Create";
        ViewBag.SubmitButton = "Add Flavor";
        return View("Form");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name")] Flavor flavor)
    {
        if (ModelState.IsValid)
        {
            _db.Flavors.Add(flavor);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        ViewBag.FormAction = "Create";
        ViewBag.SubmitButton = "Add Flavor";
        return View("Form");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        Flavor flavorToBeEdited = _db.Flavors.FirstOrDefault(f => f.FlavorId == id);

        if (flavorToBeEdited == null)
        {
            return NotFound();
        }

        // Both Create and Edit routes use `Form.cshtml`.
        ViewBag.FormAction = "Edit";
        ViewBag.SubmitButton = "Update Flavor";

        return View("Form", flavorToBeEdited);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("FlavorId,Name")] Flavor flavor)
    {
        // Ensure id from form and url match.
        if (id != flavor.FlavorId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Try to update changes, catch any ConcurrencyExceptions.
            try
            {
                _db.Update(flavor);
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlavorExists(flavor.FlavorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("details", "flavors", new { id = flavor.FlavorId });
        }

        // Otherwise reload form.
        ViewBag.FormAction = "Edit";
        ViewBag.SubmitButton = "Update Flavor";
        return RedirectToAction("edit", new { id = flavor.FlavorId });
    }

    // Handled by site.js.
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Delete(int id)
    {
        Flavor flavorToBeDeleted = _db.Flavors.FirstOrDefault(e => e.FlavorId == id);

        if (flavorToBeDeleted == null)
        {
            return NotFound();
        }

        _db.Flavors.Remove(flavorToBeDeleted);
        _db.SaveChanges();

        // Return HTTP 200 OK to AJAX request, signalling successful deletion.
        return Ok();
    }


    // Method to validate model in db.
    private bool FlavorExists(int id)
    {
        return _db.Flavors.Any(f => f.FlavorId == id);
    }
}
