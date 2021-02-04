using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context.Config;
using App.Domain;


namespace Data.Context
{
    public class AcclineERPContext : BaseDbContext
    {
        #region  Constructor


        static AcclineERPContext()
        {
        }

        public AcclineERPContext()
            : base("DefaultConnection")
        {
        }

        //public AcclineERPContext()
        //    : base("data source=WIN-85S97L25TKT;initial catalog=ASPL_DEMO;user id=sa;password=;Integrated Security=false")
        //{
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.Configuration.LazyLoadingEnabled = false;
            base.OnModelCreating(modelBuilder);
            //var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance; 
        }
        #endregion

        #region DbSet

        #region Only GL
        public DbSet<AdVGivenOPBL> AdVGivenOPBL { set; get; }
        public DbSet<AcBR> AcBR { set; get; }
        public DbSet<AdjReason> AdjReason { get; set; }
        public DbSet<Branch> Branch { set; get; }
        public DbSet<BankReceipt> BankReceipt { set; get; }
        public DbSet<BankOperation> BankOperation { get; set; }
        public DbSet<BankInfo> BankInfo { get; set; }
        public DbSet<BranchLocMap> BranchLocMap { set; get; }
        public DbSet<CashReceipt> CashReceipt { set; get; }
        public DbSet<CashReceiptSubDetails> CashReceiptSubDetails { set; get; }
        public DbSet<CashOperation> CashOperation { set; get; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<CostLedger> CostLedger { get; set; }
        public DbSet<DefAC> DefAC { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Deposit> Deposit { set; get; }
        public DbSet<DynaCap> DynaCap { get; set; }
        public DbSet<ExpenseSubDetails> ExpenseSubDetails { set; get; }
        public DbSet<Expense> Expense { set; get; }
        public DbSet<EmployeeFunc> EmployeeFunc { set; get; }
        public virtual DbSet<Employee> Employee { set; get; }
        public virtual DbSet<EmailConfig> EmailConfig { set; get; }
        public DbSet<FYDD> FYDD { set; get; }
        public DbSet<FuncSL> FuncSL { set; get; }
        public DbSet<Gset> Gset { get; set; }
        public DbSet<HORemit> HORemit { set; get; }
        public DbSet<JTrGrp> JTrGrp { get; set; }
        public DbSet<JobInfo> JobInfo { get; set; }
        public virtual DbSet<TransactionLog> TransactionLog { set; get; }
        public DbSet<Location> Location { set; get; }
        public DbSet<NewChart> NewChart { set; get; }
        public DbSet<OpenBal> OpenBal { set; get; }
        public DbSet<OpeningStock> OpeningStock { set; get; }
        public DbSet<PVchrDetail> PVchrDetail { set; get; }
        public DbSet<PVchrMain> PVchrMain { set; get; }
        public DbSet<PVchrTrack> PVchrTrack { set; get; }
        public DbSet<SysSet> SysSet { set; get; }
        public DbSet<UserBranch> UserBranch { set; get; }
        public DbSet<vuItemInfo> vuItemInfo { set; get; }
        public DbSet<Withdraw> Withdraw { set; get; }
        public DbSet<LedgerType> LedgerType { get; set; }
        public DbSet<LedgerCaption> LedgerCaption { get; set; }
        public DbSet<SummaryReportType> SummaryReportType { get; set; }
        public DbSet<SummaryReport> SummaryReport { get; set; }
        public DbSet<TVchrDetail> TVchrDetail { set; get; }
        public DbSet<PVchrMainExt> PVchrMainExt { get; set; }
        public DbSet<VchrDetail> VchrDetail { get; set; }
        public DbSet<TVchrMain> TVchrMain { get; set; }
        public DbSet<MinLev> MinLev { set; get; }
        public DbSet<LevelLen> LevelLen { set; get; }
        public DbSet<ProjInfo> ProjInfo { set; get; }
        public DbSet<UnitInfo> UnitInfo { get; set; }
        public DbSet<VoucherType> VoucherType { get; set; }
        public DbSet<VchrMain> VchrMain { set; get; }
        public DbSet<VchrMainExt> VchrMainExt { set; get; }
        public DbSet<VchrSet> VchrSet { get; set; }
        public DbSet<PayInvRecv> PayInvRecv { get; set; }
        public DbSet<Payment> Payment { get; set; }
        #endregion

        #region Security Tables

        public DbSet<SecUserInfo> SecUserInfo { get; set; }
        public DbSet<SecUserGroup> SecUserGroup { get; set; }
        public DbSet<SecUserInGroup> SecUserInGroup { get; set; }
        public DbSet<SecFormList> SecFormList { get; set; }
        public DbSet<SecProcessList> SecProcessList { get; set; }
        public DbSet<SecFormProcess> SecFormProcess { get; set; }
        public DbSet<SecFormRight> SecFormRight { get; set; }

        #endregion Security Tables

        #region Others

        public DbSet<MRAgainst> MRAgainst { get; set; }
        public DbSet<CurrencyInfo> CurrencyInfo { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<GetwayProvider> GetwayProvider { get; set; }
        public DbSet<SubsidiaryInfo_Ext> SubsidiaryInfo_Ext { get; set; }
        public DbSet<SubOpenBal> SubOpenBal { set; get; }
        public DbSet<SubsidiaryCtrl> SubsidiaryCtrl { set; get; }
        public DbSet<SubsidiaryType> SubsidiaryType { set; get; }
        public DbSet<SubsidiaryInfo> SubsidiaryInfo { set; get; }
        public DbSet<ItemType> ItemType { set; get; }
        public DbSet<Unit> Unit { set; get; }
        public DbSet<ItemMap> ItemMap { get; set; }
        public DbSet<ItemInfo> ItemInfo { set; get; }
        public DbSet<IM_InvAC> IM_InvAC { set; get; }
        public DbSet<ReceiveMain> ReceiveMain { get; set; }
        public DbSet<ReceiveDetails> ReceiveDetails { set; get; }
        public DbSet<PM_IssRec> PM_IssRec { get; set; }
        public DbSet<CurrentStock> CurrentStock { set; get; }
        public DbSet<ChequeReceipt> ChequeReceipt { get; set; }
        public DbSet<ChequeReceiptExt> ChequeReceiptExt { get; set; }
        public DbSet<ChequeArchive> ChequeArchive { get; set; }
        public DbSet<IssRecSrcDest> IssRecSrcDest { get; set; }
        public DbSet<IssueDetails> IssueDetails { set; get; }
        public virtual DbSet<RecPassword> RecPasswords { set; get; }
        public DbSet<Role> Role { set; get; }
        public DbSet<LotDT> LotDT { set; get; }
        public DbSet<CustomHouse> CustomHouse { get; set; }
        public DbSet<SubsidiaryGrp> SubsidiaryGrp { get; set; }
        public DbSet<RateChart> RateChart { get; set; }

        //VM Table but useed for load DDL
        public DbSet<VM_DrCrNoteReason> VM_DrCrNoteReason { get; set; } 
        public DbSet<VM_VATType> VM_VATType { get; set; }
        public DbSet<VM_TreasuryAc> VM_TreasuryAc { get; set; }
        public DbSet<VM_AdjHead> VM_AdjHead { get; set; }
        public DbSet<CM_CostFactor> CM_CostFactor { get; set; }
        #region For MR & sales

        public DbSet<MoneyReceipt> MoneyReceipt { get; set; }
        public DbSet<MoneyReceiptExt> MoneyReceiptExt { get; set; }
        public DbSet<SalesMain> SalesMain { get; set; }
        public DbSet<SalesDetail> SalesDetail { get; set; }
        public DbSet<tempAddedItem> tempAddedItem { get; set; }
        public DbSet<PrinterSet> PrinterSet { get; set; }
        public DbSet<InvoiceCancelDrop> InvoiceCancelDrop { get; set; }  

        #endregion


        public DbSet<MasterForm> MasterForm { get; set; }
        public DbSet<CustWiseSummSaleRpt> CustWiseSummSaleRpt { get; set; }
        public DbSet<CustAdjustment> CustAdjustment { get; set; }
        public DbSet<CustAdjExt> CustAdjExt { get; set; }
        public DbSet<PurchaseMain> PurchaseMain { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetail { get; set; }
        public DbSet<tmpPurItem> tmpPurItem { get; set; }
        public DbSet<PurRetMain> PurRetMain { get; set; }
        public DbSet<PurRetDetail> PurRetDetail { get; set; }
        public DbSet<SaleRetMain> SaleRetMain { get; set; }
        public DbSet<SaleRetDetail> SaleRetDetail { get; set; }
        public DbSet<PackingList> PackingList { get; set; }
        public DbSet<GroupType> GroupType { get; set; }

        public DbSet<GroupInfo> GroupInfo { get; set; }
        public DbSet<SGroupInfo> SGroupInfo { get; set; }
        public DbSet<SSGroupInfo> SSGroupInfo { get; set; }

        #endregion

        #region For VAT

        public DbSet<VM_BIN> VM_BIN { get; set; }
        public DbSet<VM_6P2P1> VM_6P2P1 { get; set; }
      
        #endregion end VAT

        #endregion
    }
}
