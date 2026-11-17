using System.ComponentModel.DataAnnotations;

namespace PrijavaIspita.Models
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class PredmetBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv predmeta je obavezan")]
        public string Naziv { get; set; } = null!;

        [Required(ErrorMessage = "Morate odabrati profesora")]
        public int ProfesorID { get; set; }

        [BindNever]
        public string? ProfesorIme { get; set; }

        [BindNever]
        public string? ProfesorPrezime { get; set; }

        public string ProfesorImePrezime => $"{ProfesorIme ?? ""} {ProfesorPrezime ?? ""}";

        [Required(ErrorMessage = "Morate odabrati smer")]
        public int SmerId { get; set; }
    }


}
