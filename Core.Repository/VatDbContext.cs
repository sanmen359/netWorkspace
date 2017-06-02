using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedo.Vat.Models;
using Wedo.Common.Repository;

namespace Wedo.Vat.Repository
{
    public class VatDbContext : CommonDbContext
    {

        public VatDbContext() : this("CommonDbContext") { }

        public VatDbContext(string connString) : base(connString) {
            Configuration.ValidateOnSaveEnabled = false;
        }

        /// <summary>
        /// Billing
        /// </summary>
        public DbSet<Billing> Billing { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public DbSet<Material> Material { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public DbSet<Miscellaneous> Miscellaneous { get; set; }

        /// <summary>
        /// 申请
        /// </summary>
        public DbSet<VatApplication> VatApplication { get; set; }

        /// <summary>
        /// 货运记录
        /// </summary>
        public DbSet<ShippingRecord> ShippingRecords { get; set; }

        /// <summary>
        /// 销货公司
        /// </summary>
        public DbSet<SaleCompany> Sales { get; set; }

        /// <summary>
        /// 发票
        /// </summary>
        public DbSet<Invoice> Invoices { get; set; }

        /// <summary>
        /// 发票项
        /// </summary>
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        
        /// <summary>
        /// 货运公司
        /// </summary>
        public DbSet<TransportCompany> TransportCompanies { get; set; }

        /// <summary>
        /// 审批过滤
        /// </summary>
        public DbSet<ApproverSetting> ApproverSettings { get; set; }

        /// <summary>
        /// 文件记录
        /// </summary>
        public DbSet<FileRecord> FileRecords { get; set; }

        /// <summary>
        /// 内部销货单位
        /// </summary>
        public DbSet<InnerCustomer> InnerCustomers { get; set; }

        /// <summary>
        /// 内部销货单位
        /// </summary>
        public DbSet<Unit> Units { get; set; }
        /// <summary>
        /// 最大差额限制
        /// </summary>
        public DbSet<MaxAmountSetting> MaxAmountSettings { get; set; }

        /// <summary>
        /// 自动申请设置
        /// </summary>
        public DbSet<AutoAppSetting> AutoAppSettings { get; set; }

        /// <summary>
        /// 预申请
        /// </summary>
        public DbSet<PreVatApplication> PreVat { get; set; }

        /// <summary>
        /// 退票
        /// </summary>
        public DbSet<ReturnVat> ReturnVat { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public DbSet<CommonAttach> Attachs { get; set; }

        /// <summary>
        /// 暂不申请客户
        /// </summary>
        public DbSet<NotInvoiceCustomer> NotInvoiceCustomers { get; set; }

        /// <summary>
        /// 自动月结客户
        /// </summary>
        public DbSet<AutoMonthlySetting> AutoMonthlySettings { get; set; }

        /// <summary>
        /// 先对账后开票客户
        /// </summary>
        public DbSet<ReconciliationCustomer> ReconciliationCustomers { get; set; }

        public DbSet<BillingItem> BillingItems { get; set; }

        public DbSet<SuperCSRSetting> SuperCSRSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new Mapping.Billing_Mapping());
            modelBuilder.Configurations.Add(new Mapping.Material_Mapping());
            modelBuilder.Configurations.Add(new Mapping.Miscellaneous_Mapping());
            modelBuilder.Configurations.Add(new Mapping.VatApplication_Mapping());

            modelBuilder.Configurations.Add(new Mapping.InvoiceItem_Mapping());
            modelBuilder.Configurations.Add(new Mapping.BillingItem_Mapping());
            modelBuilder.Configurations.Add(new Mapping.Invoice_Mapping());
            modelBuilder.Configurations.Add(new Mapping.ShippingRecord_Mapping());
            modelBuilder.Configurations.Add(new Mapping.SaleCompany_Mapping());

            modelBuilder.Configurations.Add(new Mapping.Saler_Mapping());
            modelBuilder.Configurations.Add(new Mapping.Buyer_Mapping());
            modelBuilder.Configurations.Add(new Mapping.Receive_Mapping());
            modelBuilder.Configurations.Add(new Mapping.TransportCompany_Mapping());
            modelBuilder.Configurations.Add(new Mapping.ApproverSetting_Mapping());
            modelBuilder.Configurations.Add(new Mapping.FileRecord_Mapping());

            modelBuilder.Configurations.Add(new Mapping.InnerCustomer_Mapping());
            modelBuilder.Configurations.Add(new Mapping.Unit_Mapping());
            modelBuilder.Configurations.Add(new Mapping.AutoAppSetting_Mapping());
            modelBuilder.Configurations.Add(new Mapping.MaxAmountSetting_Mapping());

            modelBuilder.Configurations.Add(new Mapping.ReturnVat_Mapping());
            modelBuilder.Configurations.Add(new Mapping.PreVatApplication_Mapping());
            modelBuilder.Configurations.Add(new Mapping.Attach_Mapping());
            modelBuilder.Configurations.Add(new Mapping.NotInvoiceCustomer_Mapping());
            modelBuilder.Configurations.Add(new Mapping.AutoMonthlySetting_Mapping());

            modelBuilder.Configurations.Add(new Mapping.ReconciliationCustomer_Mapping());
            modelBuilder.Configurations.Add(new Mapping.SuperCSRSetting_Mapping());

            modelBuilder.Ignore<BuyerSource>();
        }

    }
}
