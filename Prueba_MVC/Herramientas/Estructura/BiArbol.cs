using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prueba_MVC.Herramientas.Interfaz;

namespace Prueba_MVC.Herramientas.Estructura
{
    public class BiArbol<T> : IArbol<T>
    {
        public Nodo<T> raiz;


        public void Agregar(T valor, Delegate comparar)
        {
            Nodo<T> Nuevo = new Nodo<T>();
            Nuevo.Valor = valor;
            if (raiz == null)
            {
                raiz = Nuevo;
            }
            else
            {
                if ((int)comparar.DynamicInvoke (raiz.Valor,Nuevo.Valor)<0 )
                {
                    if (raiz.Hijoder == null)
                    {
                        raiz.Hijoder = Nuevo;
                        raiz.Hijoder.Padre = raiz;
                    }
                    else
                    {
                        Recorrer_Asig(raiz.Hijoder, Nuevo, comparar);   
                    }
                }
                //hijo izquierdo
                else
                {
                    if (raiz.Hijoizq == null)
                    {
                        raiz.Hijoizq = Nuevo;
                        raiz.Hijoizq.Padre = raiz;
                    }
                    else
                    {
                        Recorrer_Asig(raiz.Hijoizq, Nuevo, comparar);
                    }
                }
                raiz.Altura = Det_Altura(raiz.Hijoder, raiz.Hijoizq);
                Rotaciones(raiz, comparar);
            }
        }

        public void Recorrer_Asig(Nodo<T> dad, Nodo<T> nuevo, Delegate Comparar)
        {
            if ((int)Comparar.DynamicInvoke(dad.Valor,nuevo.Valor)<0)
            {
                if(dad.Hijoder==null)
                {
                    dad.Hijoder = nuevo;
                    dad.Hijoder.Padre = dad;
                }
                else
                {
                    //se debe buscar un valor vacio(recursividad)
                    Recorrer_Asig(dad.Hijoder, nuevo, Comparar);
                }
            }
            else
            {
                if(dad.Hijoizq==null)
                {
                    dad.Hijoizq = nuevo;
                    dad.Hijoizq.Padre = dad;
                }
                else
                {
                    //se debe buscar un valor vacio(recursividad)
                    Recorrer_Asig(dad.Hijoizq, nuevo, Comparar);
                }
            }
          dad.Altura= Det_Altura(dad.Hijoder, dad.Hijoizq);
          Rotaciones(dad, Comparar);
        }


        /************Metodos de busqueda****************/
        public T Buscar(T valor, Delegate comparar)
        {
            Nodo<T> vacio = new Nodo<T>();
            Nodo<T> nodoBuscado = new Nodo<T>();
            nodoBuscado.Valor = valor;
            if (raiz == null)
            {
                return vacio.Valor;
            }
            else
            {
                int resulcompar = (int)comparar.DynamicInvoke(raiz.Valor, nodoBuscado.Valor);
                if (resulcompar == 0)
                {
                    return raiz.Valor;
                }
                //hijo derecho
                if (resulcompar < 0)
                {
                    return Recorrer_Busqueda(raiz.Hijoder, nodoBuscado, comparar).Valor;

                }
                //hijo izquierdo
                else
                {
                    return Recorrer_Busqueda(raiz.Hijoizq, nodoBuscado, comparar).Valor;
                }
            }

        }

        public Nodo<T> Recorrer_Busqueda(Nodo<T> dad, Nodo<T> buscado, Delegate Comparar)
        {
            Nodo<T> vacio = new Nodo<T>();
            vacio.Valor = default(T);
            int resulcompar = (int)Comparar.DynamicInvoke(dad.Valor, buscado.Valor);
            if (resulcompar == 0)
            {
                return dad;
            }
            if (resulcompar < 0)
            {
                //se debe buscar un valor vacio(recursividad)
                if(dad.Hijoder != null)
                {
                    return Recorrer_Busqueda(dad.Hijoder, buscado, Comparar);
                }
                return vacio;
            }
            else
            {
                //se debe buscar un valor vacio(recursividad)
                if(dad.Hijoizq!=null)
                {
                    return Recorrer_Busqueda(dad.Hijoizq, buscado, Comparar);
                }
                return vacio;
            }
        }
        /************Finaliza metodo de busqueda****************/



