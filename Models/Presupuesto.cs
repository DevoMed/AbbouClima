using System.ComponentModel.DataAnnotations;

namespace AbbouClima.Models
{
    public class Presupuesto
    {
        [Key] // Llave primaria
        public Guid Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [Display(Name = "Nº de Presupuesto")]
        public string? NºPresupuesto { get; set; }

        [Display(Name = "Fecha del Presupuesto")]
        public string? FechaPresupuesto { get; set; }
        public string? Descripcion1 { get; set; }
        public string? Descripcion2 { get; set; }
        public string? Descripcion3 { get; set; }
        public string? Descripcion4 { get; set; }
        public string? Descripcion5 { get; set; }
        public string? Descripcion6 { get; set; }
        public string? Descripcion7 { get; set; }
        public string? Descripcion8 { get; set; }
        public string? Descripcion9 { get; set; }
        public string? Descripcion10 { get; set; }
        public int? Cantidad1 { get; set; }
        public int? Cantidad2 { get; set; }
        public int? Cantidad3 { get; set; }
        public int? Cantidad4 { get; set; }
        public int? Cantidad5 { get; set; }
        public int? Cantidad6 { get; set; }
        public int? Cantidad7 { get; set; }
        public int? Cantidad8 { get; set; }
        public int? Cantidad9 { get; set; }
        public int? Cantidad10 { get; set; }
        public int? Precio1 { get; set; }
        public int? Precio2 { get; set; }
        public int? Precio3 { get; set; }
        public int? Precio4 { get; set; }
        public int? Precio5 { get; set; }
        public int? Precio6 { get; set; }
        public int? Precio7 { get; set; }
        public int? Precio8 { get; set; }
        public int? Precio9 { get; set; }
        public int? Precio10 { get; set; }
        public int? Total1 => Cantidad1 * Precio1;
        public int? Total2 => Cantidad2 * Precio2;
        public int? Total3 => Cantidad3 * Precio3;
        public int? Total4 => Cantidad4 * Precio4;
        public int? Total5 => Cantidad5 * Precio5;
        public int? Total6 => Cantidad6 * Precio6;
        public int? Total7 => Cantidad7 * Precio7;
        public int? Total8 => Cantidad8 * Precio8;
        public int? Total9 => Cantidad9 * Precio9;
        public int? Total10 => Cantidad10 * Precio10;

        [Display(Name = "Importe Total")]
        public decimal? ImporteTotal { get; set; }
        public bool IncludeIVA { get; set; }
		public bool Enviado { get; set; }
		public int? Validez { get; set; }
        public string? Observaciones { get; set; }

        [Display(Name = "Importe Total Sin Iva")]
        public int? TotalSinIVA =>  (Total1 ?? 0) + (Total2 ?? 0) + (Total3 ?? 0) +
                                        (Total4 ?? 0) + (Total5 ?? 0) + (Total6 ?? 0) +
                                        (Total7 ?? 0) + (Total8 ?? 0) + (Total9 ?? 0) +
                                        (Total10 ?? 0);










    }
}
