using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.Base
{
    /// <summary>
    ///     Base Repositorio
    /// </summary>
    /// <typeparam name="T">Tipo de dato.</typeparam>
    /// <seealso cref="Entities.Interface.Repository.IBaseRepository" />
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseRepository{T}" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public BaseRepository(FormasContext context)
        {
            AppContext = context;
        }

        /// <summary>
        ///     Gets or sets the application context.
        /// </summary>
        /// <value>
        ///     The application context.
        /// </value>
        public DbContext AppContext { get; set; }

        /// <summary>
        ///     Entity.
        /// </summary>
        public DbSet<T> Entity => AppContext.Set<T>();

        /// <summary>
        ///     Adds the asynchronous.
        /// </summary>
        /// <param name="value">The value.</param>
        public async Task AddAsync(T value)
        {
            await Entity.AddAsync(value);

            await AppContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Adds the asynchronous.
        /// </summary>
        /// <param name="values">The values.</param>
        public async Task AddAsync(List<T> values)
        {
            await Entity.AddRangeAsync(values);

            await AppContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Updates the asynchronous.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="propertyExpressions">The property expressions.</param>
        public async Task UpdateAsync(T value, params Expression<Func<T, object>>[] propertyExpressions)
        {
            if (propertyExpressions == null || propertyExpressions.Length <= 0)
            {
                var entity = Entity.Update(value);
                entity.State = EntityState.Modified;
            }
            else
            {
                Entity.Attach(value);
                foreach (var column in propertyExpressions)
                {
                    AppContext.Entry(value).Property(column).IsModified = true;
                }
            }

            await AppContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Updates the asynchronous.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="propertyExpressions">The property expressions.</param>
        public async Task UpdateAsync(List<T> values, params Expression<Func<T, object>>[] propertyExpressions)
        {
            foreach (var value in values)
            {
                await UpdateAsync(value, propertyExpressions);
            }
        }

        /// <summary>
        ///     Deletes the asynchronous.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="predicate">The predicate.</param>
        public async Task DeleteAsync(T value = null, Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                AppContext.Entry(value).State = EntityState.Deleted;
                AppContext.Remove(value);
            }
            else
            {
                IEnumerable<T> entities = Entity.Where(predicate);

                foreach (var entity in entities)
                {
                    AppContext.Entry(entity).State = EntityState.Deleted;
                    AppContext.Remove(entity);
                }
            }

            await AppContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Gets the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="include">The include.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Expression<Func<T, T>> selector = null)
        {
            var query = Entity.AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                if (selector != null)
                {
                    return await orderBy(query).Select(selector).FirstOrDefaultAsync();
                }

                return await orderBy(query).FirstOrDefaultAsync();
            }

            if (selector != null)
            {
                return await query.Select(selector).FirstOrDefaultAsync();
            }

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Gets all asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="include">The include.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Expression<Func<T, T>> selector = null)
        {
            var query = Entity.AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                if (selector != null)
                {
                    return await orderBy(query).Select(selector).ToListAsync();
                }

                return await orderBy(query).ToListAsync();
            }

            if (selector != null)
            {
                return await query.Select(selector).ToListAsync();
            }

            return await query.AsNoTracking().ToListAsync();
        }
    }
}