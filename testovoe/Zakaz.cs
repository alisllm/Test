using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;

namespace testovoe
{
    [Table(Name = "Zakaz")]
    public class Zakaz
    {
        [Column(IsDbGenerated = true)]
        public int Id { get; set; }


        [Column(Name = "Data")]
        public DateTime Data { get; set; }


        [Column(Name = "Summa")]
        public double Summa { get; set; }


        [Column(Name = "SummaOplat")]
        public double SummaOplat { get; set; }
    }
}
