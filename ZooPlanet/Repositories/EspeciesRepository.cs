using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZooPlanet.Models;

namespace ZooPlanet.Repositories
{
	public class EspeciesRepository : Repository<Especies>
	{

        public EspeciesRepository(animalesContext ctx) : base(ctx)
        {

        }


        public override IEnumerable<Especies> GetAll()
        {
            return base.GetAll().OrderBy(x => x.Especie);
        }

        public override Especies GetById(int id)
        {
            return Context.Especies.Include(x => x.IdClaseNavigation).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Especies> GetEspeciesByClase(string Id)
        {
            return Context.Especies
                .Include(x => x.IdClaseNavigation)
                .Where(x => x.IdClaseNavigation.Nombre == Id)
                .OrderBy(x => x.Especie);
        }

        public override bool Validate(Especies entidad)
        {
            if (entidad.Peso == null || entidad.Peso <= 0)
            {
                throw new Exception("Debe agregar el peso del animal");
            }
            if (entidad.Tamaño == null || entidad.Tamaño <= 0)
            {
                throw new Exception("Debe agregar el tamaño del animal");
            }

            if (string.IsNullOrWhiteSpace(entidad.Especie))
            {
                throw new Exception("Debe agregar el nombre del animal");
            }
            if (string.IsNullOrWhiteSpace(entidad.Habitat))
            {
                throw new Exception("Debe agregar el hábitat del animal");
            }
            if (string.IsNullOrWhiteSpace(entidad.Observaciones))
            {
                throw new Exception("Debe agregar las observaciones del animal");
            }
            if (Context.Especies.Any(x => x.Especie == entidad.Especie && x.Id != entidad.Id))
            {
                throw new Exception("Ya existe un animal con el mismo nombre");
            }

            if (!Context.Clase.Any(x => x.Id == entidad.IdClase))
            {
                throw new Exception("No existe la clase especificada");
            }
            return true;
        }

    }
}