        /************Metodos de eliminacion****************/

        public void Eliminar(T valor, Delegate Comparar)
        {
            Nodo<T> nododel = new Nodo<T>();
            nododel.Valor = valor;
            int comparcion_valor = (int)Comparar.DynamicInvoke(raiz.Valor, nododel.Valor);
            if (comparcion_valor == 0)
            {
                int hijos = 0;
                if (raiz.Hijoder != null) { hijos++; }
                if (raiz.Hijoizq != null) { hijos++; }
                Metodo_Eliminacion_Raiz(hijos, raiz, Comparar);
            }
            else
            {
                //Hijo derecho
                if (comparcion_valor < 0)
                {
                    Buscar_Nodo_Eliminar(raiz.Hijoder, nododel, Comparar);
                }
                //hijo izquierdo
                else
                {
                    Buscar_Nodo_Eliminar(raiz.Hijoizq, nododel, Comparar);
                }
            }

            raiz.Altura = Det_Altura(raiz.Hijoder, raiz.Hijoizq);
            Rotaciones(raiz, Comparar);

        }


        public void Buscar_Nodo_Eliminar(Nodo<T> padre, Nodo<T> del_nodo, Delegate Comparar)
        {
            int resulcompar = (int)Comparar.DynamicInvoke(padre.Valor, del_nodo.Valor);
            if (resulcompar == 0)
            {
                int hijos = 0;
                if (padre.Hijoder != null) { hijos++; }
                if (padre.Hijoizq != null) { hijos++; }
                Metodo_Eliminacion(hijos, padre, Comparar);
            }
            else
            {
                if (resulcompar < 0)
                {
                    Buscar_Nodo_Eliminar(padre.Hijoder, del_nodo, Comparar);
                }
                //hijo izquierdo
                else
                {
                    Buscar_Nodo_Eliminar(padre.Hijoizq, del_nodo, Comparar);
                }
            }
            padre.Altura = Det_Altura(padre.Hijoder, padre.Hijoizq);
            Rotaciones(padre, Comparar);
        }

        public void Metodo_Eliminacion(int tipo, Nodo<T> Nodo_Borrar,Delegate Comparar)
        {
            int valor_com = (int)Comparar.DynamicInvoke(Nodo_Borrar.Padre.Valor, Nodo_Borrar.Valor);
            if (tipo==0)
            {
                if (valor_com < 0)
                {
                    Nodo_Borrar.Padre.Hijoder = null;
                }
                else
                {
                    Nodo_Borrar.Padre.Hijoizq = null;
                }
                    
            }
            if(tipo==1)
            {
                if(Nodo_Borrar.Hijoder!=null)
                {
                    if (valor_com < 0)
                    {
                        Nodo_Borrar.Padre.Hijoder = Nodo_Borrar.Hijoder; 
                    }
                    else
                    {
                        Nodo_Borrar.Padre.Hijoizq = Nodo_Borrar.Hijoder; 
                    }
                    Nodo_Borrar.Hijoder.Padre = Nodo_Borrar.Padre;
                }
                else
                {
                    if (valor_com < 0)
                    {
                        Nodo_Borrar.Padre.Hijoder = Nodo_Borrar.Hijoizq; 
                    }
                    else
                    {
                        Nodo_Borrar.Padre.Hijoizq = Nodo_Borrar.Hijoizq; 
                    }
                    Nodo_Borrar.Hijoizq.Padre = Nodo_Borrar.Padre;
                }
            }
            if(tipo==2)
            {
                Nodo<T> Mayor_Izq = Obtener_MayorIzq(Nodo_Borrar.Hijoizq);
                Buscar_Nodo_Eliminar(Mayor_Izq.Padre, Mayor_Izq, Comparar);
                    if (valor_com < 0)
                    {
                        Nodo_Borrar.Padre.Hijoder = Mayor_Izq; 
                    }
                    else
                    {
                    Nodo_Borrar.Padre.Hijoizq = Mayor_Izq;
                    }
                    Mayor_Izq.Padre = Nodo_Borrar.Padre;
                    Mayor_Izq.Hijoder = Nodo_Borrar.Hijoder;
                    Mayor_Izq.Hijoizq = Nodo_Borrar.Hijoizq;
                    Mayor_Izq.Hijoder.Padre = Mayor_Izq;
                    Mayor_Izq.Hijoizq.Padre = Mayor_Izq;
                    Mayor_Izq.Altura = Det_Altura(Mayor_Izq.Hijoder, Mayor_Izq.Hijoizq);
            }
        }

