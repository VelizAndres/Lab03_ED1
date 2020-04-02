using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prueba_MVC.Herramientas.Estructura;

namespace Prueba_MVC.Herramientas.Interfaz
{
    interface IArbol <T>
    {
        void Agregar(T valor, Delegate comparar);
        void Eliminar(T valor, Delegate Comparar);
        T Buscar(T valor, Delegate comparar);
    }
}
