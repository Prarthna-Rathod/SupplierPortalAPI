using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class EmailTemplateEntity
    {
        public int Id { get; set; }
        public string NameCode { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string? DocumentPath { get; set; }
    }
}
