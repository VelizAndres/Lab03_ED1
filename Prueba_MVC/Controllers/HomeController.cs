using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prueba_MVC.Herramientas.Almacen;
using Prueba_MVC.Models;
using System.IO;

namespace Prueba_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Elección de archivo.";
            return View();
        }

 


        //Acctión para cargar los datos del archivo csv al arbol
        [HttpPost]
        public ActionResult Carga(HttpPostedFileBase postedFile)
        {
      
            string directarchivo = string.Empty;
            if(postedFile != null)
            {
                string path = Server.MapPath("~/Cargas/");
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                directarchivo = path + Path.GetFileName(postedFile.FileName);
                postedFile.SaveAs(directarchivo);
                Caja_arbol.Instance.direccion_archivo_arbol = directarchivo;
            }
            using (var archivo = new FileStream(directarchivo, FileMode.Open))
            {

                using (var archivolec = new StreamReader(archivo))
                {
                    string lector = archivolec.ReadLine();
                    int posicion = lector.Length + 2;

                    lector = archivolec.ReadLine();

                    while (lector != null)
                    {
                        int pos = int.Parse(archivo.Position.ToString());
                        string[] cajatext = lector.Split(Convert.ToChar(","));
                        mFarmaco nuevo = new mFarmaco();
                        nuevo.Nombre = cajatext[1];
                        int dispo= int.Parse(cajatext[(cajatext.Length - 1)]);
                        /* string delsimb = cajatext[(cajatext.Length - 2)];
                          var precio_simb = "";
                          for(int i=1; i<delsimb.Length;i++)
                          {
                              precio_simb += delsimb[i];
                          }
                          nuevo.Precio = double.Parse(precio_simb);*/
                        //                          nuevo.Linea = pos;
                                 nuevo.Linea = posicion;

                        posicion += lector.Length +2;
                        if (dispo > 0)
                         {
                             Caja_arbol.Instance.arbolFarm.Agregar(nuevo, mFarmaco.ComparName);

                         }
                         lector = archivolec.ReadLine();
                    }

                }
            }


            return View("Index");
        }
    }
}