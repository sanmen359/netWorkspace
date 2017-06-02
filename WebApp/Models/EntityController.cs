using Core.Modes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{

    [Route("api/[controller]/[action]")]
    public class EntityController<TEntity, TKey> : Controller where TEntity : class, IEnitity<TKey>, new()
    {
        public EntityController(IUnitOfWork Work)
        {
            _Work = Work;
            _Repository = Work.GetRepository<TEntity>();
        }
        protected IUnitOfWork _Work;
        protected IRepository<TEntity> _Repository;

        // GET api/values/5
        [HttpGet("{id}")]
        public virtual Task<TEntity> Get(TKey id)
        {
            return _Work.GetRepository<TEntity>().FindAsync(id);
        }

        // POST api/values
        [HttpPost]
        public virtual ResponseResult<TEntity> PostAndReturn([FromBody]TEntity value)
        {
            _Work.GetRepository<TEntity>().Insert(value);
            _Work.SaveChanges();
            return ReturnSuccess(value);
        }

        // POST api/values
        [HttpPost]
        public virtual Task Post([FromBody]TEntity value)
        {
            _Work.GetRepository<TEntity>().Insert(value);
            return _Work.SaveChangesAsync();
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public virtual Task Put(TKey id, [FromBody]TEntity value)
        {
            _Work.GetRepository<TEntity>().Update(value);
            return _Work.SaveChangesAsync();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public virtual Task Delete(TKey id)
        {
            _Work.GetRepository<TEntity>().Delete(id);
            return _Work.SaveChangesAsync();
        }

        protected ResponseResult<T> ReturnSuccess<T>(T data, string msg = "success")
        {
            return new ResponseResult<T> { Data = data, MSG = msg, Success = true };
        }


        protected ResponseResult<T> ReturnError<T>(T data, string msg)
        {
            return new ResponseResult<T> { Data = data, MSG = msg };
        }
    }

    public class ResponseResult<T>
    {
        public T Data { get; set; }

        public string MSG { get; set; }

        public bool Success { get; set; }
    }

    public class QueryModel
    {
        public string Keyword { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
