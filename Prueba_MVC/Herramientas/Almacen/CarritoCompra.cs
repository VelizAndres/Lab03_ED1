using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prueba_MVC.Models;
namespace Prueba_MVC.Herramientas.Almacen
{
    public class CarritoCompra
    {
            private static CarritoCompra _instance = null;
            public static CarritoCompra Instance
            {
                get
                {
                    if (_instance == null) _instance = new CarritoCompra();
                    return _instance;
                }
            }
            public LinkedList<mCompraFarmaco> carrito = new LinkedList<mCompraFarmaco>();
            public string quizascompra;
            public double total;
    }
}