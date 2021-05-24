namespace QuickSoft.ScanCard.Features.Audits
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    public class Audit
    {
        public int Id { get; set; }
        public string Descriptions { get; set; }
        public string Username { get; set; }
        public DateTime CreatedDate { get; set; }
        
        [JsonIgnore]
        public DateTime ToDate { get; set; }

        [JsonIgnore]
        public DateTime FromDate { get; set; }
    }

    public record AuditEnvelope(List<Audit> Audits);
}