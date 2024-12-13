using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repository;
using Demo.DAL.Entities;
using Demo.PL.Utility;
using Demo.PL.ViewModels;

//using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _repository;
        //private readonly IDepartmentRepository _department;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork
           ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
           
            _mapper = mapper;
        }
        public async  Task<IActionResult> IndexAsync(string? searchValue)
        {
            IEnumerable<Employee> employees;
           if(string.IsNullOrWhiteSpace(searchValue))
            {
                 employees = await _unitOfWork.Employees.GetAllAsync();
                return View(_mapper.Map<IEnumerable<EmployeeVM>>(employees));
            }
             employees = await _unitOfWork.Employees.GetAllAsync(e=>e.Name.ToLower().Contains(searchValue.ToLower()));
            return View(_mapper.Map<IEnumerable<EmployeeVM>>(employees));
        }


        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.Departments = await _unitOfWork.Departments.GetAllAsync();
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> CreateAsync(EmployeeVM employeeVm)
        {
            if (ModelState.IsValid)
            {
                employeeVm.ImageName = DocumentSetting.UploadFile(employeeVm.Image,"images");
                var employee = _mapper.Map<EmployeeVM, Employee>(employeeVm);
                await _unitOfWork.Employees.AddAsync(employee);
                await _unitOfWork.completeAsync();
                return RedirectToAction(nameof(IndexAsync));
            }
            ViewBag.Departments = _unitOfWork.Departments.GetAllAsync();
            return View(employeeVm);
        }
        public async Task<IActionResult> Details(int? id) => await ReturnViewWithEmployee(id, nameof(Details));

        public async Task<IActionResult> Edit(int? id) =>await ReturnViewWithEmployee(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(EmployeeVM employeeVm, [FromRoute] int id)
        {
            if (id != employeeVm.Id)
                return BadRequest();
            
            if (ModelState.IsValid)
            {
                try
                {
                    if(employeeVm.Image is not null)
                    {
                        employeeVm.ImageName = DocumentSetting.UploadFile(employeeVm.Image, "images");
                    }
                    _unitOfWork.Employees.Update(_mapper.Map<EmployeeVM, Employee>(employeeVm));
                   await _unitOfWork.completeAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            return View(employeeVm);

        }
        public async Task<IActionResult> Delete(int? id) =>await ReturnViewWithEmployee(id, nameof(Delete));

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(EmployeeVM employeeVm, [FromRoute] int id)

        {
            if (id != employeeVm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Employees.Delete(_mapper.Map<EmployeeVM, Employee>(employeeVm));
                    if (await _unitOfWork.completeAsync() > 0 && employeeVm.ImageName is not null)
                        DocumentSetting.DeleteFile(employeeVm.ImageName, "images");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            return View(employeeVm);

        }
        private async Task<IActionResult> ReturnViewWithEmployee(int? id, string viewName)
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = await _unitOfWork.Employees.GetAsync(id.Value);
            if (employee is null)
                return NotFound();
            ViewBag.Departments = await _unitOfWork.Departments.GetAllAsync();

            return View(viewName, _mapper.Map<EmployeeVM>(employee));
        }
    }
}

