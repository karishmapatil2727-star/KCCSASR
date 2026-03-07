using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsAppWeb.Core.Models
{
    public class NewResetVochers
    {
        public long NewNotiNum { get; set; }
        public int TransactionMasterId { get; set; }
        public string NewVoucherNo { get; set; }
    }

    public class ResetVocherSaveModel
    {
        public int VocherTypeId { get; set; }
        public List<NewResetVochers> NewResetVochers { get; set; }

    }
}
