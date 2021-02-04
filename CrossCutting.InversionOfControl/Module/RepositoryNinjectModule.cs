using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interface.Repository;
using App.Domain.Interface.Repository.Common;
using Data.Repository.EntityFramework;
using Data.Repository.EntityFramework.Common;
using Ninject.Modules;
using Application.Interfaces;
using Application.Services;
using App.Domain;

namespace CrossCutting.InversionOfControl.Module
{
    public class RepositoryNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            //Security Tables
            Bind<ISecUserInfoRepository>().To<SecUserInfoRepository>();
            Bind<ISecUserGroupRepository>().To<SecUserGroupRepository>();
            Bind<ISecUserInGroupRepository>().To<SecUserInGroupRepository>();
            Bind<ISecFormListRepository>().To<SecFormListRepository>();
            Bind<ISecProcessListRepository>().To<SecProcessListRepository>();
            Bind<ISecFormProcessRepository>().To<SecFormProcessRepository>();
            Bind<ISecFormRightRepository>().To<SecFormRightRepository>();
            //Security Tables

            Bind<ITransactionLogRepository>().To<TransactionLogRepository>();
            Bind<ICommonSearchRepo>().To<CommonSearchRepo>();
            Bind<IUserProfileRepository>().To<UserProfileRepository>();
            Bind<IRecPasswordRepository>().To<RecPasswordRepository>();
            Bind<IEmailConfigRepository>().To<EmailConfigRepository>();
            Bind<IEmployeeRepository>().To<EmployeeRepository>();

            Bind<IItemTypeRepository>().To<ItemTypeRepository>();
            Bind<IUnitRepository>().To<UnitRepository>();
            Bind<IItemInfoRepository>().To<ItemInfoRepository>();
            Bind<ISubsidiaryInfoRepository>().To<SubsidiaryInfoRepository>();
            Bind<ISubsidiaryTypeRepository>().To<SubsidiaryTypeRepository>();
            Bind<IBankOperationRepository>().To<BankOperationRepository>();
            Bind<IIssueMainRepository>().To<IssueMainRepository>();
            Bind<IAcBRRepository>().To<AcBRRepository>();
            Bind<ICashReceiptSubDetailsRepository>().To<CashReceiptSubDetailsRepository>();
            Bind<ISubsidiaryCtrlRepository>().To<SubsidiaryCtrlRepository>();
            Bind<IExpenseSubDetailsRepository>().To<ExpenseSubDetailsRepository>();
            Bind<INewChartRepository>().To<NewChartRepository>();
            Bind<IFuncSLRepository>().To<FuncSLRepository>();
            Bind<IEmployeeFuncRepository>().To<EmployeeFuncRepository>();
            Bind<IIssRecSrcDestRepository>().To<IssRecSrcDestRepository>();
            
            Bind<IHORemitRepository>().To<HORemitRepository>();
            Bind<IPaymentRepository>().To<PaymentRepository>();
      
            Bind<IWithdrawRepository>().To<WithdrawRepository>();
            Bind<ICashOperationRepository>().To<CashOperationRepository>();
            Bind<ICashReceiptRepository>().To<CashReceiptRepository>();
            Bind<IExpenseRepository>().To<ExpenseRepository>();
            Bind<IDepositToBankRepository>().To<DepositToBankRepository>();

            Bind<IItemListRepository>().To<ItemListRepository>();
            Bind<ILotDTRepository>().To<LotDTRepository>();

            Bind<IBankReceiptRepository>().To<BankReceiptRepository>();

            Bind<IPVchrDetailRepository>().To<PVchrDetailRepository>();
            Bind<IPVchrMainRepository>().To<PVchrMainRepository>();
            Bind<IPVchrTrackRepository>().To<PVchrTrackRepository>();
            Bind<IJarnalVoucherRepository>().To<JarnalVoucherRepository>();
            Bind<IBankInfoRepository>().To<BankInfoRepository>();

            //Bind<IMoneyReceiptRepository>().To<CashReceiptRepository>();
            //Bind<IExpenseRepository>().To<ExpenseRepository>();
            //Bind<IDepositToBankRepository>().To<DepositToBankRepository>();
            Bind<IUserBranchRepository>().To<UserBranchRepository>();
            Bind<IvuItemInfoRepository>().To<vuItemInfoRepository>();
            Bind<ICustomerLedgerRepository>().To<CustomerLedgerRepository>();
            Bind<IAccountsReceivableRepository>().To<AccountsReceivableRepository>();
            Bind<IFYDDRepository>().To<FYDDRepository>();
            Bind<ISysSetRepository>().To<SysSetRepository>();
            Bind<ICurrentStockRepository>().To<CurrentStockRepository>();
            Bind<IReceiveRepository>().To<ReceiveRepository>();
            Bind<ILocationRepository>().To<LocationRepository>();
            Bind<IReceiveDetailsRepository>().To<ReceiveDetailsRepository>();
            Bind<IBranchRepository>().To<BranchRepository>();
            Bind<IIssueDetailsRepository>().To<IssueDetailsRepository>();
            Bind<IOpenBalRepository>().To<OpenBalRepository>();
            Bind<IOpnBalRepository>().To<OpnBalRepository>();


