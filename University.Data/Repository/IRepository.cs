using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using University.Entity;

namespace University.Data.Repository
{
	public interface IRepository<T> where T: BaseEntity
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetAsync(long id);
		void Insert(T entity);
		void Update(T entity);
		void Delete(T entity);
		Task SaveChangeAsync();
	}
}
