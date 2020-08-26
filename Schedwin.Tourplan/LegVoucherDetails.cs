using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Tourplan
{
    public class LegVoucherDetails
    {
        public String TPReference { get; set; }
        public DateTime Date { get; set; }
        public String ReservationName { get; set; }
        public String VoucherNo { get; set; }      
        public String Currency { get; set; }
        public String AreaFrom { get; set; }        
        public String AreaTo { get; set; }
        public bool IsSoleUse { get; set; }
        public String AP1 { get; set; }
        public String AP2 { get; set; }
        public decimal VoucherAmount { get; set; }

        public decimal SchedwinAmount { get; set; }
        public decimal Cost
        {
            get
            {
                decimal vatAmount = (Convert.ToDecimal(VATPercentage) / 100) + 1;
                return VoucherAmount / vatAmount;
        
            }
        }

        public decimal VATAmount
        {
            get
            {
                return VoucherAmount - Cost;
            }
        }

        public int VATPercentage { get; set; }

        public String VoucherCode { get; set; }
        public bool AreaToArea { get; set; }

        LegVoucherDetails()
        {
          
            VATPercentage = 0;
            IsSoleUse = false;
        }


        public static explicit operator LegVoucherDetails(Schedwin_Vouchers dbVoucher)
        {
            var legVoucher = new LegVoucherDetails();
            
            legVoucher.Date = dbVoucher.DATE;
            legVoucher.ReservationName = dbVoucher.NAME;
            legVoucher.VoucherNo = dbVoucher.Voucher.ToString();
            legVoucher.Currency = dbVoucher.Currency;
            legVoucher.VoucherAmount = dbVoucher.Cost ?? 0;
            legVoucher.SchedwinAmount = legVoucher.VoucherAmount;
            legVoucher.TPReference = dbVoucher.Reference;
            legVoucher.VoucherCode = dbVoucher.CODE.TrimEnd(' ');
            var codes = dbVoucher.CODE.Split('/');

            if (codes.Length > 1)
            {
                legVoucher.AreaToArea = true;
                legVoucher.AreaFrom = codes[0].Replace("$", String.Empty);
                legVoucher.AreaTo = codes[1].Replace("$", String.Empty);
            }
            else
            {
                legVoucher.VoucherCode = legVoucher.VoucherCode.TrimEnd('-');
                legVoucher.VoucherCode = legVoucher.VoucherCode.TrimEnd('.');
                legVoucher.AreaToArea = false;
            }
               


            return legVoucher;

        }

        public static async Task<List<LegVoucherDetails>> GetReservationVouchers(List<String> tourplanRefs,int VATPercentage, String Server, String databaseName)
        {
#if DEBUG
            Server = "stratus";
#endif
            var conString = RegionalConnectionGenerator.GetConnectionString(Server, databaseName);
            var ctx = new TPRegionalEntities(conString);
            using (ctx)
            {
               var dbVouchers= await ctx.Schedwin_Vouchers.Where(x =>tourplanRefs.Contains(x.Reference) && x.SERVICE=="CH").ToListAsync();
               var legVouchers = dbVouchers.Select(x => (LegVoucherDetails)x).ToList();
                if (VATPercentage >0)
                {
                    foreach (var legVoucher in legVouchers)
                        legVoucher.VATPercentage = VATPercentage;
                }

                return legVouchers;
            }
        }
    }
}
