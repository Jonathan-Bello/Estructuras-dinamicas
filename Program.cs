// =====================================================================
//   ESTRUCTURAS DE DATOS DINÁMICAS: LISTA ENLAZADA SIMPLE
//   Ejemplo aplicado: Lista de Supermercado
// =====================================================================

using System;

namespace ListaSupermercado
{
    // -----------------------------------------------------------------
    //   NODO: representa un producto de la lista
    // -----------------------------------------------------------------
    class Nodo
    {
        public string Dato;        // Nombre del producto
        public Nodo? Siguiente;    // Siguiente producto (null si es el último)

        public Nodo(string dato, Nodo? siguiente = null)
        {
            Dato = dato;
            Siguiente = siguiente;
        }
    }

    // -----------------------------------------------------------------
    //   LISTA ENLAZADA: administra los productos
    // -----------------------------------------------------------------
    class ListaEnlazada
    {
        private Nodo? cabeza;      // Primer producto de la lista
        private int cantidad;      // Número total de productos

        public bool EstaVacia() => cabeza == null;

        public int Contar() => cantidad;

        // Compara nombres de productos sin distinguir mayúsculas/minúsculas
        private static bool MismoProducto(string a, string b) =>
            a.Equals(b, StringComparison.OrdinalIgnoreCase);

        // ---------------------------------------------------------------
        //   INSERTAR AL INICIO (producto urgente) -> O(1)
        // ---------------------------------------------------------------
        public void InsertarAlInicio(string dato)
        {
            cabeza = new Nodo(dato, cabeza);
            cantidad++;
        }

        // ---------------------------------------------------------------
        //   INSERTAR AL FINAL -> O(n)
        // ---------------------------------------------------------------
        public void InsertarAlFinal(string dato)
        {
            Nodo nuevo = new Nodo(dato);

            if (cabeza == null)
            {
                cabeza = nuevo;
            }
            else
            {
                Nodo ultimo = cabeza;
                while (ultimo.Siguiente != null)
                    ultimo = ultimo.Siguiente;

                ultimo.Siguiente = nuevo;
            }

            cantidad++;
        }

        // ---------------------------------------------------------------
        //   INSERTAR EN UNA POSICIÓN ESPECÍFICA (empezando en 0)
        // ---------------------------------------------------------------
        public bool InsertarEnPosicion(string dato, int posicion)
        {
            if (posicion < 0 || posicion > cantidad)
                return false;

            if (posicion == 0)
            {
                InsertarAlInicio(dato);
                return true;
            }

            // Nos detenemos en el nodo que quedará justo antes del nuevo
            Nodo anterior = cabeza!;
            for (int i = 0; i < posicion - 1; i++)
                anterior = anterior.Siguiente!;

            anterior.Siguiente = new Nodo(dato, anterior.Siguiente);
            cantidad++;
            return true;
        }

        // ---------------------------------------------------------------
        //   ELIMINAR UN PRODUCTO por su nombre
        // ---------------------------------------------------------------
        public bool Eliminar(string dato)
        {
            Nodo? anterior = null;
            Nodo? actual = cabeza;

            while (actual != null)
            {
                if (MismoProducto(actual.Dato, dato))
                {
                    if (anterior == null)
                        cabeza = actual.Siguiente;          // Era el primero
                    else
                        anterior.Siguiente = actual.Siguiente;

                    cantidad--;
                    return true;
                }

                anterior = actual;
                actual = actual.Siguiente;
            }

            return false;
        }

        // ---------------------------------------------------------------
        //   BUSCAR: devuelve la posición del producto o -1 si no está
        // ---------------------------------------------------------------
        public int Buscar(string dato)
        {
            Nodo? actual = cabeza;
            int posicion = 0;

            while (actual != null)
            {
                if (MismoProducto(actual.Dato, dato))
                    return posicion;

                actual = actual.Siguiente;
                posicion++;
            }

            return -1;
        }

        // ---------------------------------------------------------------
        //   INVERTIR LA LISTA (cambia el orden de compra)
        // ---------------------------------------------------------------
        public void Invertir()
        {
            Nodo? anterior = null;
            Nodo? actual = cabeza;

            while (actual != null)
            {
                Nodo? siguiente = actual.Siguiente;
                actual.Siguiente = anterior;
                anterior = actual;
                actual = siguiente;
            }

            cabeza = anterior;
        }

        // ---------------------------------------------------------------
        //   VACIAR LA LISTA
        // ---------------------------------------------------------------
        public void Vaciar()
        {
            cabeza = null;
            cantidad = 0;
        }

        // ---------------------------------------------------------------
        //   MOSTRAR: recorre e imprime la lista de compras
        // ---------------------------------------------------------------
        public void Mostrar()
        {
            if (cabeza == null)
            {
                Console.WriteLine("   Tu carrito/lista de supermercado está vacío.");
                return;
            }

            Console.Write("   Inicio -> ");
            for (Nodo? actual = cabeza; actual != null; actual = actual.Siguiente)
                Console.Write($"[{actual.Dato}] -> ");

            Console.WriteLine("null");
            Console.WriteLine($"   Total de productos: {cantidad}");
        }
    }

    // -----------------------------------------------------------------
    //   OPCIONES DEL MENÚ
    // -----------------------------------------------------------------
    enum Opcion
    {
        Salir = 0,
        AgregarAlInicio = 1,
        AgregarAlFinal = 2,
        AgregarEnPosicion = 3,
        Eliminar = 4,
        Buscar = 5,
        Invertir = 6,
        Vaciar = 7,
        CargarEjemplo = 8
    }

