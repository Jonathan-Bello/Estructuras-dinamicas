// =====================================================================
//  ESTRUCTURAS DE DATOS DINÁMICAS: LISTA ENLAZADA SIMPLE
// ---------------------------------------------------------------------
//  Una lista enlazada es una colección de NODOS que se crean en memoria
//  conforme se necesitan (memoria dinámica). Cada nodo guarda:
//     1) Un dato.
//     2) Una referencia (enlace) al siguiente nodo.
//
//     Cabeza
//       |
//       v
//    [ 10 | o-]--> [ 25 | o-]--> [ 7 | null ]
//
//  A diferencia de un arreglo:
//   - No tiene tamaño fijo: crece y se reduce en tiempo de ejecución.
//   - Insertar/eliminar al inicio es muy rápido (no hay que recorrer).
//   - Para llegar al elemento "n" hay que recorrer desde la cabeza.
// =====================================================================

using System;

namespace ListasEnlazadas
{
    // -----------------------------------------------------------------
    //  CLASE NODO: la "pieza" básica de la lista
    // -----------------------------------------------------------------
    class Nodo
    {
        public int Dato;          // Información que guarda el nodo
        public Nodo? Siguiente;   // Enlace al siguiente nodo (null si es el último)

        public Nodo(int dato)
        {
            Dato = dato;
            Siguiente = null;     // Al crearse, todavía no apunta a nadie
        }
    }

    // -----------------------------------------------------------------
    //  CLASE LISTA ENLAZADA: administra los nodos
    // -----------------------------------------------------------------
    class ListaEnlazada
    {
        private Nodo? cabeza;     // Primer nodo de la lista
        private int cantidad;     // Número de nodos en la lista

        public ListaEnlazada()
        {
            cabeza = null;        // Lista vacía
            cantidad = 0;
        }

        public bool EstaVacia() => cabeza == null;

        public int Contar() => cantidad;

        // ---------------------------------------------------------------
        //  INSERTAR AL INICIO  -> O(1)
        //  1. Se crea el nodo nuevo.
        //  2. El nuevo apunta a la cabeza actual.
        //  3. La cabeza ahora es el nodo nuevo.
        // ---------------------------------------------------------------
        public void InsertarAlInicio(int dato)
        {
            Nodo nuevo = new Nodo(dato);
            nuevo.Siguiente = cabeza;
            cabeza = nuevo;
            cantidad++;
        }

        // ---------------------------------------------------------------
        //  INSERTAR AL FINAL  -> O(n)
        //  Hay que recorrer hasta el último nodo (el que apunta a null).
        // ---------------------------------------------------------------
        public void InsertarAlFinal(int dato)
        {
            Nodo nuevo = new Nodo(dato);

            if (cabeza == null)
            {
                cabeza = nuevo;
            }
            else
            {
                Nodo actual = cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;   // Avanzamos al siguiente nodo
                }
                actual.Siguiente = nuevo;        // El último ahora apunta al nuevo
            }
            cantidad++;
        }

        // ---------------------------------------------------------------
        //  INSERTAR EN UNA POSICIÓN (empezando en 0)
        //  Nos detenemos en el nodo ANTERIOR a la posición deseada.
        // ---------------------------------------------------------------
        public bool InsertarEnPosicion(int dato, int posicion)
        {
            if (posicion < 0 || posicion > cantidad)
                return false;

            if (posicion == 0)
            {
                InsertarAlInicio(dato);
                return true;
            }

            Nodo anterior = cabeza!;
            for (int i = 0; i < posicion - 1; i++)
            {
                anterior = anterior.Siguiente!;
            }

            Nodo nuevo = new Nodo(dato);
            nuevo.Siguiente = anterior.Siguiente;  // El nuevo apunta al que seguía
            anterior.Siguiente = nuevo;            // El anterior apunta al nuevo
            cantidad++;
            return true;
        }

        // ---------------------------------------------------------------
        //  ELIMINAR UN VALOR (la primera vez que aparece)
        //  Para "quitar" un nodo basta con que su anterior lo "salte".
        //  El recolector de basura (Garbage Collector) de .NET liberará
        //  la memoria del nodo que ya nadie referencia.
        // ---------------------------------------------------------------
        public bool Eliminar(int dato)
        {
            if (cabeza == null)
                return false;

            // Caso especial: el dato está en la cabeza
            if (cabeza.Dato == dato)
            {
                cabeza = cabeza.Siguiente;
                cantidad--;
                return true;
            }

            Nodo actual = cabeza;
            while (actual.Siguiente != null)
            {
                if (actual.Siguiente.Dato == dato)
                {
                    actual.Siguiente = actual.Siguiente.Siguiente; // "Saltamos" el nodo
                    cantidad--;
                    return true;
                }
                actual = actual.Siguiente;
            }
            return false; // No se encontró
        }

        // ---------------------------------------------------------------
        //  BUSCAR: devuelve la posición del dato o -1 si no existe
        // ---------------------------------------------------------------
        public int Buscar(int dato)
        {
            Nodo? actual = cabeza;
            int posicion = 0;

            while (actual != null)
            {
                if (actual.Dato == dato)
                    return posicion;

                actual = actual.Siguiente;
                posicion++;
            }
            return -1;
        }

