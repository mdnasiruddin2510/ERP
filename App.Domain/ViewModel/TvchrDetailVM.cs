using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
    public class TvchrDetailVM
    {
        public TvchrDetailVM()
        {

        }
        public TvchrDetailVM(int SerialNo, int PVchrDetailId, string Accode, string Narration, double CrAmount, double DrAmount, string Sub_Ac, string DeptCode, string UnitCode, string VchrNo, string FinYear)
        {
            this.SerialNo = SerialNo;
            this.PVchrDetailId = PVchrDetailId;
            this.Accode = Accode;
            this.Narration = Narration;
            this.CrAmount = CrAmount;
            this.DrAmount = DrAmount;
            this.Sub_Ac = Sub_Ac;
            this.DeptCode = DeptCode;
            this.UnitCode = UnitCode;
            this.VchrNo = VchrNo;
            this.FinYear = FinYear;
        }
        public int SerialNo { get; set; }
        public int PVchrDetailId { get; set; }
        public string Narration { set; get; }
        public double DrAmount { set; get; }
        public double CrAmount { set; get; }
        public string Sub_Ac { set; get; }
        public string UnitCode { set; get; }
        public string DeptCode { set; get; }
        public string Accode { set; get; }
        public string VchrNo { get; set; }
        public string FinYear { get; set; }

        

    }
}
