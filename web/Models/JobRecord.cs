using System;

namespace web.Models
{
    public class JobRecord
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime AnchorUtc { get; set; }

        public double Duration { get; set; }

        public string Task { get; set; }

        public string Context { get; set; }
    }
}