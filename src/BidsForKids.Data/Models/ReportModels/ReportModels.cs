﻿using System.Collections.Generic;

namespace BidsForKids.Data.Models.ReportModels
{
    public class BaseReport
    {
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
    }

    public class ProcurementReport : BaseReport
    {
        public string ReportProcurementType { get; set; }
        public List<SerializableObjects.SerializableProcurement> rows { get; set; }
    }
    
    public class DonorReport : BaseReport
    {
        public string ReportDonorType { get; set; }
        public List<SerializableObjects.SerializableDonor> rows { get; set; }
    }

}