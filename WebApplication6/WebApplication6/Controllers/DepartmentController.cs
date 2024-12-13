using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> CreateAsync(Department department)
        {
            if (ModelState.IsValid)
            {
               await _unitOfWork.Departments.AddAsync(department);
                return RedirectToAction(nameof(IndexAsync));
            }
            return View(department);
        }
        public async Task<IActionResult> DetailsAsync(int? id) => await ReturnViewWithDepartment(id, nameof(DetailsAsync));

        public async Task<IActionResult> EditAsync(int? id) =>await ReturnViewWithDepartment(id, nameof(Edit));

        [HttpPost]
        public IActionResult Edit(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Departments.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            return View(department);

        }
        public async Task<IActionResult> DeleteAsync(int? id) => await ReturnViewWithDepartment(id, nameof(Delete));

        [HttpPost]
        public IActionResult Delete(Department department, [FromRoute] int id)

        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Departments.Delete(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            return View(department);

        }
        private async Task< IActionResult>  ReturnViewWithDepartment(int? id, string viewName)
        {
            if (!id.HasValue)
                return BadRequest();

            var department = await _unitOfWork.Departments.GetAsync(id.Value);
            if (department is null)
                return NotFound();

            return View(viewName, department);
        }
    }
}
