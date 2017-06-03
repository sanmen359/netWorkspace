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

        // GET api/entitys/5
        [HttpGet("{id}")]
        public virtual Task<TEntity> Get(TKey id)
        {
            return _Work.GetRepository<TEntity>().FindAsync(id);
        }

        [HttpPost]
        public virtual ResponseResult<TEntity> PostAndReturn([FromBody]TEntity entity)
        {
            OnCreateBefore(entity);
            _Work.GetRepository<TEntity>().Insert(entity);
            _Work.SaveChanges();
            return ReturnSuccess(entity);
        }

        // POST api/entity
        [HttpPost]
        public virtual Task Post([FromBody]TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception("对象不能为空！");
            }
            OnCreateBefore(entity);
            _Work.GetRepository<TEntity>().Insert(entity);
            return _Work.SaveChangesAsync();
        }

        // PUT api/entity/5
        [HttpPut("{id}")]
        public virtual Task Put(TKey id, [FromBody]TEntity entity)
        {
            OnUpdateBefore(entity);

            _Work.GetRepository<TEntity>().Update(entity);
            return _Work.SaveChangesAsync();
        }

        // DELETE api/entity/5
        [HttpDelete("{id}")]
        public virtual Task Delete(TKey id)
        {
            _Work.GetRepository<TEntity>().Delete(id);
            return _Work.SaveChangesAsync();
        }

        protected virtual void OnCreateBefore(TEntity data)
        {
            
        }

        protected virtual void OnUpdateBefore(TEntity data)
        {

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