        public void Metodo_Eliminacion_Raiz(int tipo, Nodo<T> Nodo_Borrar, Delegate Comparar)
        {
            if (tipo == 0)
            {
                raiz = null;
            }
            if (tipo == 1)
            {
                if (Nodo_Borrar.Hijoder != null)
                {
                    raiz = Nodo_Borrar.Hijoder;
                }
                else
                {
                    raiz = Nodo_Borrar.Hijoizq;
                }
            }
            if (tipo == 2)
            {
                Nodo<T> Mayor_Izq = Obtener_MayorIzq(Nodo_Borrar.Hijoizq);
                Buscar_Nodo_Eliminar(Mayor_Izq.Padre, Mayor_Izq, Comparar);
                raiz = Mayor_Izq;
                Mayor_Izq.Padre = null;
                Mayor_Izq.Hijoder = Nodo_Borrar.Hijoder;
                Mayor_Izq.Hijoizq = Nodo_Borrar.Hijoizq;
                Mayor_Izq.Hijoder.Padre = Mayor_Izq;
                Mayor_Izq.Hijoizq.Padre = Mayor_Izq;
                Mayor_Izq.Altura = Det_Altura(Mayor_Izq.Hijoder, Mayor_Izq.Hijoizq);
            }
        }

        public Nodo<T> Obtener_MayorIzq(Nodo<T> guia)
        {
            if(guia.Hijoder == null)
            {
              return guia;
            }
            return Obtener_MayorIzq(guia.Hijoder);
        }
        /************Finaliza metodos de eliminacion****************/

        /************Recorridos****************/
        public string texto_impresion = "";

        public string ExportarInorder(Delegate ob_nom)
        {
            texto_impresion ="";
            try
            {
                if(raiz.Valor!=null)
                {
                    Inorder(raiz, ob_nom);   
                }
                else
                {
                    return "Arbol Vacio";
                }
                return texto_impresion;
            }
            catch {
            return "Arbol Vacio";
            }
        }
        private void Inorder(Nodo<T> nodo_obt, Delegate ob_nom)
        {
            if(nodo_obt.Hijoizq!=null)
            {
                Inorder(nodo_obt.Hijoizq,ob_nom);
            }
            texto_impresion += (string)ob_nom.DynamicInvoke(nodo_obt.Valor) + Environment.NewLine;
            if(nodo_obt.Hijoder!=null)
            {
                Inorder(nodo_obt.Hijoder, ob_nom);
            }

        }
        ///
        public string ExportarPreorder(Delegate ob_nom)
        {
            texto_impresion = "";
            try
            {
                if (raiz.Valor != null)
                {
                    Preorder(raiz, ob_nom);
                }
                else
                {
                    return "Arbol Vacio";
                }
                return texto_impresion;
            }
            catch
            {
                return "Arbol Vacio";
            }
        }
        private void Preorder(Nodo<T> nodo_obt, Delegate ob_nom)
        {
            texto_impresion += (string)ob_nom.DynamicInvoke(nodo_obt.Valor) + Environment.NewLine;
            if (nodo_obt.Hijoizq != null)
            {
                Preorder(nodo_obt.Hijoizq, ob_nom);
            }
            if (nodo_obt.Hijoder != null)
            {
                Preorder(nodo_obt.Hijoder, ob_nom);
            }
        }
        ///
        public string ExportarPostorder(Delegate ob_nom)
        {
            texto_impresion = "";
            try
            {
                if (raiz.Valor != null)
                {
                    Postorder(raiz, ob_nom);
                }
                else
                {
                    return "Arbol Vacio";
                }
                return texto_impresion;
            }
            catch
            {
                return "Arbol Vacio";
            }
        }
        private void Postorder(Nodo<T> nodo_obt, Delegate ob_nom)
        {
            if (nodo_obt.Hijoizq != null)
            {
                Postorder(nodo_obt.Hijoizq, ob_nom);
            }
            if (nodo_obt.Hijoder != null)
            {
                Postorder(nodo_obt.Hijoder, ob_nom);
            }
            texto_impresion += (string)ob_nom.DynamicInvoke(nodo_obt.Valor) + Environment.NewLine;
        }
        /************Finaliza recorridos****************/



