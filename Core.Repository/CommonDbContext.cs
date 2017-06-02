using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedo.Common.Models;

namespace Wedo.Common.Repository
{
    public class CommonDbContext:DbContext
    {
        public CommonDbContext() : base() { }

        public CommonDbContext(string connString) : base(connString) { }

        public DbSet<CM_Module> Modules { get; set; }

        public DbSet<CM_Resource> Resources { get; set; }

        public DbSet<CM_Role> Roles { get; set; }

        public DbSet<CM_User> Users { get; set; }

        //public DbSet<CM_Role_User> Role_Users { get; set; }

        public DbSet<OperateLog> OperateLogs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new Mapping.CM_Module_Map());
            modelBuilder.Configurations.Add(new Mapping.CM_Resource_Map());
            modelBuilder.Configurations.Add(new Mapping.CM_Role_Map());

            modelBuilder.Configurations.Add(new Mapping.CM_User_Map());
            //modelBuilder.Configurations.Add(new Mapping.CM_Role_User_Map());
            modelBuilder.Configurations.Add(new Mapping.OperateLog_Mapping());

        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                List<string> errorList = new List<string>();
                foreach (var item in e.EntityValidationErrors)
                {
                    if (item.IsValid == false)
                    {
                        foreach (var m in item.ValidationErrors)
                        {
                            errorList.Add(m.ErrorMessage);
                        }
                    }
                }
                throw new Exception("Save failed：" + string.Join(".", errorList));
            }
        }

    }
}
