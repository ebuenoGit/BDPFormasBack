using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    /// <summary>
    ///     Interfaz Base Repositorio
    /// </summary>
    /// <typeparam name="T">Tipo de Dato.</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        ///     Gets or sets the application context.
        /// </summary>
        /// <value>
        ///     The application context.
        /// </value>
        DbContext AppContext { get; set; }

        /// <summary>
        ///     Entity.
        /// </summary>
        DbSet<T> Entity { get; }

        /// <summary>
        ///     Adds the asynchronous.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        Task AddAsync(List<T> values);

        /// <summary>
        ///     Adds the asynchronous.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        Task AddAsync(T value);

        /// <summary>
        ///     Deletes the asynchronous.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task DeleteAsync(T value = null, Expression<Func<T, bool>> predicate = null);

        /// <summary>
        ///     Gets all asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="include">The include.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, T>> selector = null);

        /// <summary>
        ///     Gets the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="include">The include.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, T>> selector = null);

        /// <summary>
        ///     Updates the asynchronous.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="propertyExpressions">The property expressions.</param>
        /// <returns></returns>
        Task UpdateAsync(List<T> values, params Expression<Func<T, object>>[] propertyExpressions);

        /// <summary>
        ///     Updates the asynchronous.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="propertyExpressions">The property expressions.</param>
        /// <returns></returns>
        Task UpdateAsync(T value, params Expression<Func<T, object>>[] propertyExpressions);
    }
}