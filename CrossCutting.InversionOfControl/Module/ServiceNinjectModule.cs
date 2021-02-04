using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interface.Service;
using App.Domain.Interface.Service.Common;
using App.Domain.Services;
using App.Domain.Services.Common;
using Ninject.Modules;
using Application.Interfaces;
using Application.Services;

namespace CrossCutting.InversionOfControl.Module
{
    public class ServiceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IService<>)).To(typeof(Service<>));
            //Security Tables
            Bind<ISecUserInfoService>().To<SecUserInfoService>();
            Bind<ISecUserGroupService>().To<SecUserGroupService>();
            Bind<ISecUserInGroupService>().To<SecUserInGroupService>();
            Bind<ISecFormListService>().To<SecFormListService>();
            Bind<ISecProcessListService>().To<SecProcessListService>();
            Bind<ISecFormProcessService>().To<SecFormProcessService>();
            Bind<ISecFormRightService>().To<SecFormRightService>();
            //Security Tables
            Bind<ITransactionLogService>().To<TransactionLogService>();
            Bind<ICommonSearchService>().To<CommonSearchService>();
            Bind<IUserProfileService>().To<UserProfileService>();
            Bind<IRecPasswordService>().To<RecPasswordService>();
            Bind<IEmailConfigService>().To<EmailConfigService>();
            Bind<IEmployeeService>().To<EmployeeService>();


            Bind<IItemTypeService>().To<ItemTypeService>();
            Bind<IUnitService>().To<UnitService>();
            Bind<IItemInfoService>().To<ItemInfoService>();
            Bind<IBankOperationService>().To<BankOperationService>();
            Bind<ISubsidiaryInfoService>().To<SubsidiaryInfoService>();
            Bind<ISubsidiaryTypeService>().To<SubsidiaryTypeService>();
            Bind<IIssueMainService>().To<IssueMainService>();
            Bind<IAcBRService>().To<AcBRService>();
            Bind<IFuncSLService>().To<FuncSLService>();
            Bind<IEmployeeFuncService>().To<EmployeeFuncService>();
            Bind<IIssRecSrcDestService>().To<IssRecSrcDestService>();
            Bind<INewChartService>().To<NewChartService>();

            Bind<IHORemitService>().To<HORemitService>();
            Bind<IPaymentService>().To<PaymentService>();
       
            Bind<ICashReceiptService>().To<CashReceiptService>();
            Bind<IExpenseService>().To<ExpenseService>();
            Bind<IDepositToBankService>().To<DepositToBankService>();
            Bind<IWithdrawService>().To<WithdrawService>();
            Bind<ICashOperationService>().To<CashOperationService>();
            Bind<IExpenseSubDetailsService>().To<ExpenseSubDetailsService>();

            Bind<IItemListService>().To<ItemListService>();
            Bind<ILotDTService>().To<LotDTService>();
            Bind<IBankReceiptService>().To<BankReceiptService>();

            Bind<IBankInfoService>().To<BankInfoService>();
            Bind<IJarnalVoucherService>().To<JarnalVoucherService>();
            Bind<IPVchrDetailService>().To<PVchrDetailService>();
            Bind<IPVchrMainService>().To<PVchrMainService>();
            Bind<IPVchrTrackService>().To<PVchrTrackService>();

            Bind<ICashReceiptSubDetailsService>().To<CashReceiptSubDetailsService>();
            Bind<ISubsidiaryCtrlService>().To<SubsidiaryCtrlService>();
            //Bind<ISysSetService>().To<SysSetService>();
            //Bind<IMoneyReceiptService>().To<MoneyReceiptService>();
            //Bind<IExpenseService>().To<ExpenseService>();
            Bind<IUserBranchService>().To<UserBranchService>();
            Bind<IvuItemInfoService>().To<vuItemInfoService>();
            Bind<IJTrGrpService>().To<JTrGrpService>();
            Bind<ICustomerLedgerService>().To<CustomerLedgerService>();
            Bind<IAccountsReceivableService>().To<AccountsReceivableService>();
            Bind<IFYDDService>().To<FYDDService>();
            Bind<ISysSetService>().To<SysSetService>();
            Bind<ICurrentStockService>().To<CurrentStockService>();
            Bind<IReceiveService>().To<ReceiveService>();
            Bind<ILocationService>().To<LocationService>();
            Bind<IReceiveDetailsService>().To<ReceiveDetailsService>();
            Bind<IBranchService>().To<BranchService>();
            Bind<IIssueDetailsService>().To<IssueDetailsService>();
            Bind<IOpenBalService>().To<OpenBalService>();
            Bind<IOpnBalService>().To<OpnBalService>();

            Bind<ILedgerTypeService>().To<LedgerTypeService>();
            Bind<ILedgerCaptionService>().To<LedgerCaptionService>();
            Bind<IReportLedgerService>().To<ReportLedgerService>();

            Bind<ISummaryReportTypeService>().To<SummaryReportTypeService>();
            Bind<ISummaryReportService>().To<SummaryReportService>();

            Bind<ICashBookService>().To<CashBookService>();

           // Bind<IJTrGrpService>().To<JTrGrpService>();
            Bind<IGsetService>().To<GsetService>();

            //nasir
            Bind<IIM_InvACService>().To<IM_InvACService>();
            Bind<IPM_IssRecService>().To<PM_IssRecService>();

            Bind<IPVchrMainExtAppService>().To<PVchrMainExtAppService>();
            Bind<IVchrMainAppService>().To<VchrMainAppService>();
            Bind<IVchrDetailAppService>().To<VchrDetailAppService>();
            Bind<ITVchrDetailAppService>().To<TVchrDetailAppService>();
            Bind<ITVchrMainAppService>().To<TVchrMainAppService>();
            Bind<IVchrPreviewVMAppService>().To<VchrPreviewVMAppService>();

            Bind<IMinLevService>().To<MinLevService>();
            Bind<ILevelLenService>().To<LevelLenService>();
            Bind<IProjInfoService>().To<ProjInfoService>();

            Bind<ICommonPVVMService>().To<CommonPVVMService>();
            Bind<ISalesMainService>().To<SalesMainService>();
            Bind<ISalesDetailService>().To<SalesDetailService>();
            Bind<IItemWiseDisVatVMService>().To<ItemWiseDisVatVMService>();
            Bind<ISubsidiaryExtService>().To<SubsidiaryExtService>();
            Bind<IBalSheetRptService>().To<BalSheetRptService>();

            Bind<IChequeReceiptService>().To<ChequeReceiptService>();
            Bind<IChequeReceiptExtService>().To<ChequeReceiptExtService>();
            Bind<IDynaCapService>().To<DynaCapService>();

            Bind<IVchrSetService>().To<VchrSetService>();
            Bind<IDefACService>().To<DefACService>();
            Bind<IMoneyReceiptService>().To<MoneyReceiptService>();
            Bind<IMoneyReceiptExtService>().To<MoneyReceiptExtService>();
            Bind<IChequeArchiveService>().To<ChequeArchiveService>();

            Bind<ItmpPurItemService>().To<tmpPurItemService>();
            Bind<IPurchaseMainService>().To<PurchaseMainService>();
            Bind<IPurchaseDetailService>().To<PurchaseDetailService>();
            Bind<IPayInvRecvService>().To<PayInvRecvService>();
            Bind<ICostLedgerService>().To<CostLedgerService>();
            Bind<IOpeningStockService>().To<OpeningStockService>();
            Bind<ICustWiseSummSaleRptService>().To<CustWiseSummSaleRptService>();
            Bind<ICustAdjustmentService>().To<CustAdjustmentService>();
            Bind<ICustAdjExtService>().To<CustAdjExtService>();
            Bind<IrptJobWiseVMService>().To<rptJobWiseVMService>(); 
            Bind<ISaleRetMainService>().To<SaleRetMainService>();
            Bind<ISaleRetDetailService>().To<SaleRetDetailService>();
            Bind<IPurRetMainService>().To<PurRetMainService>();
            Bind<IPurRetDetailService>().To<PurRetDetailService>();

            Bind<IUnitInfoService>().To<UnitInfoService>();
            Bind<IDepartmentService>().To<DepartmentService>();
            Bind<IDesignationService>().To<DesignationService>();
            Bind<IMasterFormService>().To<MasterFormService>();
            Bind<IVM_BINService>().To<VM_BINService>();
			Bind<IRateChartService>().To<RateChartService>();
            Bind<IPackingListService>().To<PackingListService>();
            Bind<ISubOpenBalService>().To<SubOpenBalService>();

            Bind<IMRAgainstService>().To<MRAgainstService>();
            Bind<IGroupInfoService>().To<GroupInfoService>();
            Bind<ISGroupInfoService>().To<SGroupInfoService>();
            Bind<ISSGroupInfoService>().To<SSGroupInfoService>();
            Bind<ICompanyInfoService>().To<CompanyInfoService>();
        }
    }
}
