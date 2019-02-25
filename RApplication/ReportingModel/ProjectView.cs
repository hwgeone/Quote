﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.ReportingModel
{
    public class ProjectView
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public string OpportunityOwner { get; set; }
        public string Territory { get; set; }
        /// <summary>
        /// 索引
        /// </summary>
        public string QuoteNumber { get; set; }
        public string StudySite { get; set; }
        public string ProjectLine { get; set; }
        /// <summary>
        /// 根据QuoteNumber对Amount求和
        /// 自动计算得出
        /// </summary>
        public float TotalBooking { get; set; }
        /// <summary>
        /// oracle
        /// </summary>
        public string KickOffDate { get; set; }
        /// <summary>
        /// oracle
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// oracle
        /// </summary>
        public string ProjectClosedDate { get; set; }

        /// <summary>
        /// == USD
        /// </summary>
        public string USDCurrency { get; set; }
        /// <summary>
        /// 根据QuoteNumber对CostAmount/ExchangeRate求和
        /// 自动计算得出
        /// Exchange 从ExchangeRate表中获取
        /// </summary>
        public float FinalCost { get; set; }
        public float different { get; set; }
    }
}
