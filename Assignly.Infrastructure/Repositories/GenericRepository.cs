using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignly.Infrastructure.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        public AppDBContext Context { get; }
        public GenericRepository(AppDBContext context)
        {
            Context = context;
        }

        public async Task Add(T entity)
        {
            await Context.AddAsync(entity);
        }
        public void Delete(Guid id)
        {
            var entity = Context.Find<T>(id);
            if(entity != null)
            {
               Context.Remove(entity);
            }
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await Context.Set<T>().ToListAsync();
        }
        public async Task<T> GetById(Guid id)
        {
            return await Context.FindAsync<T>(id);
        }
        public void Update(T entity)
        {
            Context.Update(entity);
        }
        public async Task SaveChanges()
        {
            await Context.SaveChangesAsync();
        }
    }
}
