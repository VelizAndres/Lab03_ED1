using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prueba_MVC.Herramientas.Almacen;
using Prueba_MVC.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace Prueba_MVC.Controllers
{
    public class HomeController : Controller 
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargaArch()
        {
            ViewBag.Message = "Elección de archivo";
            return View();
        }

        public ActionResult Exportacion()
        {
            return View();
        }

        public ActionResult Export_Inorder()
        {
            string impresion = Caja_arbol.Instance.arbolFarm.ExportarInorder(mFarmaco.ObtenerNombre);
            string direc_arch = "";
            string Path = Server.MapPath("~/Orden_Expor");
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            direc_arch = Path + "Inorder_exp.txt";
            System.IO.File.Create(direc_arch).Close();
            using (StreamWriter escritor = new StreamWriter(direc_arch,false))
            {
                escritor.Write(impresion);
            }
            ViewBag.Mensaje = "Exportacion Inorder Exitosa";
            return View("Exportacion");
        }
        public ActionResult Export_Preorder()
        {
            string impresion = Caja_arbol.Instance.arbolFarm.ExportarPreorder(mFarmaco.ObtenerNombre);
            string direc_arch = "";
            string Path = Server.MapPath("~/Orden_Expor");
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            direc_arch = Path + "Preorder_exp.txt";
            System.IO.File.Create(direc_arch).Close();
            using (StreamWriter escritor = new StreamWriter(direc_arch, false))
            {
                escritor.Write(impresion);
            }
            ViewBag.Mensaje = "Exportacion Preorder Exitosa";
            return View("Exportacion");

        }
        public ActionResult Export_Postorder()
        {
            string impresion = Caja_arbol.Instance.arbolFarm.ExportarPostorder(mFarmaco.ObtenerNombre);
            string direc_arch = "";
            string Path = Server.MapPath("~/Orden_Expor");
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            direc_arch = Path + "Postorder_exp.txt";
            System.IO.File.Create(direc_arch).Close();
            using (StreamWriter escritor = new StreamWriter(direc_arch, false))
            {
                escritor.Write(impresion);
            }
            ViewBag.Mensaje = "Exportacion Postorder Exitosa";
            return View("Exportacion");
        }

        //public static List<mCompraFarmaco> GuardandoFarmaco = new List<mCompraFarmaco>();

        //public ActionResult Recarga(string id, FormCollection collection)
        //{
        //    mCompraFarmaco Recarga_Farmaco = new mCompraFarmaco()
        //    {
        //        Cantidad=collection["Cantidad"]
        //    };
        //    var contador = 0;
        //    foreach (var item in GuardandoFarmaco)
        //    {
        //        if (item.Nombre == id)
        //        {
        //            GuardandoFarmaco[contador] = Recarga_Farmaco;
        //        }
        //        else
        //        {
        //            contador++;
        //        }


        //        return View(Recarga_Farmaco);
        //}


        public ActionResult Recarga(int id, FormCollection collection)
        {
            
           
            /*
            // acceder al archivo para leer
            var file = new StreamReader("C:\\Users\\HP\\Downloads\\MOCK_DATA (4) (1).csv");

            //leemos linea 
            var linea = file.ReadLine();
            linea = file.ReadLine();
            */
            //lista guardar
            mCompraFarmaco farmaco = new mCompraFarmaco();

            /*
            //lista de datos
            var datos = new List<string>();

            //mientra no se termine el archivo
            while (linea != null)
            {
                while (linea != "")
                {
                    var coma = linea.IndexOf(',');
                    var comilla = linea.IndexOf('"');
                    // si la coma esta antes de la comilla
                    if (coma < comilla)
                    {
                        var x = linea.Substring(0, linea.IndexOf(','));
                        datos.Add(x);
                        linea = linea.Remove(0, linea.IndexOf(',') + 1);
                    }
                    else//si la comilla esta antes de la coma
                    {
                        if (comilla == -1 && coma == -1)
                        {
                            datos.Add(linea);
                            linea = "";
                            // asignas datos de la lista a el objeto
                            var nuevo = new mCompraFarmaco
                            {
                                Id = int.Parse(datos[0]),
                                Nombre = datos[1],
                                Descripcion = datos[2],
                                Casa = datos[3],
                                Precio = double.Parse(datos[4].Replace("$", "")),
                                Cantidad = int.Parse(datos[5])

                            };
                            Farmacos.Add(nuevo);
                            datos = new List<string>();
                        }
                        else if (comilla == -1)
                        {
                            var c = linea.Substring(0, linea.IndexOf(','));
                            datos.Add(c);
                            linea = linea.Remove(0, linea.IndexOf(',') + 1);
                        }
                        else
                        {

                            linea = linea.Remove(0, 1);
                            comilla = linea.IndexOf('"');
                            var x = linea.Substring(0, comilla);
                            datos.Add(x);
                            linea = linea.Remove(0, comilla + 2);
                        }
                    }
                }
                //id f
                //nombre
                //descripcion
                //casa
                //precio
                //existencia
                linea = file.ReadLine();
            }
            */
            return View(farmaco);
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
            //Modificación de los digitos de la exitencia
            List<string[]> texto = new List<string[]>();
            using (var archivo = new FileStream(directarchivo, FileMode.Open))
            {
                using (var archivolec = new StreamReader(archivo))
                {
                    string lector = archivolec.ReadLine();
                    while (lector != null)
                    {
                        Regex regx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        string[] infor_separada = regx.Split(lector);
                        if (infor_separada[infor_separada.Length - 1].Length < 2)
                        {
                            infor_separada[infor_separada.Length - 1] = "0" + infor_separada[infor_separada.Length - 1];
                        }
                        lector = archivolec.ReadLine();
                        texto.Add(infor_separada);
                    }

                }
            }
            using (var archivo = new FileStream(directarchivo, FileMode.Open))
            {
                using (var escritor = new StreamWriter(archivo))
                {
                    for (int j = 0; j < texto.Count; j++)
                    {
                        string texual = texto[j].ToString();
                        string[] contenedor = texto[j];
                        string textocompleto = contenedor[0];
                        for (int ax = 1; ax < contenedor.Length; ax++)
                        {
                            textocompleto += "," + contenedor[ax];
                        }
                        escritor.WriteLine(textocompleto);
                    }
                }
            }
            //Carga de datos al arbol
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