using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    [Table("NewChart")]
    public class NewChart
    {
        [Key]
        public string Accode { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AcSyscode { get; set; }
        public string AcName { get; set; }
        public int? ParentCode { get; set; }
        public int? LevelNo { get; set; }
        public bool? B_S { get; set; }
        public bool? I_S { get; set; }
        public bool? T_B { get; set; }
        public string ParentAcCode { get; set; }
        public string AccType { get; set; }
        public string OldCode { get; set; }
        [NotMapped]
        public string NewAcName { get; set; }
        [NotMapped]
        public bool IsModify { get; set; }
        [NotMapped]
        public string IsModifyFor { get; set; }
        [NotMapped]
        public string NodeName { get; set; }
        [NotMapped]
        public string NewOldCode { get; set; }
        [NotMapped]
        public virtual List<NewChart> children { get; set; }
        [NotMapped]
        public string text { set; get; }
        [NotMapped]
        public string perentId { set; get; }

        public string CorpCode { set; get; }
        public bool Stock { set; get; }
        public bool Subsidiary { set; get; }
        public string BranchCode { set; get; }

    }
}