        // ---------------------------------------------------------------
        //  INVERTIR LA LISTA (ejercicio clásico)
        //  Se cambia la dirección de cada enlace usando tres referencias.
        // ---------------------------------------------------------------
        public void Invertir()
        {
            Nodo? anterior = null;
            Nodo? actual = cabeza;

            while (actual != null)
            {
                Nodo? siguiente = actual.Siguiente; // 1. Guardamos el siguiente
                actual.Siguiente = anterior;        // 2. Invertimos el enlace
                anterior = actual;                  // 3. Avanzamos "anterior"
                actual = siguiente;                 // 4. Avanzamos "actual"
            }
            cabeza = anterior;
        }

        // ---------------------------------------------------------------
        //  VACIAR LA LISTA
        // ---------------------------------------------------------------
        public void Vaciar()
        {
            cabeza = null;   // Sin referencias, el GC libera todos los nodos
            cantidad = 0;
        }

        // ---------------------------------------------------------------
        //  MOSTRAR: recorre la lista de principio a fin
        // ---------------------------------------------------------------
        public void Mostrar()
        {
            if (cabeza == null)
            {
                Console.WriteLine("  La lista está vacía.");
                return;
            }

            Console.Write("  Cabeza -> ");
            Nodo? actual = cabeza;
            while (actual != null)
            {
                Console.Write($"[{actual.Dato}] -> ");
                actual = actual.Siguiente;
            }
            Console.WriteLine("null");
            Console.WriteLine($"  Total de nodos: {cantidad}");
        }
    }

    // -----------------------------------------------------------------
    //  PROGRAMA PRINCIPAL: menú interactivo
    // -----------------------------------------------------------------
    class Program
    {
        static void Main(string[] args)
        {
            ListaEnlazada lista = new ListaEnlazada();
            int opcion;

            do
            {
                Console.WriteLine();
                Console.WriteLine("========= LISTA ENLAZADA SIMPLE =========");
                lista.Mostrar();
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine(" 1. Insertar al inicio");
                Console.WriteLine(" 2. Insertar al final");
                Console.WriteLine(" 3. Insertar en una posición");
                Console.WriteLine(" 4. Eliminar un valor");
                Console.WriteLine(" 5. Buscar un valor");
                Console.WriteLine(" 6. Invertir la lista");
                Console.WriteLine(" 7. Vaciar la lista");
                Console.WriteLine(" 8. Cargar datos de ejemplo");
                Console.WriteLine(" 0. Salir");
                Console.WriteLine("=========================================");

                opcion = LeerEntero("Elige una opción: ");
                Console.WriteLine();

                switch (opcion)
                {
                    case 1:
                        lista.InsertarAlInicio(LeerEntero("Valor a insertar: "));
                        Console.WriteLine("  Nodo insertado al inicio.");
                        break;

                    case 2:
                        lista.InsertarAlFinal(LeerEntero("Valor a insertar: "));
                        Console.WriteLine("  Nodo insertado al final.");
                        break;

                    case 3:
                        int valor = LeerEntero("Valor a insertar: ");
                        int pos = LeerEntero($"Posición (0 a {lista.Contar()}): ");
                        Console.WriteLine(lista.InsertarEnPosicion(valor, pos)
                            ? "  Nodo insertado correctamente."
                            : "  Posición no válida.");
                        break;

                    case 4:
                        Console.WriteLine(lista.Eliminar(LeerEntero("Valor a eliminar: "))
                            ? "  Nodo eliminado."
                            : "  El valor no existe en la lista.");
                        break;

                    case 5:
                        int buscado = LeerEntero("Valor a buscar: ");
                        int posicion = lista.Buscar(buscado);
                        Console.WriteLine(posicion >= 0
                            ? $"  El valor {buscado} está en la posición {posicion}."
                            : $"  El valor {buscado} no se encontró.");
                        break;

                    case 6:
                        lista.Invertir();
                        Console.WriteLine("  Lista invertida.");
                        break;

                    case 7:
                        lista.Vaciar();
                        Console.WriteLine("  Lista vaciada.");
                        break;

                    case 8:
                        foreach (int n in new[] { 10, 25, 7, 42, 3 })
                            lista.InsertarAlFinal(n);
                        Console.WriteLine("  Se agregaron: 10, 25, 7, 42, 3");
                        break;

                    case 0:
                        Console.WriteLine("  ¡Hasta luego!");
                        break;

                    default:
                        Console.WriteLine("  Opción no válida.");
                        break;
                }

            } while (opcion != 0);
        }

        // Lee un número entero validando la entrada del usuario
        static int LeerEntero(string mensaje)
        {
            int numero;
            Console.Write(mensaje);
            while (!int.TryParse(Console.ReadLine(), out numero))
            {
                Console.Write("  Entrada inválida. Escribe un número entero: ");
            }
            return numero;
        }
    }
}