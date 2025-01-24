﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>? filter = null, string? includeProperties =null);
        Task<T> GetAsync(Expression<Func<T,bool>> filter ,string? includeProperties =null, bool tracked = false);
        Task RemoveAsync(T entity);
        Task AddAsync(T entity);
    }
}