        /************Rotaciones****************/
        public int Det_Altura(Nodo<T> NHder,Nodo<T> NHizq)
        {
            if(NHder==null && NHizq==null)
            {
                return 0;
            }
            if(NHder!=null && NHizq==null)
            {
                return NHder.Altura + 1;
            }
            if (NHizq != null && NHder == null)
            {
                return NHizq.Altura + 1;
            }
            else
            {
                if (NHder.Altura >= NHizq.Altura)
                {
                    return NHder.Altura + 1;
                }
                else
                {
                    return NHizq.Altura + 1;
                }
            }
        }

        public int Det_FactEqui(Nodo<T> NHder, Nodo<T> NHizq)
        {
            if (NHder == null && NHizq == null)
            {
                return 0;
            }
            if (NHder != null && NHizq == null)
            {
                return NHder.Altura+1;
            }
            if (NHizq != null && NHder == null)
            {
                return -NHizq.Altura-1;
            }
            else
            {
                return NHder.Altura-NHizq.Altura;
            }
        }

        public void Rotaciones(Nodo<T> N_dad, Delegate Comparador)
        {
            int Fact_Equi = Det_FactEqui(N_dad.Hijoder, N_dad.Hijoizq);
            if(Fact_Equi==2)
            {
                if (Det_FactEqui(N_dad.Hijoder.Hijoder, N_dad.Hijoder.Hijoizq)==-1)
                {
                    //rotacion doble izquierda
                    Rot_Simple_Derecha(N_dad.Hijoder, Comparador);

                    if (N_dad == raiz)
                    {
                        Rot_Simple_Izquierda_Raiz(N_dad, Comparador);
                    }
                    else
                    {
                        Rot_Simple_Izquierda(N_dad, Comparador);
                    }
                }
                else
                {
                    //rotacion simple izquierda
                    if(N_dad==raiz)
                    {
                        Rot_Simple_Izquierda_Raiz(N_dad, Comparador);
                    }
                    else
                    {
                        Rot_Simple_Izquierda(N_dad, Comparador);
                    }
                }
            }
            if(Fact_Equi==-2)
            {
                if (Det_FactEqui(N_dad.Hijoizq.Hijoder, N_dad.Hijoizq.Hijoizq) == 1)
                {
                    //rotacion doble derecha
                    Rot_Simple_Izquierda(N_dad.Hijoizq, Comparador);

                    if (N_dad == raiz)
                    {
                        Rot_Simple_Derecha_Raiz(N_dad, Comparador);
                    }
                    else
                    {
                        Rot_Simple_Derecha(N_dad, Comparador);
                    }
                }
                else
                {
                    //rotacion simple derecha
                    if (N_dad == raiz)
                    {
                        Rot_Simple_Derecha_Raiz(N_dad, Comparador);
                    }
                    else
                    {
                        Rot_Simple_Derecha(N_dad, Comparador);
                    }
                }
            }
            else
            {
                //No pasa nada oiga
            }
        }

        public void Rot_Simple_Izquierda_Raiz(Nodo<T> Raiz_Rot, Delegate Comparador)
        {
            Nodo<T> N_Aux = Raiz_Rot;
            raiz = Raiz_Rot.Hijoder;
            N_Aux.Padre = N_Aux.Hijoder;
            //En caso de que el hijo derecho tenga un hijo izquierdo
            if (N_Aux.Hijoder.Hijoizq != null)
            {
                N_Aux.Hijoder.Hijoizq.Padre = N_Aux;
                N_Aux.Hijoder = N_Aux.Hijoder.Hijoizq;
                raiz.Hijoizq = N_Aux;
            }
            else
            {
                N_Aux.Hijoder.Hijoizq = N_Aux;
                N_Aux.Hijoder = null;
            }
            raiz.Padre = null;
            //Colocar las nuevas alturas
            N_Aux.Altura = Det_Altura(Raiz_Rot.Hijoder, Raiz_Rot.Hijoizq);
            raiz.Altura = Det_Altura(raiz.Hijoder, raiz.Hijoizq);
        }

