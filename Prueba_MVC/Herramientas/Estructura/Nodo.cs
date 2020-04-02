using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prueba_MVC.Models;


namespace Prueba_MVC.Herramientas.Estructura
{
    public class Nodo<T> 
    {
        private Nodo<T> padre;
        private Nodo<T> hijoizq;
        private Nodo<T> hijoder;
        private T valor;
        private int altura;

        public Nodo<T> Padre { get => padre; set => padre = value; }
        public Nodo<T> Hijoizq { get => hijoizq; set => hijoizq = value; }
        public Nodo<T> Hijoder { get => hijoder; set => hijoder = value; }
        public T Valor { get => valor; set => valor = value; }
        public int Altura { get => altura; set => altura = value; }
    }
}
