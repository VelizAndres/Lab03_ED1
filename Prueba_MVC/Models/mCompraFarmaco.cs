using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prueba_MVC.Models
{
    public class mCompraFarmaco
    {
        private string nombre;
        private int cantidad;
        private double precio_uni;
        private double subtotal;

        public string Nombre { get => nombre; set => nombre = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public double Precio_uni { get => precio_uni; set => precio_uni = value; }
        public double Subtotal { get => subtotal; set => subtotal = value; }
    }
}