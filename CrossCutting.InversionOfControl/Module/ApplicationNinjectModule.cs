using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Services;
using Ninject.Modules;
using App.Domain.Interface.Service;
using App.Domain.Services;

namespace CrossCutting.InversionOfControl.Module
{
    public class ApplicationNinjectModule : NinjectModule
    {
        public override void Load()
        {
            //Security Tables
            Bind<ISecUserInfoAppService>().To<SecUserInfoAppService>();
            Bind<ISecUserGroupAppService>().To<SecUserGroupAppService>();
            Bind<ISecUserInGroupAppService>().To<SecUserInGroupAppService>();
            Bind<ISecFormListAppService>().To<SecFormListAppService>();
            Bind<ISecProcessListAppService>().To<SecProcessListAppService>();
            Bind<ISecFormProcessAppService>().To<SecFormProcessAppService>();
            Bind<ISecFormRightAppService>().To<SecFormRightAppService>();
            //Security Tables
            Bind<ITransactionLogAppService>().To<TransactionLogAppService>();
            Bind<ICommonAppService>().To<CommonAppService>();
            Bind<IUserProfileAppService>().To<UserProfileAppService>();
            Bind<IRecPasswordAppService>().To<RecPasswordAppService>();
            Bind<IEmailConfigAppService>().To<EmailConfigAppService>();
            Bind<IEmployeeAppService>().To<EmployeeAppService>();

            Bind<IItemTypeAppService>().To<ItemTypeAppService>();
            Bind<IItemInfoAppService>().To<ItemInfoAppService>();
            Bind<IUnitAppService>().To<UnitAppService>();
            Bind<ISubsidiaryInfoAppService>().To<SubsidiaryInfoAppService>();
            Bind<ISubsidiaryTypeAppService>().To<SubsidiaryTypeAppService>();
            Bind<IBankOperationAppService>().To<BankOperationAppService>();
            Bind<IIssueMainAppService>().To<IssueMainAppService>();
            Bind<IAcBRAppService>().To<AcBRAppService>();
            Bind<ICashReceiptSubDetailsAppService>().To<CashReceiptSubDetailsAppService>();
            Bind<ISubsidiaryCtrlAppService>().To<SubsidiaryCtrlAppService>();
            Bind<IExpenseSubDetailsAppService>().To<ExpenseSubDetailsAppService>();
            Bind<INewChartAppService>().To<NewChartAppService>();
            Bind<IEmployeeFuncAppService>().To<EmployeeFuncAppService>();
            Bind<IFuncSLAppService>().To<FuncSLAppService>();
            Bind<IIssRecSrcDestAppService>().To<IssRecSrcDestAppService>();

            //nasir
            Bind<IIM_InvACAppService>().To<IM_InvACAppService>();
            Bind<IPM_IssRecAppService>().To<PM_IssRecAppService>();
          
            Bind<IHORemitAppService>().To<HORemitAppService>();
            Bind<IPaymentAppService>().To<PaymentAppService>();
       
            Bind<ICashReceiptAppService>().To<CashReceiptAppService>();
            Bind<IExpenseAppService>().To<ExpenseAppService>();
            Bind<IDepositToBankAppService>().To<DepositToBankAppService>();
            Bind<IWithdrawAppService>().To<WithdrawAppService>();
            Bind<ICashOperationAppService>().To<CashOperationAppService>();


            Bind<IItemListAppService>().To<ItemListAppService>();
            Bind<ILotDTAppService>().To<LotDTAppService>();


            Bind<IBankInfoAppService>().To<BankInfoAppService>();
            Bind<IJarnalVoucherAppService>().To<JarnalVoucherAppService>();
            Bind<IPVchrDetailAppService>().To<PVchrDetailAppService>();
            Bind<IPVchrMainAppService>().To<PVchrMainAppService>();
            Bind<IPVchrTrackAppService>().To<PVchrTrackAppService>();

            Bind<ISubOpenBalAppService>().To<SubOpenBalAppService>();
            Bind<IAdVGivenOPBLAppService>().To<AdVGivenOPBLAppService>();
            //Bind<IDepositToBankAppService>().To<DepositToBankAppService>();
            Bind<IUserBranchAppService>().To<UserBranchAppService>();
            Bind<IvuItemInfoAppService>().To<vuItemInfoAppService>();
            Bind<ICustomerLedgerAppService>().To<CustomerLedgerAppService>();
            Bind<IAccountsReceivableAppService>().To<AccountsReceivableAppService>();
            Bind<IFYDDAppService>().To<FYDDAppService>();
            Bind<ISysSetAppService>().To<SysSetAppService>();
            Bind<ICurrentStockAppService>().To<CurrentStockAppService>();
            Bind<IReceiveAppService>().To<ReceiveAppService>();
            Bind<ILocationAppService>().To<LocationAppService>();
            Bind<IReceiveDetailsAppService>().To<ReceiveDetailsAppService>();
            Bind<IBranchAppService>().To<BranchAppService>();
            Bind<IIssueDetailsAppService>().To<IssueDetailsAppService>();
            Bind<IOpenBalAppService>().To<OpenBalAppService>();

            Bind<IOpnBalAppService>().To<OpnBalAppService>();

            Bind<IBankReceiptAppService>().To<BankReceiptAppService>();

            Bind<ILedgerTypeAppService>().To<LedgerTypeAppService>();
            Bind<ILedgerCaptionAppService>().To<LedgerCaptionAppService>();
            Bind<IReportLedgerAppService>().To<ReportLedgerAppService>();

            Bind<ISummaryReportTypeAppService>().To<SummaryReportTypeAppService>();
            Bind<ISummaryReportAppService>().To<SummaryReportAppService>();

            Bind<IJTrGrpAppService>().To<JTrGrpAppService>();
            Bind<IGsetAppService>().To<GsetAppService>();
            Bind<ICashBookAppService>().To<CashBookAppService>();
            Bind<IPVchrMainExtService>().To<PVchrMainExtService>();
            Bind<IVchrMainService>().To<VchrMainService>();
            Bind<IVchrDetailService>().To<VchrDetailService>();
            Bind<ITVchrDetailService>().To<TVchrDetailService>();
            Bind<ITVchrMainService>().To<TVchrMainService>();
            Bind<IVchrPreviewVMService>().To<VchrPreviewVMService>();

            Bind<IMinLevAppService>().To<MinLevAppService>();
            Bind<ILevelLenAppService>().To<LevelLenAppService>();
            Bind<IProjInfoAppService>().To<ProjInfoAppService>();

            Bind<ICommonPVVMAppService>().To<CommonPVVMAppService>();
            Bind<ISalesMainAppService>().To<SalesMainAppService>();
            Bind<ISalesDetailAppService>().To<SalesDetailAppService>();
            Bind<IItemWiseDisVatAppService>().To<ItemWiseDisVatVMAppService>();
            Bind<ISubsidiaryExtAppService>().To<SubsidiaryExtAppService>();
            Bind<IBalSheetRptAppService>().To<BalSheetRptAppService>();
            Bind<IChequeReceiptExtAppService>().To<ChequeReceiptExtAppService>();
            Bind<IChequeReceiptAppService>().To<ChequeReceiptAppService>();
            Bind<IDynaCapAppService>().To<DynaCapAppService>();

            Bind<IVchrSetAppService>().To<VchrSetAppService>();
            Bind<IDefACAppService>().To<DefACAppService>();
            Bind<IMoneyReceiptAppService>().To<MoneyReceiptAppService>();
            Bind<IMoneyReceiptExtAppService>().To<MoneyReceiptExtAppService>();
            Bind<IChequeArchiveAppService>().To<ChequeArchiveAppService>();

            Bind<ItmpPurItemAppService>().To<tmpPurItemAppService>();
            Bind<IPurchaseMainAppService>().To<PurchaseMainAppService>();
            Bind<IPurchaseDetailAppService>().To<PurchaseDetailAppService>();
            Bind<IPayInvRecvAppService>().To<PayInvRecvAppService>();
            Bind<ICostLedgerAppService>().To<CostLedgerAppService>();
            Bind<IOpeningStockAppService>().To<OpeningStockAppService>();
            Bind<ICustWiseSummSaleRptAppService>().To<CustWiseSummSaleRptAppService>();
            Bind<ICustAdjustmentAppService>().To<CustAdjustmentAppService>();
            Bind<ICustAdjExtAppService>().To<CustAdjExtAppService>();
            Bind<IrptJobWiseVMAppService>().To<rptJobWiseVMAppService>();
            Bind<ISaleRetMainAppService>().To<SaleRetMainAppService>();
            Bind<ISaleRetDetailAppService>().To<SaleRetDetailAppService>();
            Bind<IPurRetMainAppService>().To<PurRetMainAppService>();
            Bind<IPurRetDetailAppService>().To<PurRetDetailAppService>();

            Bind<IDepartmentAppService>().To<DepartmentAppService>();
            Bind<IDesignationAppService>().To<DesignationAppService>();
            Bind<IVM_BINAppService>().To<VM_BINAppService>(); 
            Bind<IUnitInfoAppService>().To<UnitInfoAppService>();
            Bind<IMasterFormAppService>().To<MasterFormAppService>();
			Bind<IRateChartAppService>().To<RateChartAppService>();
            Bind<IPackingListAppService>().To<PackingListAppService>();

            Bind<IGroupInfoAppService>().To<GroupInfoAppService>();
            Bind<ISGroupInfoAppService>().To<SGroupInfoAppService>();
            Bind<ISSGroupInfoAppService>().To<SSGroupInfoAppService>();
            Bind<IMRAgainstAppService>().To<MRAgainstAppService>();
            Bind<ICompanyInfoAppService>().To<CompanyInfoAppService>();
        }
    }
}
