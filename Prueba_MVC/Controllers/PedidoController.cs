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
            return View(comprador.Pedidos);
        }
        public ActionResult BuscarFarmaco()
        {
          
            return View(CarritoCompra.Instance.carrito);
        }



        public ActionResult Busqueda(string Texto)
        {
            try { mFarmaco farmaco = new mFarmaco();
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
                return View("BuscarFarmaco", CarritoCompra.Instance.carrito); 
            }
       }

        // GET: Pedido/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pedido/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here




                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pedido/Edit/5
        public ActionResult Agregar(string cantidad)
        {
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
                }
            }
            Regex regx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            string[] infor_separada = regx.Split(info_farmaco);

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
            FarmacoNuevo.Subtotal = FarmacoNuevo.Precio_uni* int.Parse(cantidad);
            CarritoCompra.Instance.carrito.AddFirst(FarmacoNuevo);
            CarritoCompra.Instance.total += FarmacoNuevo.Subtotal;
            if (CarritoCompra.Instance.total < 0)
            {
                CarritoCompra.Instance.total = 0;
            }
            ViewBag.TotalCompra = CarritoCompra.Instance.total;
            return View("BuscarFarmaco", CarritoCompra.Instance.carrito);
        }

        // POST: Pedido/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Pedido/Delete/5
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