        public void Rot_Simple_Izquierda(Nodo<T> N_Rot, Delegate Comparador)
        {
            Nodo<T> HijoCambio = N_Rot.Hijoder.Hijoizq;
            if ((int)Comparador.DynamicInvoke(N_Rot.Valor, N_Rot.Padre.Valor) < 0)
            {
                N_Rot.Padre.Hijoizq = N_Rot.Hijoder;
            }
            else
            {
                N_Rot.Padre.Hijoder = N_Rot.Hijoder;
            }
            N_Rot.Hijoder.Padre = N_Rot.Padre;
            N_Rot.Padre = N_Rot.Hijoder;
            N_Rot.Padre.Hijoizq = N_Rot;
            //En caso de que el hijo derecho tenga un hijo izquierdo
            if (HijoCambio != null)
            {
                HijoCambio.Padre = N_Rot;
                N_Rot.Hijoder = HijoCambio;
            }
            else
            {
                N_Rot.Hijoder = null;
            }
            //Colocar la nueva altura
            N_Rot.Altura = Det_Altura(N_Rot.Hijoder, N_Rot.Hijoizq);
            N_Rot.Padre.Altura= Det_Altura(N_Rot.Padre.Hijoder, N_Rot.Padre.Hijoizq);
        }

        public void Rot_Simple_Derecha_Raiz(Nodo<T> Raiz_Rot, Delegate Comparador)
        {
            Nodo<T> N_Aux = Raiz_Rot;
            raiz = Raiz_Rot.Hijoizq;
            N_Aux.Padre = N_Aux.Hijoizq;
            //En caso de que el hijo izquierdo tenga un hijo derecho
            if (N_Aux.Hijoizq.Hijoder != null)
            {
                N_Aux.Hijoizq.Hijoder.Padre = N_Aux;
                N_Aux.Hijoizq = N_Aux.Hijoizq.Hijoder;
                raiz.Hijoder = N_Aux;
            }
            else
            {
                N_Aux.Hijoizq.Hijoder = N_Aux;
                N_Aux.Hijoizq = null;
            }
            raiz.Padre = null;
            //Colocar las nuevas alturas
            N_Aux.Altura = Det_Altura(Raiz_Rot.Hijoder, Raiz_Rot.Hijoizq);
            raiz.Altura = Det_Altura(raiz.Hijoder, raiz.Hijoizq);
        }

        public void Rot_Simple_Derecha(Nodo<T> N_Rot, Delegate Comparador)
        {
            Nodo<T> HijoCambio = N_Rot.Hijoizq.Hijoder;
            if ((int)Comparador.DynamicInvoke(N_Rot.Valor, N_Rot.Padre.Valor) < 0)
            {
                N_Rot.Padre.Hijoizq = N_Rot.Hijoizq;
            }
            else
            {
                N_Rot.Padre.Hijoder = N_Rot.Hijoizq;
            }
            N_Rot.Hijoizq.Padre = N_Rot.Padre;
            N_Rot.Padre = N_Rot.Hijoizq;
            N_Rot.Padre.Hijoder = N_Rot;
            //En caso de que el hijo izquierdo tenga un hijo derecho
            if (HijoCambio != null)
            {
                HijoCambio.Padre = N_Rot;
                N_Rot.Hijoizq = HijoCambio;
            }
            else
            {
                N_Rot.Hijoizq = null;
            }
            //Colocar la nueva altura
            N_Rot.Altura = Det_Altura(N_Rot.Hijoder, N_Rot.Hijoizq);
            N_Rot.Padre.Altura= Det_Altura(N_Rot.Padre.Hijoder, N_Rot.Padre.Hijoizq);
        }
        /************Finaliza rotaciones****************/

    }
}