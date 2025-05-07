using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeriAktarimIsci.Model
{
    public class SatirDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ManufacturerName { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string Language { get; set; } = null!;
        public decimal Price { get; set; }
        public string TopCategoryName { get; set; } = null!;
    }
}

 


