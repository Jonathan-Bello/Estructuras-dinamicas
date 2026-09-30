// Playlist de canciones con lista enlazada (memoria dinámica)
using System;

namespace ListasEnlazadas;

record Cancion(string Titulo, string Artista)
{
    public override string ToString() => $"\"{Titulo}\" - {Artista}";
}

class Nodo
{
    public Cancion Dato;
    public Nodo? Siguiente;
    public Nodo(Cancion cancion) => Dato = cancion;
}

class Playlist
{
    private Nodo? cabeza;
    private int cantidad;

    public bool EstaVacia() => cabeza == null;
    public int Contar() => cantidad;

    public void AgregarAlInicio(Cancion cancion) => AgregarEnPosicion(cancion, 0);
    public void AgregarAlFinal(Cancion cancion) => AgregarEnPosicion(cancion, cantidad);

    // Devuelve el nodo en la posición i (solo se usa con i válido)
    private Nodo En(int i)
    {
        Nodo n = cabeza!;
        while (i-- > 0) n = n.Siguiente!;
        return n;
    }

    public bool AgregarEnPosicion(Cancion cancion, int posicion)
    {
        if (posicion < 0 || posicion > cantidad) return false;

        if (posicion == 0)
            cabeza = new Nodo(cancion) { Siguiente = cabeza };
        else
        {
            Nodo anterior = En(posicion - 1);
            anterior.Siguiente = new Nodo(cancion) { Siguiente = anterior.Siguiente };
        }
        cantidad++;
        return true;
    }

    public bool EliminarPorTitulo(string titulo)
    {
        int pos = BuscarPorTitulo(titulo);
        if (pos < 0) return false;

        if (pos == 0)
            cabeza = cabeza!.Siguiente;
        else
        {
            Nodo anterior = En(pos - 1);
            anterior.Siguiente = anterior.Siguiente!.Siguiente;
        }
        cantidad--;
        return true;
    }

    public int BuscarPorTitulo(string titulo)
    {
        int pos = 0;
        for (Nodo? n = cabeza; n != null; n = n.Siguiente, pos++)
            if (n.Dato.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase))
                return pos;
        return -1;
    }

    public void InvertirPlaylist()
    {
        Nodo? anterior = null, actual = cabeza;
        while (actual != null)
            (actual.Siguiente, anterior, actual) = (anterior, actual, actual.Siguiente);
        cabeza = anterior;
    }

    public void Vaciar()
    {
        cabeza = null;
        cantidad = 0;
    }

    public void Mostrar()
    {
        if (cabeza == null)
        {
            Console.WriteLine("  La playlist está vacía.");
            return;
        }

        Console.WriteLine("  [EN REPRODUCCIÓN]");
        int pos = 0;
        for (Nodo? n = cabeza; n != null; n = n.Siguiente)
            Console.WriteLine($"   {pos++}. {n.Dato}");
        Console.WriteLine($"  Total de canciones: {cantidad}");
    }
}

class Program
{
    static void Main()
    {
        var miPlaylist = new Playlist();
        int opcion;

        do
        {
            Console.WriteLine("\n========= REPRODUCTOR DE MÚSICA =========");
            miPlaylist.Mostrar();
            Console.WriteLine(@"-----------------------------------------
 1. Agregar canción al inicio (Reproducir ya)
 2. Agregar canción al final de la cola
 3. Insertar canción en turno específico
 4. Eliminar canción por título
 5. Buscar canción por título
 6. Invertir orden de la playlist
 7. Vaciar playlist
 8. Cargar playlist de ejemplo
 0. Salir
=========================================");

            opcion = LeerEntero("Elige una opción: ");
            Console.WriteLine();

            switch (opcion)
            {
                case 1:
                    miPlaylist.AgregarAlInicio(LeerCancion());
                    Console.WriteLine("  Canción colocada al inicio.");
                    break;
                case 2:
                    miPlaylist.AgregarAlFinal(LeerCancion());
                    Console.WriteLine("  Canción agregada al final de la lista.");
                    break;
                case 3:
                    var c = LeerCancion();
                    int pos = LeerEntero($"Posición (0 a {miPlaylist.Contar()}): ");
                    Console.WriteLine(miPlaylist.AgregarEnPosicion(c, pos)
                        ? "  Canción insertada en el turno especificado."
                        : "  Posición no válida.");
                    break;
                case 4:
                    Console.Write("Título de la canción a eliminar: ");
                    Console.WriteLine(miPlaylist.EliminarPorTitulo(Console.ReadLine() ?? "")
                        ? "  Canción eliminada de la playlist."
                        : "  No se encontró la canción.");
                    break;
                case 5:
                    Console.Write("Título de la canción a buscar: ");
                    int posicion = miPlaylist.BuscarPorTitulo(Console.ReadLine() ?? "");
                    Console.WriteLine(posicion >= 0
                        ? $"  La canción está en el turno {posicion}."
                        : "  La canción no está en la playlist.");
                    break;
                case 6:
                    miPlaylist.InvertirPlaylist();
                    Console.WriteLine("  Se ha invertido el orden de la playlist.");
                    break;
                case 7:
                    miPlaylist.Vaciar();
                    Console.WriteLine("  Playlist vaciada.");
                    break;
                case 8:
                    foreach (var (titulo, artista) in new[] {
                        ("Bohemian Rhapsody", "Queen"), ("Hotel California", "Eagles"),
                        ("Billie Jean", "Michael Jackson"), ("Shape of You", "Ed Sheeran") })
                        miPlaylist.AgregarAlFinal(new Cancion(titulo, artista));
                    Console.WriteLine("  Se cargaron canciones de ejemplo.");
                    break;
                case 0:
                    Console.WriteLine("  ¡Reproductor cerrado!");
                    break;
                default:
                    Console.WriteLine("  Opción no válida.");
                    break;
            }
        } while (opcion != 0);
    }

    static Cancion LeerCancion()
    {
        Console.Write("  Título de la canción: ");
        string titulo = Console.ReadLine() ?? "Sin título";
        Console.Write("  Artista/Banda: ");
        return new Cancion(titulo, Console.ReadLine() ?? "Desconocido");
    }

    static int LeerEntero(string mensaje)
    {
        int numero;
        Console.Write(mensaje);
        while (!int.TryParse(Console.ReadLine(), out numero))
            Console.Write("  Entrada inválida. Escribe un número entero: ");
        return numero;
    }
}