            Bind<ILedgerTypeRepository>().To<LedgerTypeRepository>();
            Bind<ILedgerCaptionRepository>().To<LedgerCaptionRepository>();
            Bind<IReportLedgerRepository>().To<ReportLedgerRepository>();

            Bind<ISummaryReportTypeRepository>().To<SummaryReportTypeRepository>();
            Bind<ISummaryReportRepository>().To<SummaryReportRepository>();

            Bind<ICashBookRepository>().To<CashBookRepository>();

            Bind<IJTrGrpRepository>().To<JTrGrpRepository>();
            Bind<IGsetRepository>().To<GsetRepository>();

            //nasir
            Bind<IIM_InvACRepository>().To<IM_InvACRepository>();
            Bind<IPM_IssRecRepository>().To<PM_IssRecRepository>();

            Bind<IPVchrMainExtRepository>().To<PVchrMainExtRepository>();
            Bind<IVchrMainRepository>().To<VchrMainRepository>();
            Bind<IVchrDetailRepository>().To<VchrDetailRepository>();
            Bind<ITVchrDetailRepository>().To<TVchrDetailRepository>();
            Bind<ITVchrMainRepository>().To<TVchrMainRepository>();
            Bind<IVchrPreviewVMRepository>().To<VchrPreviewVMRepository>();

            Bind<IMinLevRepository>().To<MinLevRepository>();
            Bind<ILevelLenRepository>().To<LevelLenRepository>();
            Bind<IProjInfoRepository>().To<ProjInfoRepository>();

            Bind<ICommonPVVMRepository>().To<CommonPVVMRepository>();
            Bind<ISalesMainRepository>().To<SalesMainRepository>();
            Bind<ISalesDetailRepository>().To<SalesDetailRepository>();
            Bind<IItemWiseDisVatVMRepository>().To<ItemWiseDisVatVMRepository>();
            Bind<ISubsisiaryExtRepository>().To<SubsidiaryExtRepository>();
            Bind<IBalSheetRptRepository>().To<BalSheetRptRepository>();
            Bind<IChequeReceiptRepository>().To<ChequeReceiptRepository>();
            Bind<IChequeReceiptExtRepository>().To<ChequeReceiptExtRepository>();
            Bind<IDynaCapRepository>().To<DynaCapRepository>();

            Bind<IVchrSetRepository>().To<VchrSetRepository>();
            Bind<IDefACRepository>().To<DefACRepository>();
            Bind<IMoneyReceiptRepository>().To<MoneyReceiptRepository>();
            Bind<IMoneyReceiptExtRepository>().To<MoneyReceiptExtRepository>();
            Bind<IChequeArchiveRepository>().To<ChequeArchiveRepository>();

            Bind<ItmpPurItemRepository>().To<tmpPurItemRepository>();
            Bind<IPurchaseMainRepository>().To<PurchaseMainRepository>();
            Bind<IPurchaseDetailRepository>().To<PurchaseDetailRepository>();
            Bind<IPayInvRecvRepository>().To<PayInvRecvRepository>();
            Bind<ICostLedgerRepository>().To<CostLedgerRepository>();
            Bind<IOpeningStockRepository>().To<OpeningStockRepository>();
            Bind<ICustWiseSummSaleRptRepository>().To<CustWiseSummSaleRptRepository>();
            Bind<ICustAdjustmentRepository>().To<CustAdjustmentRepository>();
            Bind<ICustAdjExtRepository>().To<CustAdjExtRepository>();
            Bind<IrptJobWiseVMRepository>().To<rptJobWiseVMRepository>();
            Bind<ISaleRetMainRepository>().To<SaleRetMainRepository>();
            Bind<ISaleRetDetailRepository>().To<SaleRetDetailRepository>();
            Bind<IPurRetMainRepository>().To<PurRetMainRepository>();
            Bind<IPurRetDetailRepository>().To<PurRetDetailRepository>();

            Bind<IDepartmentRepository>().To<DepartmentRepository>();
            Bind<IDesignationRepository>().To<DesignationRepository>();
            Bind<IMasterFormRepository>().To<MasterFormRepository>();
            Bind<IVM_BINRepository>().To<VM_BINRepository>();
            Bind<IUnitInfoRepository>().To<UnitInfoRepository>();
			Bind<IRateChartRepository>().To<RateChartRepository>();
            Bind<IPackingListRepository>().To<PackingListRepository>();
            Bind<ISubOpenBalRepository>().To<SubOpenBalRepository>();

            Bind<IMRAgainstRepository>().To<MRAgainstRepository>();
            Bind<IGroupInfoRepository>().To<GroupInfoRepository>();
            Bind<ISGroupInfoRepository>().To<SGroupInfoRepository>();
            Bind<ISSGroupInfoRepository>().To<SSGroupInfoRepository>();
            Bind<ICompanyInfoRepository>().To<CompanyInfoRepository>();
        } 
    }
}
