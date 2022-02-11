using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class DocumentVM
    {
        public Guid Id { get; set; }
        public Guid ProjectsId { get; set; }
        public string FileName { get; set; }
        public long Length { get; set; }
        public long Length_MB { get; set; }
    }
}
