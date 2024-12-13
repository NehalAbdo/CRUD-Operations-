using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repository
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly IDepartmentRepository departments;
        private readonly IEmployeeRepository employees;
        private readonly CompanyDbContext _context;
        public UnitOfWork(CompanyDbContext context)
        {
            departments = new DepartmentRepository(context);
            employees = new EmployeeRepository(context);
            _context = context;
        }
        public IDepartmentRepository Departments => departments;
        public IEmployeeRepository Employees => employees;


        public async Task<int> completeAsync() => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()=> await _context.DisposeAsync();
    }
}
