using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Modes
{
    public class DataDictionary:IEnitity<Guid>
    {
        public Guid? ParentId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Remark { get; set; }

        public int Order { get; set; }

        public Guid ID { get; set; }
    }
}
