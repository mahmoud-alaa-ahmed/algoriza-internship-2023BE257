using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.RepositoryInterfaces
{
	public interface IBaseRepository<T> where T : class
	{
		public Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? where = null);
		Task<IEnumerable<T>> GetAllAsync(string[] includes = null, Expression<Func<T, bool>>? where = null, int pageSize = 15, int pageNumber = 1);
		Task<IEnumerable<T>> GetAllAsync(string[] includes = null, Expression<Func<T, bool>>? where = null);
		Task<T> GetByIdAsync(Expression<Func<T, bool>> find, string[]? includes = null);
		Task<T> GetByIdAsyncAsNoTracking(Expression<Func<T, bool>> find, string[]? includes = null);
		Task<T> AddAsync(T entity);
		Task<IEnumerable<T>> AddManyAsync(List<T>? entities);
		Task<T> FindAnyAsync(Expression<Func<T, bool>> find); //find by email name id or any property which is not null.
		bool CheckEmailPattern(string email); //check email or u can use regular exp direct as an attribute.
		Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> find, Expression<Func<T, Object>> order, string orderBy, int pageSize = 15, int pageNumber = 1);
		T Edit(T entity);
		void Delete(T entity);
	}
}
