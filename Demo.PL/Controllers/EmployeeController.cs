using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index(string SearchValue = "")
        {
            IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModel> employeesViewModel;
            if (string.IsNullOrEmpty(SearchValue))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
                employeesViewModel = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.Search(SearchValue);
                employeesViewModel = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            }


            return View(employeesViewModel);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(new EmployeeViewModel());
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeViewModel)
        {
            //ModelState["Department"].ValidationState = ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                ////Manual Mapping
                //Employee employee = new Employee
                //{
                //    Name = employeeViewModel.Name,
                //    Email = employeeViewModel.Email,
                //    Address = employeeViewModel.Address,
                //    DepartmentId = employeeViewModel.DepartmentId,
                //    HireDate = employeeViewModel.HireDate,
                //    Salary = employeeViewModel.Salary,
                //    IsActive = employeeViewModel.IsActive,
                //};

                var employee = _mapper.Map<Employee>(employeeViewModel);

                employee.ImageUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");

                _unitOfWork.EmployeeRepository.Add(employee);

                _unitOfWork.Complete();

                return RedirectToAction("Index");
            }

            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();

            return View(employeeViewModel);
        }
    }
}
