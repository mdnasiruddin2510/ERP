using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class MonthlyReturn
    {
        public int  MR_Id { get; set; }
        public byte Rev_No { get; set; }
        public DateTime Rev_Date { get; set; }

        public Boolean Accepted { get; set; }

        public string UserName { get; set; }
        public byte NumPrint { get; set; }
        public string P1_BIN { get; set; }
        public string P1_Name { get; set; }
        public string P1_Addr { get; set; }
        public string P1_Nat { get; set; }
        public string P1_FA_Nat { get; set; }
        public byte P2_TD_Mon { get; set; }
        public int P2_TD_Yr { get; set; }
        public byte P2_MR_Type { get; set; }

        public Boolean P2_Last_Act { get; set; }

        public DateTime P2_Submmit_Dt { get; set; }
        public decimal P3_DExp_Val { get; set; }
        public decimal P3_DExp_SD { get; set; }
        public decimal P3_DExp_Vat { get; set; }
        public decimal P3_IDExp_Val { get; set; }
        public decimal P3_IDExp_SD { get; set; }
        public decimal P3_IDExp_Vat { get; set; }
        public decimal P3_Ex_Val { get; set; }
        public decimal P3_Ex_SD { get; set; }
        public decimal P3_Ex_Vat { get; set; }
        public decimal P3_Ideal_Val { get; set; }
        public decimal P3_Ideal_SD { get; set; }
        public decimal P3_Ideal_Vat { get; set; }
        public decimal P3_MRP_Val { get; set; }
        public decimal P3_MRP_SD { get; set; }
        public decimal P3_MRP_Vat { get; set; }
        public decimal P3_Fixed_Val { get; set; }
        public decimal P3_Fixed_SD { get; set; }
        public decimal P3_FR_Vat { get; set; }
        public decimal P3_NIdeal_Val { get; set; }
        public decimal P3_NIdeal_SD { get; set; }
        public decimal P3_NIdeal_Vat { get; set; }
        public decimal P3_Ret_WS_Trade_Val { get; set; }
       public decimal P3_Ret_WS_Trade_SD { get; set; }
           public decimal P3_Ret_WS_Trade_Vat { get; set; }
           public decimal P4_P_Z_LT_Val { get; set; }
           public decimal P4_Z_LT_SD { get; set; }
           public decimal P4_Z_LT_Vat { get; set; }
           public decimal P4_Z_Im_Val { get; set; }
           public decimal P4_Z_Im_SD { get; set; }
           public decimal P4_Z_Im_Vat { get; set; }
           public decimal P4_Ex_LT_Val { get; set; }
           public decimal P4_Ex_LT_SD { get; set; }
           public decimal P4_Ex_LT_Vat { get; set; }
           public decimal P4_Ex_Im_Val { get; set; }
           public decimal P4_Ex_Im_SD { get; set; }
           public decimal P4_Ex_Im_Vat { get; set; }
           public decimal P4_Ideal_LT_Val { get; set; }
           public decimal P4_Ideal_LT_SD { get; set; }
           public decimal P4_Ideal_LT_Vat { get; set; }
           public decimal P4_Ideal_Im_Val { get; set; }
           public decimal P4_Ideal_Im_SD { get; set; }
           public decimal P4_Ideal_Im_Vat { get; set; }
           public decimal P4_NIdeal_LT_Val { get; set; }
           public decimal P4_NIdeal_LT_SD { get; set; }
           public decimal P4_NIdeal_LT_Vat { get; set; }
           public decimal P4_NIdeal_Im_Val { get; set; }
           public decimal P4_NIdeal_Im_SD { get; set; }
           public decimal P4_NIdeal_Im_Vat { get; set; }
           public decimal P4_Fixed_Val { get; set; }
           public decimal P4_Fixed_SD { get; set; }
           public decimal P4_FR_Vat { get; set; }
           public decimal P4_NEx_TO_Val { get; set; }
           public decimal P4_NEx_TO_SD { get; set; }
           public decimal P4_NEx_TO_Vat { get; set; }
           public decimal P4_NEx_NR_Val { get; set; }
      public decimal P4_NEx_NR_SD { get; set; }
      public decimal P4_NEx_NR_Vat { get; set; }
      public decimal P4_NEx_LT_Val { get; set; }
      public decimal P4_NEx_LT_SD { get; set; }
      public decimal P4_NEx_LT_Vat { get; set; }
      public decimal P4_NEx_IM_Val { get; set; }
      public decimal P4_NEx_IM_SD { get; set; }
      public decimal P4_NEx_IM_Vat { get; set; }
      public decimal P5_VDS_Amt { get; set; }
      public decimal P5_Non_Bk_Pay_Amt { get; set; }
      public decimal P5_DN_Amt { get; set; }
      public decimal P5_Other_Adj_Amt { get; set; }
      public decimal P6_Adj_VDS_Deli { get; set; }
      public decimal P6_Adj_AIT_Im_Paid { get; set; }
      public decimal P6_Adj_VAT_Ex_RM_Paid { get; set; }
      public decimal P6_Adj_CN_Issue { get; set; }
      public decimal P7_WO_CBAdj_VAT { get; set; }
      public decimal P7_CBAdj_VAT { get; set; }
      public decimal P7_Tot_WO_CBAdj_SD { get; set; }
      public decimal P7_Tot_CBAdj_SD { get; set; }
      public decimal P7_DN_Adjd_SD { get; set; }
      public decimal P7_CN_Adjd_SD { get; set; }
      public decimal P7_Ex_RM_Incrd_SD { get; set; }
      public decimal P7_Int_DueVAT { get; set; }
      public decimal P7_Int_DueSD { get; set; }
      public decimal P7_FIN_ChrgNComp { get; set; }
      public decimal P7_BR_Tax { get; set; }
      public decimal P7_DS_Chrg { get; set; }
      public decimal P7_ITD_SChrg { get; set; }
      public decimal P7_HP_SChrg { get; set; }
      public decimal P7_EP_SChrg { get; set; }
      public decimal P7_Last_CB_VAT { get; set; }
      public decimal P7_Last_CB_SD { get; set; }
      public decimal P8_TD_Paid_VAT { get; set; }
      public decimal P8_TD_Paid_SD { get; set; }
      public decimal P8_Due_VAT_Int { get; set; }
      public decimal P8_Due_SD_Int { get; set; }
      public decimal P8_FIN_ChrgNComp { get; set; }
      public decimal P8_BR_Tax { get; set; }
      public decimal P8_DS_Chrg { get; set; }
      public decimal P8_ITD_SChrg { get; set; }
      public decimal P8_HP_SChrg { get; set; }
      public decimal P8_EP_SChrg { get; set; }
      public decimal P8_Last_TD_CB_VAT { get; set; }
      public decimal P8_Last_TD_CB_SD { get; set; }
      public decimal P9_CB_VAT { get; set; }
      public decimal P9_CB_SD { get; set; }

      public Boolean P10_Agree { get; set; }

      public string P11_Name { get; set; }
      public string P11_Desig { get; set; }
      public DateTime P11_Date { get; set; }
      public string P11_Mobile { get; set; }
      public string P11_Email { get; set; } //VM_MonthlyReturn
        
    }
}
