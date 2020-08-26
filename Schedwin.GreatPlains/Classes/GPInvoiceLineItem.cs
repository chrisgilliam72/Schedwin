using Schedwin.Common;
using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Schedwin.GreatPlains
{
    public class GPInvoiceLineItem : ViewModelBase 
    {
        public bool IsSelected { get; set; }

        public Brush RowColor
        {
            get
            {
               switch (ItemStatus)
                {
                    case "Invoiced": return Brushes.LightGreen;
                    case "Cancelled": return Brushes.Red;
                    default: return null;
                }

            }
        }

        private String _itemStatus;
        public String ItemStatus
        {
            get
            {
                return _itemStatus;
            }
            set
            {
                _itemStatus = value;
                NotifyPropertyChanged("RowColor");
                NotifyPropertyChanged("ItemStatus");
            }
        }

        public String DocType { get; set; }

        public String TypeID { get; set; }


        public DateTime? Date { get; set; }

        private String _batchID;
        public String BatchID
        {
            get
            {
                return _batchID;
            }
            set
            {
                _batchID = value;
                NotifyPropertyChanged("BatchID");
            }
        }

        public String Company { get; set; }

        private string _debtor;
        public String Debtor
        {
            get
            {
                return _debtor;
            }
            set
            {
                _debtor = value;
                NotifyPropertyChanged("Debtor");
            }
        }


        public String SiteID { get; set; }

        public bool SiteInGP { get; set; }

        public String ItemID { get; set; }

        public String Reservation { get; set; }

        public bool Cancelled { get; set; }

        public bool FOC { get; set; }

        public String VoucherNo { get; set; }

        public String Currency { get; set; }

        public String UoM { get; set; }

        public int QTY { get; set; }

        public String Rate { get; set; }
        
        public String Amount { get; set; }

        public String TaxAmount { get; set; }

        public String TaxSchedule { get; set; }


        public String ProfitCenter { get; set; }

        public String TaxPercentage
        {
           get
            {
                double taxAmount = Convert.ToDouble(TaxAmount);
                double amount = Convert.ToDouble(Amount);
                if (amount != 0)
                    return Math.Ceiling((taxAmount / amount * 100)).ToString();
                else
                    return "0";
            }
        }

        public double InvoiceRate { get; set; }

        public String Description { get; set; }

        public int IDX_From { get; set; }

        public int IDX_TO { get; set; }

        public int IDX_ResLeg { get; set; }
        public int IDX_ResLegBudget { get; set; }

        public int IDX_CompanyID { get; set; }

        public bool IsInvoiced { get; set; }

        public bool IsScheduled { get; set; }
        public Guid? RowGuid { get; set; }

        public GPInvoiceLineItem()
        {
            Description = "";
            Debtor = "";
            IsInvoiced = false;
            IsScheduled = false;
            RowGuid = null;
            DocType = "Invoice";
            TypeID = "STD";
            TaxSchedule = "SALES STD RATE";
            TaxAmount = "0";
        }

        public static explicit operator GPInvoiceLineItem(tsch_ReservationLegs legs)
        {
            var gpLineItem = new GPInvoiceLineItem();
            gpLineItem.Date = legs.BookingDate;
            gpLineItem.Reservation = legs.tsch_ReservationHeader.Reservationname;
                 
            gpLineItem.VoucherNo = legs.Voucher;
            gpLineItem.ItemStatus = "Pending";
            gpLineItem.IDX_ResLeg = legs.IDX;
            gpLineItem.IDX_ResLegBudget = legs.IDX;
  
            gpLineItem.IDX_CompanyID = legs.tsch_ReservationHeader.IDX_Operators;

            gpLineItem.IDX_From = legs.FromAp;
            gpLineItem.IDX_TO = legs.ToAp;


            if (legs.tsch_LegsRes != null && legs.tsch_LegsRes.Count > 0)
            {
                gpLineItem.ItemID = legs.tsch_LegsRes.FirstOrDefault().tsch_AC_Pilot.tset_ACDetails.Registration;
                gpLineItem.IsScheduled = true;
            }

            else
                gpLineItem.ItemID = "Not Scheduled";

            return gpLineItem;
        }


        public static explicit operator GPInvoiceLineItem(tsch_ReservationLegBudget legBudget)
        {
            var gpLineItem = new GPInvoiceLineItem();
            gpLineItem.Date = legBudget.tsch_ReservationLegs.BookingDate;
            gpLineItem.Reservation = legBudget.tsch_ReservationLegs.tsch_ReservationHeader.Reservationname;
            gpLineItem.Rate = legBudget.Rate.ToString();
            gpLineItem.QTY = Convert.ToInt32(legBudget.Qty);
            gpLineItem.Amount = legBudget.Budget.ToString();
            gpLineItem.VoucherNo = legBudget.tsch_ReservationLegs.Voucher;
            gpLineItem.TaxAmount = (legBudget.Tax ?? 0).ToString();
            gpLineItem.ItemStatus = "Pending";
            gpLineItem.IDX_ResLeg = legBudget.tsch_ReservationLegs.IDX;
            gpLineItem.IDX_ResLegBudget = legBudget.IDX;
            gpLineItem.FOC = legBudget.FOC;
            gpLineItem.IDX_CompanyID = legBudget.tsch_ReservationLegs.tsch_ReservationHeader.IDX_Operators;
            gpLineItem.ProfitCenter = "3375";
            if (legBudget.tset_GPExportInfo!=null && legBudget.tset_GPExportInfo.Count>0)
            {
                gpLineItem.BatchID = legBudget.tset_GPExportInfo.FirstOrDefault().BatchID;
                gpLineItem.Description = legBudget.tset_GPExportInfo.FirstOrDefault().Description;
                gpLineItem.Debtor = legBudget.tset_GPExportInfo.FirstOrDefault().Debtor;
                gpLineItem.ItemStatus = "Invoiced";
                gpLineItem.IsInvoiced = true;
                gpLineItem.RowGuid = legBudget.tset_GPExportInfo.FirstOrDefault().rowguid;
            }

            gpLineItem.IDX_From = legBudget.IDXFrom;
            gpLineItem.IDX_TO = legBudget.IDXTo;
            gpLineItem.UoM = legBudget.RateType;
            gpLineItem.Currency = legBudget.Currency;
            if (legBudget.RateType.ToLower() != "seat")
                gpLineItem.ItemID = legBudget.RateType;
            else
            {
                if (legBudget.tsch_ReservationLegs.tsch_LegsRes != null && legBudget.tsch_ReservationLegs.tsch_LegsRes.Count > 0)
                {
                    gpLineItem.ItemID = legBudget.tsch_ReservationLegs.tsch_LegsRes.FirstOrDefault().tsch_AC_Pilot.tset_ACDetails.Registration;
                    gpLineItem.ProfitCenter = legBudget.tsch_ReservationLegs.tsch_LegsRes.FirstOrDefault().tsch_AC_Pilot.tset_ACDetails.ProfitCenter;
                    gpLineItem.IsScheduled = true;
                }

                else
                    gpLineItem.ItemID = "Not Scheduled";
            }

            if (legBudget.tsch_ReservationLegs.Cancelled.HasValue && legBudget.tsch_ReservationLegs.Cancelled.Value)
                gpLineItem.ItemStatus = "Cancelled";

            return gpLineItem;
        }


        public static explicit operator tset_GPExportInfo (GPInvoiceLineItem lineItem)
        {
            var exportInfo = new tset_GPExportInfo();
            exportInfo.IDX_ResLeg = lineItem.IDX_ResLeg;
            exportInfo.DocType = "Invoice";
            exportInfo.TypeID = "STD";
            exportInfo.TaxDate = lineItem.Date.Value;
            exportInfo.BatchID = lineItem.BatchID;
            exportInfo.Debtor = lineItem.Debtor;
            exportInfo.SiteID = lineItem.SiteID;
            exportInfo.Currency = lineItem.Currency;
            exportInfo.ItemID = lineItem.ItemID;
            exportInfo.UofM = lineItem.UoM;
            exportInfo.Qty = lineItem.QTY;
            exportInfo.Rate =Convert.ToDouble(lineItem.Rate);
            exportInfo.Description = lineItem.Description;
            exportInfo.TaxSched = "";
            exportInfo.TaxPer = Convert.ToDouble(lineItem.TaxPercentage);
            exportInfo.ProfitCenter = lineItem.ProfitCenter;
            exportInfo.IDX_ResLegBudget = lineItem.IDX_ResLegBudget;

            return exportInfo;

        }


    }
}
