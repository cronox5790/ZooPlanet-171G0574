using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooPlanet.Models;
namespace ZooPlanet.Areas.Admin.Models.ViewModels
{
    public class AgregarViewModels
    {
        public Especies Animal { get; set; }

        public IEnumerable<Clase> Clases { get; set; }

        public IFormFile Archivo { get; set; }

        public string Imagen { get; set; }
    }
}
