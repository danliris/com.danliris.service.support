﻿using System;
using System.Collections.Generic;
using System.Text;

namespace com.danliris.support.lib.ViewModel
{
    public class FactBeacukaiViewModel
    {
        public string DetailshippingOrderId { get; set; }
        public string BCId { get; set; }
        public string BCNo { get; set; }
        public string BCType { get; set; }
        public DateTime BCDate { get; set; }
        public string BonNo { get; set; }
        public string BonDate { get; set; }
        public string SupllierCode { get; set; }
        public string SupplierName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitQtyName { get; set; }
        public double Quantity { get; set; }
        public decimal PricePo { get; set; }
        public decimal Rate { get; set; }
        public decimal Nominal { get; set; }
        public string CurrencyCode { get; set; }
    }
}
