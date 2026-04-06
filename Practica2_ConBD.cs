// ============================================================
//  CENTRALITA TELEFÓNICA - CON BASE DE DATOS
//  Requiere: MySql.Data (NuGet)
//  Comando NuGet: Install-Package MySql.Data
// ============================================================
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

// ============================================================
//  CLASE ABSTRACTA - LLamada
// ============================================================
abstract class LLamada
{
    private string numOrigen;
    private string numDestino;
    private double duracion;

    public LLamada(string param1, string param2, double param3)
    {
        numOrigen  = param1;
        numDestino = param2;
        duracion   = param3;
    }

    public string getNumOrigen()  => numOrigen;
    public string getNumDestino() => numDestino;
    public double getDuracion()   => duracion;

    public abstract double calcularPrecio();
    public abstract string getTipo();
    public abstract int    getFranja();
}

// ============================================================
//  LlamadaLocal
// ============================================================
class LlamadaLocal : LLamada
{
    private double precio;

    public LlamadaLocal(string param1, string param2, int param3)
        : base(param1, param2, param3)
    {
        precio = 0.15;
    }

    public override double calcularPrecio() => getDuracion() * precio;
    public override string getTipo()        => "LOCAL";
    public override int    getFranja()      => 0; // NULL en BD

    public override string ToString()
    {
        return $"[LOCAL]      Origen: {getNumOrigen(),-12} " +
               $"Destino: {getNumDestino(),-12} " +
               $"Duración: {getDuracion(),6}s  " +
               $"Coste: {calcularPrecio(),8:F2} €";
    }
}

// ============================================================
//  LLamadaProvincial
// ============================================================
class LLamadaProvincial : LLamada
{
    private double precio1 = 0.20;
    private double precio2 = 0.25;
    private double precio3 = 0.30;
    private int    franja;

    public LLamadaProvincial(string param1, string param2, int param3, int param4)
        : base(param1, param2, param3)
    {
        franja = param4;
    }

    public override double calcularPrecio()
    {
        switch (franja)
        {
            case 1: return getDuracion() * precio1;
            case 2: return getDuracion() * precio2;
            case 3: return getDuracion() * precio3;
            default: return 0;
        }
    }

    public override string getTipo()   => "PROVINCIAL";
    public override int    getFranja() => franja;

    public override string ToString()
    {
        return $"[PROVINCIAL] Origen: {getNumOrigen(),-12} " +
               $"Destino: {getNumDestino(),-12} " +
               $"Duración: {getDuracion(),6}s  " +
               $"Franja: {franja}  " +
               $"Coste: {calcularPrecio(),8:F2} €";
    }
}

// ============================================================
//  CLASE - ConexionBD
// ============================================================
class ConexionBD
{
<<<<<<< HEAD
    
=======

>>>>>>> 47848fd2ae95cb949927fa0ec81e313209f96824
    private static string servidor  = "localhost";
    private static string baseDatos = "CentralitaTelefonica";
    private static string usuario   = "root";
    private static string contrasena = "";  

    private static string cadenaConexion =
        $"Server={servidor};Database={baseDatos};" +
        $"User ID={usuario};Password={contrasena};";

