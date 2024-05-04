using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        //private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context) : base(context)
        {
            //_context = context;
        }
        //public int Add(Department department)
        //{
        //    _context.Department.Add(department);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Department department)
        //{
        //    _context.Department.Remove(department);
        //    return _context.SaveChanges();
        //}

        //public IEnumerable<Department> GetAll()
        //    => _context.Department.ToList();

        //public Department GetByID(int? id)
        //    => _context.Department.FirstOrDefault(x => x.Id == id);

        //public int Update(Department department)
        //{
        //    _context.Department.Update(department);
        //    return _context.SaveChanges();
        //}
    }
}
