using ClinicDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Medicine :BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ExpiryDate { get; set; }
        public ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; } = new HashSet<PrescriptionMedicine>();
    }
}
