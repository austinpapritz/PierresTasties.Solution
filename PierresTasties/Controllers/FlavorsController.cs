using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Putting SelectList in ViewBag
using Microsoft.EntityFrameworkCore;
using PierresTasties.Models;
using System.Collections.Generic;
using System.Diagnostics;

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
        Flavor model = _db.Flavors.FirstOrDefault(f => f.FlavorId == id);

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
        ViewData["SubmitButton"] = "Add Flavor";
        return View("Form");
    }

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
        ViewData["FormAction"] = "Create";
        ViewData["SubmitButton"] = "Add Flavor";
        return View("Form");
    }

    public IActionResult Edit(int id)
    {
        Flavor flavorToBeEdited = _db.Flavors.FirstOrDefault(f => f.FlavorId == id);

        if (flavorToBeEdited == null)
        {
            return NotFound();
        }

        // Both Create and Edit routes use `Form.cshtml`.
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update Flavor";

        return View("Form", flavorToBeEdited);
    }

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
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update Flavor";
        return RedirectToAction("edit", new { id = flavor.FlavorId });
    }

    // Method to validate model in db.
    private bool FlavorExists(int id)
    {
        return _db.Flavors.Any(f => f.FlavorId == id);
    }
}
