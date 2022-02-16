using System;
using System.Collections.Generic;

#nullable disable

namespace AspFile.Models
{
    public partial class File
    {
        public int Id { get; set; }
        public byte[] Filedb { get; set; }
    }
}
