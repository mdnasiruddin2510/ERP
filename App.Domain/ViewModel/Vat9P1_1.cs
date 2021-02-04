using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class Vat6P9P1_1
    {
        public string TP_BIN { get; set; }
        public string TP_Name { get; set; }
        public string TP_Addr { get; set; }
        public string TP_Nat { get; set; }
        public string TP_FA_Nat { get; set; }


        public string TD_Mon { get; set; }
        public int TD_Yr { get; set; }
        public int MR_Type { get; set; }
        public int Last_TP_Activity { get; set; }
        public string Submmit_Dt { get; set; }

        public int D_Z_DExp_Val { get; set; }
        public int D_Z_DExp_SD { get; set; }
        public int D_Z_DExp_Vat { get; set; }
        public int D_Z_IDExp_Val { get; set; }
        public int D_Z_IDExp_SD { get; set; }
        public int D_Z_IDExp_Vat { get; set; }
        public int D_Ex_Val { get; set; }
        public int D_Ex_SD { get; set; }
        public int D_Ex_Vat { get; set; }
        public int D_Ideal_Val { get; set; }
        public int D_Ideal_SD { get; set; }
        public int D_Ideal_Vat { get; set; }
        public int D_MRP_Val { get; set; }
        public int D_MRP_SD { get; set; }
        public int D_MRP_Vat { get; set; }
        public int D_Fixed_Val { get; set; }
        public int D_Fixed_SD { get; set; }
        public int D_FR_Vat { get; set; }
        public int D_NIdeal_Val { get; set; }
        public int D_NIdeal_SD { get; set; }
        public int D_NIdeal_Vat { get; set; }
        public int D_Ret_WS_Trade_Val { get; set; }
        public int D_Ret_WS_Trade_SD { get; set; }
        public int D_Ret_WS_Trade_Vat { get; set; }


        public int P_Z_LT_Val { get; set; }
        public int P_Z_LT_SD { get; set; }
        public int P_Z_LT_Vat { get; set; }
        public int P_Z_Im_Val { get; set; }
        public int P_Z_Im_SD { get; set; }
        public int P_Z_Im_Vat { get; set; }
        public int P_Ex_LT_Val { get; set; }
        public int P_Ex_LT_SD { get; set; }
        public int P_Ex_LT_Vat { get; set; }
        public int P_Ex_Im_Val { get; set; }
        public int P_Ex_Im_SD { get; set; }
        public int P_Ex_Im_Vat { get; set; }
        public int P_Ideal_LT_Val { get; set; }
        public int P_Ideal_LT_SD { get; set; }
        public int P_Ideal_LT_Vat { get; set; }
        public int P_Ideal_Im_Val { get; set; }
        public int P_Ideal_Im_SD { get; set; }
        public int P_Ideal_Im_Vat { get; set; }
        public int P_NIdeal_LT_Val { get; set; }
        public int P_NIdeal_LT_SD { get; set; }
        public int P_NIdeal_LT_Vat { get; set; }
        public int P_NIdeal_Im_Val { get; set; }
        public int P_NIdeal_Im_SD { get; set; }
        public int P_NIdeal_Im_Vat { get; set; }
        public int P_Fixed_Val { get; set; }
        public int P_Fixed_SD { get; set; }
        public int P_FR_Vat { get; set; }
        public int P_NEx_TO_Val { get; set; }
        public int P_NEx_TO_SD { get; set; }
        public int P_NEx_TO_Vat { get; set; }
        public int P_NEx_NR_Val { get; set; }
        public int P_NEx_NR_SD { get; set; }
        public int P_NEx_NR_Vat { get; set; }
        public int P_NEx_LT_Val { get; set; }
        public int P_NEx_LT_SD { get; set; }
        public int P_NEx_LT_Vat { get; set; }
        public int P_NEx_IM_Val { get; set; }
        public int P_NEx_IM_SD { get; set; }
        public int P_NEx_IM_Vat { get; set; }


        public int VDS_Amt { get; set; }
        public int Non_Bk_Pay_Amt { get; set; }
        public int DN_Amt { get; set; }
        public int Other_Adj_Amt { get; set; }


        public int Adj_VDS_Deli { get; set; }
        public int Adj_AIT_Im_Paid { get; set; }
        public int Adj_VAT_Ex_RM_Paid { get; set; }
        public int Adj_CN_Issue { get; set; }
        public int Other_Adj { get; set; }


        public int Curr_TD_Incrd_WO_CBAdj_VAT { get; set; }
        public int Curr_TD_Incrd_CBAdj_VAT { get; set; }
        public int Curr_TD_Incrd_Tot_WO_CBAdj_SD { get; set; }
        public int Curr_TD_Incrd_Tot_CBAdj_SD { get; set; }
        public int DN_Adjd_SD { get; set; }
        public int CN_Adjd_SD { get; set; }
        public int Ex_RM_Incrd_SD { get; set; }
        public int Int_DueVAT { get; set; }
        public int Int_DueSD { get; set; }
        public int FIN_ChrgNComp { get; set; }

        public int BR_Tax { get; set; }
        public int DS_Chrg { get; set; }
        public int ITD_SChrg { get; set; }
        public int HP_SChrg { get; set; }
        public int EP_SChrg { get; set; }
        public int Last_TD_CB_VAT { get; set; }
        public int Last_TD_CB_SD { get; set; }


        public int TrDep_TD_Paid_VAT { get; set; }
        public int TrDep_TD_Paid_SD { get; set; }
        public int TrDep_Due_VAT_Int { get; set; }
        public int TrDep_Due_SD_Int { get; set; }
        public int TrDep_FIN_ChrgNComp { get; set; }
        public int TrDep_BR_Tax { get; set; }
        public int TrDep_DS_Chrg { get; set; }

        public int TrDep_ITD_SChrg { get; set; }
        public int TrDep_HP_SChrg { get; set; }
        public int TrDep_EP_SChrg { get; set; }
        public int TrDep_Last_TD_CB_VAT { get; set; }
        public int TrDep_Last_TD_CB_SD { get; set; }


        public int CB_VAT { get; set; }
        public int CB_SD { get; set; }

        public int CB_Return_Agreed { get; set; }

        public string Sub_Name { get; set; }
        public string Sub_Desig { get; set; }
        public string Sub_Date { get; set; }
        public string Sub_Mobile { get; set; }
        public string Sub_Email { get; set; }

    }
}
