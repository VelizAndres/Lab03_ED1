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
    public class PedidoController : Controller
    {
        // GET: Pedido
        public ActionResult Venta()
        {
            mPedido comprador = new mPedido();
            comprador.Pedidos = CarritoCompra.Instance.carrito;
            ViewBag.TotalCompra= CarritoCompra.Instance.total;

            int cant_produc = CarritoCompra.Instance.carrito.Count;
            var item = CarritoCompra.Instance.carrito.First;
            for (int num_prod = 0; num_prod < cant_produc; num_prod++)
            {
                CarritoCompra.Instance.carrito.Remove(item);
                item = item.Next;
            }
            CarritoCompra.Instance.total = 0;
            return View(comprador);
        }

        public ActionResult VentasHoy()
        {
            return View(ListaPedidos.Instance.pedidos_del_dia);
        }

        public ActionResult BuscarFarmaco()
        {
            try
            {
                int cant_produc = CarritoCompra.Instance.carrito.Count;
                var item = CarritoCompra.Instance.carrito.First;
                for (int num_prod = 0; num_prod < cant_produc; num_prod++)
                {
                    Delete(item.Value.Nombre);
                    item = item.Next;
                }
                return View(CarritoCompra.Instance.carrito);
            }
            catch
            {
                return View("CargaArch", "Home");
            }
        }



        public ActionResult Busqueda(string Texto)
        {
            try {
                mFarmaco farmaco = new mFarmaco();
                farmaco.Nombre = Texto;
                farmaco = Caja_arbol.Instance.arbolFarm.Buscar(farmaco, mFarmaco.ComparName);
                int linea_buscad = farmaco.Linea;
                string info_farmaco = "";
                using (FileStream archivo = new FileStream(Caja_arbol.Instance.direccion_archivo_arbol, FileMode.Open))
                {
                    using (StreamReader lector = new StreamReader(archivo))
                    {
                        archivo.Seek(linea_buscad, SeekOrigin.Begin);
                        info_farmaco = lector.ReadLine();
                    }
                }
                Regex regx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                string[] infor_separada = regx.Split(info_farmaco);
                ViewBag.Nombre = infor_separada[1];
                ViewBag.Descripción = infor_separada[2];
                ViewBag.Productora = infor_separada[3];
                ViewBag.Precio = infor_separada[4];
                ViewBag.Existencia = infor_separada[5];
                CarritoCompra.Instance.quizascompra = infor_separada[1];
                if (CarritoCompra.Instance.total < 0)
                {
                    CarritoCompra.Instance.total=0;
                }
                ViewBag.TotalCompra = CarritoCompra.Instance.total;
                return View("BuscarFarmaco", CarritoCompra.Instance.carrito);
            }
            catch
            {
                if (CarritoCompra.Instance.total < 0)
                {
                    CarritoCompra.Instance.total = 0;
                }
                ViewBag.TotalCompra = CarritoCompra.Instance.total;
                return View("BuscarFarmaco", CarritoCompra.Instance.carrito); 
            }
       }

        [HttpPost]
        public ActionResult Venta(FormCollection collection)
        {
            try
            {
                var Pedido = new mPedido
                {
                    Name = collection["Name"],
                    Direccion = collection["Direccion"],
                    Nit = collection["Nit"],
                    Total = CarritoCompra.Instance.total,
                    Pedidos = CarritoCompra.Instance.carrito
                };
               ListaPedidos.Instance.pedidos_del_dia.AddFirst(Pedido);
            return View("VentasHoy",ListaPedidos.Instance.pedidos_del_dia);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Agregar(string cantidad)
        {
            try {
                mFarmaco farmaco = new mFarmaco();
                farmaco.Nombre = CarritoCompra.Instance.quizascompra;
                farmaco = Caja_arbol.Instance.arbolFarm.Buscar(farmaco, mFarmaco.ComparName);
                int linea_buscad = farmaco.Linea;
                string info_farmaco = "";
                using (FileStream archivo = new FileStream(Caja_arbol.Instance.direccion_archivo_arbol, FileMode.Open))
                {
                    using (StreamReader lector = new StreamReader(archivo))
                    {
                        archivo.Seek(linea_buscad, SeekOrigin.Begin);
                        info_farmaco = lector.ReadLine();
                        linea_buscad += info_farmaco.Length;
                    }
                }
                Regex regx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                string[] infor_separada = regx.Split(info_farmaco);
                if (int.Parse(cantidad) <= int.Parse(infor_separada[infor_separada.Length - 1]))
                {
                    mCompraFarmaco FarmacoNuevo = new mCompraFarmaco();
                    FarmacoNuevo.Nombre = infor_separada[1];
                    string delsimb = infor_separada[4];
                    var precio_simb = "";
                    for (int i = 1; i < delsimb.Length; i++)
                    {
                        precio_simb += delsimb[i];
                    }
                    FarmacoNuevo.Precio_uni = double.Parse(precio_simb);
                    FarmacoNuevo.Cantidad = int.Parse(cantidad);
                    FarmacoNuevo.Subtotal = FarmacoNuevo.Precio_uni * int.Parse(cantidad);
                    CarritoCompra.Instance.carrito.AddFirst(FarmacoNuevo);
                    CarritoCompra.Instance.total += FarmacoNuevo.Subtotal;





                    //Se procede a la eliminación de producto comprado en la exitencia
                    using (var archivo = new FileStream(Caja_arbol.Instance.direccion_archivo_arbol, FileMode.Open))
                    {
                        using (var escritor = new StreamWriter(archivo))
                        {
                            int posicion_existencia = linea_buscad - infor_separada[infor_separada.Length - 1].Length;
                            archivo.Seek(posicion_existencia, SeekOrigin.Begin);
                            string existencia_actual = Convert.ToString(int.Parse(infor_separada[infor_separada.Length - 1]) - int.Parse(cantidad));
                            if (existencia_actual.Length < 2)
                            {
                                existencia_actual = "0" + existencia_actual;
                            }
                            escritor.WriteLine(existencia_actual);
                            if (int.Parse(existencia_actual) == 0)
                            {
                                Caja_arbol.Instance.arbolFarm.Eliminar(farmaco, mFarmaco.ComparName);
                            }
                        }
                    }
                }
                if (CarritoCompra.Instance.total < 0)
                {
                    CarritoCompra.Instance.total = 0;
                }
                ViewBag.TotalCompra = CarritoCompra.Instance.total;

                return View("BuscarFarmaco", CarritoCompra.Instance.carrito);
            }
            
            catch
            {
                if (CarritoCompra.Instance.total < 0)
                {
                    CarritoCompra.Instance.total = 0;
                }
                ViewBag.TotalCompra = CarritoCompra.Instance.total;

                return View("BuscarFarmaco", CarritoCompra.Instance.carrito);
            }
         }


        public ActionResult Delete(string id)
        {
            try
            {
                mCompraFarmaco farmaco = new mCompraFarmaco();
                foreach (var item in CarritoCompra.Instance.carrito)
                {
                    if (item.Nombre == id)
                    {
                        farmaco = item;
                    }
                }
                mFarmaco drug = new mFarmaco();
                drug.Nombre = id;
                drug = Caja_arbol.Instance.arbolFarm.Buscar(drug, mFarmaco.ComparName);
                int linea_buscad = 0;
                string[] infor_separada = new string[7];
                if (drug != null)
                {
                    linea_buscad = drug.Linea;
                    string info_farmaco = "";
                    using (FileStream archivo = new FileStream(Caja_arbol.Instance.direccion_archivo_arbol, FileMode.Open))
                    {
                        using (StreamReader lector = new StreamReader(archivo))
                        {
                            archivo.Seek(linea_buscad, SeekOrigin.Begin);
                            info_farmaco = lector.ReadLine();
                            linea_buscad += info_farmaco.Length;
                        }
                    }
                    Regex regx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    infor_separada = regx.Split(info_farmaco);
                }
                else
                {
                    using (FileStream archivo = new FileStream(Caja_arbol.Instance.direccion_archivo_arbol, FileMode.Open))
                    {
                        using (var archivolec = new StreamReader(archivo))
                        {
                            bool Encontrado = false;
                            string lector = archivolec.ReadLine();
                            int posicion = lector.Length + 2;
                            
                            lector = archivolec.ReadLine();
                            while (lector != null && !Encontrado)
                            {
                                string[] cajatext = lector.Split(Convert.ToChar(","));
                                mFarmaco nuevo = new mFarmaco();
                                nuevo.Nombre = cajatext[1];
                                nuevo.Linea = posicion;
                                posicion += lector.Length + 2;
                                if (nuevo.Nombre == id)
                                {
                                    Caja_arbol.Instance.arbolFarm.Agregar(nuevo, mFarmaco.ComparName);
                                    Encontrado = true;
                                    linea_buscad = nuevo.Linea + lector.Length;
                                    infor_separada[infor_separada.Length - 1] = "00";
                                }
                                lector = archivolec.ReadLine();
                            }
                        }
                    }

                }


                    //Se procede a la agregacion del producto regresado a la exitencia
                    using (var archivo = new FileStream(Caja_arbol.Instance.direccion_archivo_arbol, FileMode.Open))
                    {
                    using (var escritor = new StreamWriter(archivo))
                    {
                        int posicion_existencia = linea_buscad - infor_separada[infor_separada.Length - 1].Length;
                        archivo.Seek(posicion_existencia, SeekOrigin.Begin);
                        string existencia_actual = Convert.ToString(int.Parse(infor_separada[infor_separada.Length - 1]) + farmaco.Cantidad);
                        if (existencia_actual.Length < 2)
                        {
                            existencia_actual = "0" + existencia_actual;
                        }
                        escritor.WriteLine(existencia_actual);
                    }
                }

                CarritoCompra.Instance.carrito.Remove(farmaco);
                CarritoCompra.Instance.total = CarritoCompra.Instance.total - farmaco.Subtotal;
                if (CarritoCompra.Instance.total < 0)
                {
                    CarritoCompra.Instance.total = 0;
                }

                ViewBag.TotalCompra = CarritoCompra.Instance.total;
                return View("BuscarFarmaco", CarritoCompra.Instance.carrito);
            }
            catch
            {
                return View("BuscarFarmaco", CarritoCompra.Instance.carrito);
            }
        }
    }
}
