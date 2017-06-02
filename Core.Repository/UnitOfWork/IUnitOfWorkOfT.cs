// Copyright (c) love.net team. All rights reserved.

using System.Data.Entity;
using System.Threading.Tasks;

namespace Wedo.Vat.UnitOfWork {
    /// <summary>
    /// Defines the interface(s) for generic unit of work.
    /// </summary>
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext {
        /// <summary>
        /// Saves all changes made in this context to the database with distributed transaction.
        /// </summary>
        /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        Task<int> SaveChangesAsync( params IUnitOfWork[] unitOfWorks);
    }
}
