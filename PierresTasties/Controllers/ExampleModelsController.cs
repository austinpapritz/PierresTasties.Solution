using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Putting SelectList in ViewBag
using Microsoft.EntityFrameworkCore;
using PierresTasties.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace PierresTasties.Controllers;

public class ExampleModelsController : Controller
{
    private readonly PierresTastiesContext _db;
    public ExampleModelsController(PierresTastiesContext db)
    {
        _db = db;
    }

    public ActionResult Index()
    {
        List<ExampleModel> model = _db.ExampleModels.ToList();
        return View(model);
    }

    public IActionResult Details(int id)
    {
        ExampleModel model = _db.ExampleModels.FirstOrDefault(e => e.Id == id);

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
        ViewData["SubmitButton"] = "Add ExampleModel";
        return View("Form");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name")] ExampleModel exampleModel)
    {
        if (ModelState.IsValid)
        {
            _db.ExampleModels.Add(exampleModel);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        ViewData["FormAction"] = "Create";
        ViewData["SubmitButton"] = "Add ExampleModel";
        return View("Form");
    }

    public IActionResult Edit(int id)
    {
        ExampleModel exampleModelToBeEdited = _db.ExampleModels.FirstOrDefault(e => e.Id == id);

        if (exampleModelToBeEdited == null)
        {
            return NotFound();
        }

        // Both Create and Edit routes use `Form.cshtml`.
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update ExampleModel";

        return View("Form", exampleModelToBeEdited);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Name")] ExampleModel exampleModel)
    {
        // Ensure id from form and url match.
        if (id != exampleModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Try to update changes, catch any ConcurrencyExceptions.
            try
            {
                _db.Update(exampleModel);
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExampleModelExists(exampleModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("details", "examplemodels", new { id = exampleModel.Id });
        }

        // Otherwise reload form.
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update ExampleModel";
        return RedirectToAction("edit", new { id = exampleModel.Id });
    }

    // Method to validate model in db.
    private bool ExampleModelExists(int id)
    {
        return _db.ExampleModels.Any(e => e.Id == id);
    }
}
