using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prueba_MVC.Models;
namespace Prueba_MVC.Herramientas.Almacen
{
    public class ListaPedidos
    {
        private static ListaPedidos _instance = null;

        public static ListaPedidos Instance
        {
            get
            {
                if (_instance == null) _instance = new ListaPedidos();
                return _instance;
            }
        }

        public LinkedList<mPedido> arbolFarm = new LinkedList<mPedido>();
    }
}