using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ViewModel
{
   public class IssueRecvInfoVM
    {
       public string IssueNo { get; set; }
       public string FromLocCode { get; set; }
       public string BatchNo { get; set; }
       public string RefNo { get; set; }
       public string JobNo { get; set; }
       public DateTime RefDate { get; set; }
       public DateTime IssueDate { get; set; }
       public string LotNo { get; set; }
       public int ItemTypeCode { get; set; }
       public string ItemCode { get; set; }
       public decimal Price { get; set; }
       public decimal Qty { get; set; }
       public decimal RExtQty { get; set; }
       public decimal ExQty { get; set; }
       public string ReceiveNo { get; set; }
       public string ProducesItem { get; set; }
       public string RecvLotNo { get; set; }
       public decimal PricePerUnit { get; set; }
       public decimal ProduceQty { get; set; }
       public string ReceivedByCode { get; set; }
       public string Remarks {get; set;}
       public bool GLPost { set; get; }

    }
}
