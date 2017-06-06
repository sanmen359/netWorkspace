using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Modes;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Api
{
    [Route("api/[controller]/[action]")]
    public class DictionaryController : EntityController<DataDictionary,Guid>
    {
        public DictionaryController(IUnitOfWork Work):base(Work)
        {
        }

        // GET: api/Dictionary/Query
        [HttpGet]
        public IEnumerable<DataDictionary> Query()
        { 
            return _Repository.Query(m=>m.ParentId.HasValue==false);
        }

        public List<Tree> GetTree()
        {
            var parents = _Repository.Query(m => m.ParentId.HasValue == false).Select(m=>new Tree { id=m.Id,label=m.Name,code=m.Code}).ToList();
            foreach(var item in parents)
            {
                item.children= _Repository.Query(m => m.ParentId.HasValue && m.ParentId.Value==item.id).Select(m => new Tree { id = m.Id, label = m.Name, code = m.Code,type="type" }).ToList();
                item.type = "category";
            }
            return parents;
        }

        [HttpGet("{id}")]
        public Task UpdateTree(Guid Id, [FromBody]Tree tree)
        {
            var model = _Repository.Find(Id);
            if (model == null)
            {
                throw new Exception(string.Format("{0} is not in database",Id));
            }
            else
            {
                model.Code = tree.code;
                model.Name = tree.label;
                model.Value = tree.label;
                Check(model);
                return _Work.SaveChangesAsync();
            }
        }

        [HttpPost("{id?}")]
        public Guid AddTree(Guid? id, [FromBody]Tree tree)
        {
            var newData = new DataDictionary { Code = tree.code, Name = tree.label, Value = tree.label, ParentId = id, Id = Guid.NewGuid() };
            _Repository.Insert(newData);
            _Work.SaveChanges();
            return newData.Id;
        }

        [HttpPost]
        public Task< IEnumerable<DataDictionary>>  GetItemByParentId([FromBody]QueryModel filter)
        {
            var parentID = Guid.Parse(filter.Keyword);
            return _Repository.Query(m => m.ParentId == parentID).OrderBy(m=>m.Order)
                .ToPagedAsync( filter.PageIndex,filter.PageSize);
        }

        [HttpDelete("{id}")]
        public Task DeleteTree(Guid id)
        {
            _Repository.Delete(id);
            return _Work.SaveChangesAsync();
        }
        protected override void OnCreateBefore(DataDictionary data)
        {
            Check(data);
        }
        protected override void OnUpdateBefore(DataDictionary data)
        {
            
            Check(data);
        }

        protected void Check(DataDictionary data)
        {
            var exist = _Repository.Query(m => m.Code == data.Code && m.Id != data.Id).Count() > 0;
            if (exist)
            {
                throw new Exception("已存在此CODE，不能保存.");
            }
        }

    }

    public class Tree
    {
        public Guid id { get; set; }

        public string label { get; set; }

        public string code { get; set; }

        public string type { get; set; }

        public List<Tree> children { get; set; }
    }

}
