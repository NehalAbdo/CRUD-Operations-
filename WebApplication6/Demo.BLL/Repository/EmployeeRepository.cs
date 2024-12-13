using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.DAL.Context;
using System.Linq.Expressions;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Repository
{
    public class EmployeeRepository : GenericIRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<Employee>> GetAllAsync(string Address)
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<Employee> GetAllByName(string name)
        //{
        //    return _context.Employees.Where(e => e.Name.ToLower().Contains(name.ToLower())).ToList();
        //}

        public async Task<IEnumerable<Employee>> GetAllAsync(Expression<Func<Employee, bool>> expression)
        {
            return await _context.Employees.Include(e=>e.Department).Where(expression).ToListAsync();
        }
    }
}
