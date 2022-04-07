using System;
using System.Data.Linq.Mapping;

namespace testovoe
{

    [Table(Name = "Prihod_Deneg")]
    public class Prihod_Deneg
    {
        [Column(IsDbGenerated = true)]
        public int Id { get; set; }


        [Column(Name = "Data")]
        public DateTime Data { get; set; }


        [Column(Name = "Summa")]
        public double Summa { get; set; }


        [Column(Name = "Ostatok")]
        public double Ostatok { get; set; }
    }
}
