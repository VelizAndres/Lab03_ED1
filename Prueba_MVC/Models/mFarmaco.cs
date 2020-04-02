using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prueba_MVC.Herramientas.Estructura;
using Prueba_MVC.Herramientas.Almacen;

namespace Prueba_MVC.Models
{
    public class mFarmaco
    {
        private string nombre;
        private int linea;

        public string Nombre { get => nombre; set => nombre = value; }
        public int Linea { get => linea; set => linea = value; }



        public static Comparison<mFarmaco> ComparName = delegate (mFarmaco medic1, mFarmaco medic2)
        {
//            Nodo<mFarmaco> nodo=   Caja_arbol.Instance.arbolFarm.raiz;
            return medic1.nombre.CompareTo(medic2.nombre);
        };

        public static Func<mFarmaco, string> ObtenerNombre = delegate (mFarmaco farm)
          {
              return farm.Nombre;
          };
         

        

    }
}