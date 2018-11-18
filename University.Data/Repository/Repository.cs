using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Entity;

namespace University.Data.Repository
{
	public class Repository<T> : IRepository<T> where T : BaseEntity
	{
		protected readonly UniversityContext context;
		protected DbSet<T> entities;
		public Repository(UniversityContext context)
		{
			this.context = context;
			entities = context.Set<T>();
		}

		public IQueryable<T> GetAll()
		{
			return entities.AsQueryable<T>();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await entities.ToListAsync();
		}
		public async Task<T> GetAsync(long id)
		{
			return await entities.SingleOrDefaultAsync(s => s.Id == id);
		}
		public void Insert(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			entities.Add(entity);
		}
		public void Update(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			context.Entry(entity).State = EntityState.Modified;
		}
		public void Delete(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			entities.Remove(entity);
		}

		public async Task SaveChangeAsync()
		{
			await context.SaveChangesAsync();
		}
	}
}
