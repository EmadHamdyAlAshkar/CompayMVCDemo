using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(
          /*IDepartmentRepository departmentRepository,*/
                                    IUnitOfWork unitOfWork,   
                                    ILogger<DepartmentController> logger)
        {
            _unitOfWork = unitOfWork;
            //_departmentRepository = departmentRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();

            //ViewBag.Message = "Hello from View Bag";

            //ViewData["MessageData"] = "Hello From View Data";

            TempData.Keep("MessageTemp");

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.DepartmentRepository.Add(department);
                _unitOfWork.Complete();

                TempData["MessageTemp"] = "Department Added Successfully!";

                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public IActionResult Details(int? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }
                var department = _unitOfWork.DepartmentRepository.GetByID(id);

                if (department is null)
                    return NotFound();

                return View(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Update (int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department = _unitOfWork.DepartmentRepository.GetByID(id);

            if (department is null)
                return NotFound();

            return View(department);
        }

        [HttpPost]
        public IActionResult Update(int id , Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return View(department);
        }

        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department = _unitOfWork.DepartmentRepository.GetByID(id);

            if (department is null)
                return NotFound();

            _unitOfWork.DepartmentRepository.Delete(department);

            return RedirectToAction(nameof(Index));
        }
    }
}
