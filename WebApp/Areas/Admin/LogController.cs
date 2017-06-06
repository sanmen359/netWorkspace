using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Modes;
using Web.Api;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin
{
    [Produces("application/json")]
    [Route("api/admin/[controller]/[action]")]
    public class LogController : EntityController<Log, int>
    {
        public LogController(IUnitOfWork Work):base(Work)
        {
        }

        [HttpPost]
        public Task<IEnumerable<Log>> Query([FromBody]TimeQueryModel filter)
        {
            return GetQuery(filter).ToPagedAsync(filter.PageIndex, filter.PageSize);
        }

        [HttpPost]
        public Task<int> QueryCount([FromBody]TimeQueryModel filter)
        { 
            return GetQuery(filter).CountAsync();
        }

        private IQueryable<Log> GetQuery(TimeQueryModel filter)
        {
            var startTime = filter.StartTime ?? DateTime.Now.AddDays(-1).Date;
            var endTime = filter.EndTime ?? DateTime.Now.AddDays(1).Date;
            var query = _Repository.Query(m => m.Time < endTime && m.Time > startTime);
            if (string.IsNullOrWhiteSpace(filter.Keyword)==false){
                query = query.Where(m => m.Message.Contains(filter.Keyword));
            }
            return query;
        }
    }
}