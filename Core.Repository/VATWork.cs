using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedo.Common.Models;
using Wedo.Vat.UnitOfWork;

namespace Wedo.Vat.Repository
{
    public class VATWork : UnitOfWork<VatDbContext>
    {
        public VATWork():this(new VatDbContext())
        {
            
        }

        public VATWork(VatDbContext dbContext)
            : base(dbContext)
        {
            dbContext.Configuration.LazyLoadingEnabled = false;
            dbContext.Configuration.ValidateOnSaveEnabled = false;
        }

        public  IEnumerable<CM_User> GetBPMUserQuery(string sql, params object[] parameters) {  
            return GetBPMContext().Database.SqlQuery<CM_User>(sql, parameters);
        }

        private DbContext _bpmContext;

        public DbContext GetBPMContext() {
            if (_bpmContext == null) { 
                _bpmContext= new DbContext("BPM");
            }
            return _bpmContext;
        }
    }
}