    // -----------------------------------------------------------------
    //   PROGRAMA PRINCIPAL
    // -----------------------------------------------------------------
    class Program
    {
        static readonly string[] ProductosBasicos = { "Leche", "Huevos", "Pan", "Manzanas", "Café" };

        static void Main(string[] args)
        {
            ListaEnlazada lista = new ListaEnlazada();
            Opcion opcion;

            do
            {
                MostrarMenu(lista);
                opcion = (Opcion)LeerEntero("Elige una opción: ");
                Console.WriteLine();

                switch (opcion)
                {
                    case Opcion.AgregarAlInicio:   AgregarAlInicio(lista);   break;
                    case Opcion.AgregarAlFinal:    AgregarAlFinal(lista);    break;
                    case Opcion.AgregarEnPosicion: AgregarEnPosicion(lista); break;
                    case Opcion.Eliminar:          EliminarProducto(lista);  break;
                    case Opcion.Buscar:            BuscarProducto(lista);    break;
                    case Opcion.Invertir:          InvertirLista(lista);     break;
                    case Opcion.Vaciar:            VaciarLista(lista);       break;
                    case Opcion.CargarEjemplo:     CargarEjemplo(lista);     break;
                    case Opcion.Salir:             Console.WriteLine("   ¡Buena compra! Hasta luego."); break;
                    default:                       Console.WriteLine("   Opción no válida."); break;
                }

            } while (opcion != Opcion.Salir);
        }

        // ---------------------------------------------------------------
        //   MENÚ
        // ---------------------------------------------------------------
        static void MostrarMenu(ListaEnlazada lista)
        {
            Console.WriteLine();
            Console.WriteLine("========= LISTA DE SUPERMERCADO =========");
            lista.Mostrar();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine(" 1. Agregar producto al inicio (urgente)");
            Console.WriteLine(" 2. Agregar producto al final");
            Console.WriteLine(" 3. Agregar producto en una posición");
            Console.WriteLine(" 4. Eliminar producto comprado");
            Console.WriteLine(" 5. Buscar un producto");
            Console.WriteLine(" 6. Invertir orden de la lista");
            Console.WriteLine(" 7. Vaciar lista de compras");
            Console.WriteLine(" 8. Cargar productos básicos de ejemplo");
            Console.WriteLine(" 0. Salir");
            Console.WriteLine("=========================================");
        }

        // ---------------------------------------------------------------
        //   ACCIONES DEL MENÚ
        // ---------------------------------------------------------------
        static void AgregarAlInicio(ListaEnlazada lista)
        {
            lista.InsertarAlInicio(LeerTexto("Nombre del producto urgente: "));
            Console.WriteLine("   Producto agregado al inicio.");
        }

        static void AgregarAlFinal(ListaEnlazada lista)
        {
            lista.InsertarAlFinal(LeerTexto("Nombre del producto a agregar: "));
            Console.WriteLine("   Producto agregado al final.");
        }

        static void AgregarEnPosicion(ListaEnlazada lista)
        {
            string producto = LeerTexto("Nombre del producto: ");
            int posicion = LeerEntero($"Posición (0 a {lista.Contar()}): ");

            Console.WriteLine(lista.InsertarEnPosicion(producto, posicion)
                ? "   Producto insertado correctamente."
                : "   Posición no válida.");
        }

        static void EliminarProducto(ListaEnlazada lista)
        {
            string producto = LeerTexto("Nombre del producto a eliminar: ");

            Console.WriteLine(lista.Eliminar(producto)
                ? "   Producto eliminado de la lista."
                : "   El producto no se encuentra en la lista.");
        }

        static void BuscarProducto(ListaEnlazada lista)
        {
            string producto = LeerTexto("Producto a buscar: ");
            int posicion = lista.Buscar(producto);

            Console.WriteLine(posicion >= 0
                ? $"   El producto '{producto}' está en la posición {posicion}."
                : $"   El producto '{producto}' no está en la lista.");
        }

        static void InvertirLista(ListaEnlazada lista)
        {
            lista.Invertir();
            Console.WriteLine("   Orden de la lista invertido.");
        }

        static void VaciarLista(ListaEnlazada lista)
        {
            lista.Vaciar();
            Console.WriteLine("   Lista de compras vaciada.");
        }

        static void CargarEjemplo(ListaEnlazada lista)
        {
            foreach (string producto in ProductosBasicos)
                lista.InsertarAlFinal(producto);

            Console.WriteLine($"   Se agregaron: {string.Join(", ", ProductosBasicos)}.");
        }

        // ---------------------------------------------------------------
        //   LECTURA DE DATOS
        // ---------------------------------------------------------------

        // Lee un texto asegurando que no esté vacío
        static string LeerTexto(string mensaje)
        {
            string? entrada;
            do
            {
                Console.Write(mensaje);
                entrada = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(entrada));

            return entrada.Trim();
        }

        // Lee un número entero validando la entrada del usuario
        static int LeerEntero(string mensaje)
        {
            Console.Write(mensaje);

            int numero;
            while (!int.TryParse(Console.ReadLine(), out numero))
                Console.Write("   Entrada inválida. Escribe un número entero: ");

            return numero;
        }
    }
}