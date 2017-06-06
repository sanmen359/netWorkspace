using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api
{
    public class ResponseResult<T>
    {
        public bool Success { get; set; }

        public string MSG { get; set; }

        public T Data { get; set; }
    }

    public class QueryModel
    {
        public string Keyword { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }

    public class TimeQueryModel
    {
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Keyword { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
