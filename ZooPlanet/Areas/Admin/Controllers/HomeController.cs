using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using ZooPlanet.Areas.Admin.Models.ViewModels;
using ZooPlanet.Models;
using ZooPlanet.Repositories;


namespace ZooPlanet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        animalesContext context;
        public IWebHostEnvironment Enviroment { get; set; }
        public HomeController(animalesContext ctx, IWebHostEnvironment env)
        {
            context = ctx;
            Enviroment = env;
        }


        [Route("Administrador")]
        public IActionResult Index()
        {
            EspeciesRepository repos = new EspeciesRepository(context);

            return View(repos.GetAll());

        }

        [Route("Administrador/Agregar")]
        public IActionResult Agregar()
        {
            AgregarViewModels vm = new AgregarViewModels();
            ClasesRepository repos = new ClasesRepository(context);
            vm.Clases = repos.GetAll();
            return View(vm);
        }

        [Route("Administrador/Agregar")]
        [HttpPost]
        public IActionResult Agregar(AgregarViewModels vm)
        {
            try
            {
                EspeciesRepository repos = new EspeciesRepository(context);
                repos.Insert(vm.Animal);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception error)
            {
                ModelState.AddModelError("", error.Message);
                ClasesRepository crepos = new ClasesRepository(context);
                vm.Clases = crepos.GetAll();
                return View(vm);
            }

        }


        [Route("Administrador/Editar/{id}")]
        public IActionResult Editar(int id)
        {
            EspeciesRepository repos = new EspeciesRepository(context);
            ClasesRepository clasRepos = new ClasesRepository(context);
            AgregarViewModels vm = new AgregarViewModels();
            vm.Animal = repos.GetById(id);
            vm.Clases = clasRepos.GetAll();
            if (vm.Animal == null)
            {
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return View(vm);
            }

        }

        [HttpPost]
        [Route("Administrador/Editar/{id}")]
        public IActionResult Editar(AgregarViewModels vm)
        {
            try
            {
                EspeciesRepository repos = new EspeciesRepository(context);
                ClasesRepository crepos = new ClasesRepository(context);
                var buscarAnimal = repos.GetById(vm.Animal.Id);
                vm.Clases = crepos.GetAll();
                if (vm.Animal != null)
                {
                    buscarAnimal.IdClase = vm.Animal.IdClase;
                    buscarAnimal.Habitat = vm.Animal.Habitat;
                    buscarAnimal.Especie = vm.Animal.Especie;
                    buscarAnimal.Observaciones = vm.Animal.Observaciones;
                    buscarAnimal.Peso = vm.Animal.Peso;
                    buscarAnimal.Tamaño = vm.Animal.Tamaño;
                    repos.Update(buscarAnimal);
                    return RedirectToAction("Index", "Home");
                }
                else
                    return RedirectToAction("Index", "Home");

            }
            catch (Exception error)
            {
                ModelState.AddModelError("", error.Message);
                ClasesRepository crepos = new ClasesRepository(context);
                vm.Clases = crepos.GetAll();
                return View(vm);
            }
        }

        [Route("Administrador/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            EspeciesRepository repos = new EspeciesRepository(context);
            var buscarAnimal = repos.GetById(id);
            if (buscarAnimal != null)
            {
                return View(buscarAnimal);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("Administrador/Eliminar/{id}")]
        public IActionResult Eliminar(Especies e)
        {
            EspeciesRepository repos = new EspeciesRepository(context);
            var especie = repos.GetById(e.Id);
            if (especie != null)
            {
                repos.Delete(especie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "La especie no existe, o ya sido eliminada");
                return View(e);
            }

        }


        [Route("Administrador/Imagen/{id}")]
        public IActionResult Imagen(int id)
        {
            EspeciesRepository repos = new EspeciesRepository(context);
            AgregarViewModels vm = new AgregarViewModels();
            vm.Animal = repos.GetById(id);
            if (System.IO.File.Exists(Enviroment.WebRootPath + $"/especies/{vm.Animal.Id}.jpg"))
            {
                vm.Imagen = vm.Animal.Id + ".jpg";
            }
            else
            {
                vm.Imagen = "nophoto.jpg";
            }
            return View(vm);
        }


        [HttpPost]
        [Route("Administrador/Imagen/{id}")]
        public IActionResult Imagen(AgregarViewModels vm)
        {
            try
            {
                if (vm.Archivo == null)
                {
                    ModelState.AddModelError("", "No olvide seleccionar la imagen del producto.");
                    return View(vm);
                }
                else
                {
                    if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un archivo jpg de menos de 2MB.");
                        return View(vm);
                    }
                }
                if (vm.Archivo != null)
                {
                    FileStream fs = new FileStream(Enviroment.WebRootPath + "/especies/" + vm.Animal.Id + ".jpg", FileMode.Create);
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception error)
            {
                ModelState.AddModelError("", error.Message);
                return View(vm);
            }
        }
    }
}
