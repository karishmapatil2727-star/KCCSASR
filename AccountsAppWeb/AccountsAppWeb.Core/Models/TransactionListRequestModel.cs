using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsAppWeb.Core.Models
{
    public class TransactionListRequestModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int VoucherTypeId { get; set; }
        public int ForInstituteId { get; set; }
        public int ToInstituteId { get; set; }
    }
}