    // Guardar una llamada en la BD
    public static bool GuardarLlamada(LLamada llamada)
    {
        try
        {
            using (var conn = new MySqlConnection(cadenaConexion))
            {
                conn.Open();
                string sql = @"INSERT INTO Llamadas 
                               (tipo_llamada, num_origen, num_destino, duracion, franja, costo)
                               VALUES 
                               (@tipo, @origen, @destino, @duracion, @franja, @costo)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tipo",     llamada.getTipo());
                    cmd.Parameters.AddWithValue("@origen",   llamada.getNumOrigen());
                    cmd.Parameters.AddWithValue("@destino",  llamada.getNumDestino());
                    cmd.Parameters.AddWithValue("@duracion", llamada.getDuracion());
                    cmd.Parameters.AddWithValue("@franja",
                        llamada.getFranja() == 0 ? (object)DBNull.Value : llamada.getFranja());
                    cmd.Parameters.AddWithValue("@costo",    llamada.calcularPrecio());
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ⚠️  Error al guardar en BD: {ex.Message}");
            return false;
        }
    }

    // Mostrar informe desde la BD
    public static void MostrarInformeBD()
    {
        try
        {
            using (var conn = new MySqlConnection(cadenaConexion))
            {
                conn.Open();

                // Total llamadas y facturación
                string sqlTotal = "SELECT COUNT(*) AS total, ROUND(SUM(costo),2) AS facturacion FROM Llamadas";
                using (var cmd = new MySqlCommand(sqlTotal, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"\n╔══════════════════════════════════════════════════════╗");
                        Console.WriteLine($"║        INFORME DESDE BASE DE DATOS                   ║");
                        Console.WriteLine($"╠══════════════════════════════════════════════════════╣");
                        Console.WriteLine($"║  Total llamadas registradas : {reader["total"],5}               ║");
                        Console.WriteLine($"║  Facturación total          : {reader["facturacion"],8} €          ║");
                        Console.WriteLine($"╚══════════════════════════════════════════════════════╝");
                    }
                }

                // Detalle de todas las llamadas
                Console.WriteLine("\n─── Detalle de llamadas en BD ───");
                string sqlDetalle = "SELECT * FROM ResumenLlamadas ORDER BY id";
                using (var cmd2 = new MySqlCommand(sqlDetalle, conn))
                using (var reader2 = cmd2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        string franja = reader2["franja"] == DBNull.Value
                            ? "  -  "
                            : $"F:{reader2["franja"]}";

                        Console.WriteLine($"  #{reader2["id"],-3} " +
                                          $"[{reader2["tipo_llamada"],-10}] " +
                                          $"{reader2["num_origen"]} → {reader2["num_destino"]} " +
                                          $"| {reader2["duracion"]}s " +
                                          $"| {franja} " +
                                          $"| {reader2["costo_euros"]} €");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ⚠️  Error al leer BD: {ex.Message}");
        }
    }
}

// ============================================================
//  CLASE Centralita
// ============================================================
class Centralita
{
    private int    cont;
    private double acum;
    private List<LLamada> llamadas;

    public Centralita()
    {
        cont     = 0;
        acum     = 0.0;
        llamadas = new List<LLamada>();
    }

    public int    getTotalLlamadas()  => cont;
    public double getTotalFacturado() => acum;

    public void registrarLlamada(LLamada param)
    {
        cont++;
        acum += param.calcularPrecio();
        llamadas.Add(param);

        // Guardar en base de datos
        bool guardado = ConexionBD.GuardarLlamada(param);
        string icono  = guardado ? "✅" : "⚠️ ";

        Console.WriteLine($"  {icono} #{cont:D2} → {param}");
    }

    public void mostrarInforme()
    {
        Console.WriteLine($"\n╔══════════════════════════════════════════════════════╗");
        Console.WriteLine($"║          INFORME FINAL - CENTRALITA                  ║");
        Console.WriteLine($"╠══════════════════════════════════════════════════════╣");
        Console.WriteLine($"║  Total de llamadas registradas : {cont,5}               ║");
        Console.WriteLine($"║  Facturación total             : {acum,8:F2} €          ║");
        Console.WriteLine($"╚══════════════════════════════════════════════════════╝");
    }
}

// ============================================================
//  PROGRAMA PRINCIPAL - Practica2
// ============================================================
class Practica2
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║    CENTRALITA TELEFÓNICA + BASE DE DATOS MySQL      ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");

        Centralita centralita = new Centralita();

        Console.WriteLine("📞 Registrando llamadas y guardando en BD...\n");

        // Llamadas locales
        centralita.registrarLlamada(new LlamadaLocal("600111222", "600333444", 120));
        centralita.registrarLlamada(new LlamadaLocal("600555666", "600777888",  45));
        centralita.registrarLlamada(new LlamadaLocal("611000111", "611222333", 200));

        // Llamadas provinciales
        centralita.registrarLlamada(new LLamadaProvincial("700100200", "700300400",  90, 1));
        centralita.registrarLlamada(new LLamadaProvincial("700500600", "700700800",  60, 2));
        centralita.registrarLlamada(new LLamadaProvincial("711000111", "711222333", 150, 3));
        centralita.registrarLlamada(new LLamadaProvincial("722111222", "722333444",  30, 1));
        centralita.registrarLlamada(new LLamadaProvincial("733444555", "733666777",  75, 2));

        // Informe en consola
        centralita.mostrarInforme();

        // Informe desde la base de datos
        ConexionBD.MostrarInformeBD();

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
