using System;
using System.Collections.Generic;

// ============================================================
//  CLASE BASE ABSTRACTA - AutoBus
//  Aplica: Abstracción + Encapsulamiento
// ============================================================
abstract class AutoBus
{
    private string nombre;
    private string ruta;
    private double precioPasaje;
    private int    asientosDisponibles;
    private double totalVentas;

    public AutoBus(string nombre, string ruta, double precio, int asientos)
    {
        this.nombre              = nombre;
        this.ruta                = ruta;
        this.precioPasaje        = precio;
        this.asientosDisponibles = asientos;
        this.totalVentas         = 0;
    }

    // Getters
    public string getNombre()             => nombre;
    public string getRuta()               => ruta;
    public double getPrecioPasaje()       => precioPasaje;
    public int    getAsientosDisponibles()=> asientosDisponibles;
    public double getTotalVentas()        => totalVentas;

    // Vender pasajes - Herencia compartida
    public bool venderPasaje(int cantidad)
    {
        if (cantidad > asientosDisponibles)
        {
            Console.WriteLine($"  ❌ No hay suficientes asientos en {nombre}. Disponibles: {asientosDisponibles}");
            return false;
        }
        asientosDisponibles -= cantidad;
        totalVentas         += cantidad * precioPasaje;
        return true;
    }

    // POLIMORFISMO - cada bus muestra info distinta
    public abstract void mostrarInfo();
}

// ============================================================
//  CLASE HIJA - AutoBusPlatinum
//  Aplica: Herencia + Polimorfismo
// ============================================================
class AutoBusPlatinum : AutoBus
{
    private bool tieneWifi;
    private bool tieneAireAcondicionado;

    public AutoBusPlatinum(string ruta, double precio, int asientos)
        : base("Auto Bus Platinum", ruta, precio, asientos)
    {
        tieneWifi              = true;
        tieneAireAcondicionado = true;
    }

    public override void mostrarInfo()
    {
        Console.WriteLine($"🚌 {getNombre()} | Ruta: {getRuta()}");
        Console.WriteLine($"   Precio pasaje : RD$ {getPrecioPasaje():N2}");
        Console.WriteLine($"   Total ventas  : RD$ {getTotalVentas():N2}");
        Console.WriteLine($"   Asientos disp.: {getAsientosDisponibles()}");
        Console.WriteLine($"   WiFi: {(tieneWifi ? "Sí" : "No")} | A/C: {(tieneAireAcondicionado ? "Sí" : "No")}");
    }
}

// ============================================================
//  CLASE HIJA - AutoBusGold
//  Aplica: Herencia + Polimorfismo
// ============================================================
class AutoBusGold : AutoBus
{
    private bool tieneTV;

    public AutoBusGold(string ruta, double precio, int asientos)
        : base("Auto Bus Gold", ruta, precio, asientos)
    {
        tieneTV = true;
    }

    public override void mostrarInfo()
    {
        Console.WriteLine($"🚌 {getNombre()} | Ruta: {getRuta()}");
        Console.WriteLine($"   Precio pasaje : RD$ {getPrecioPasaje():N2}");
        Console.WriteLine($"   Total ventas  : RD$ {getTotalVentas():N2}");
        Console.WriteLine($"   Asientos disp.: {getAsientosDisponibles()}");
        Console.WriteLine($"   TV a bordo    : {(tieneTV ? "Sí" : "No")}");
    }
}

// ============================================================
//  CLASE HIJA - AutoBusEconomico
//  Aplica: Herencia + Polimorfismo
// ============================================================
class AutoBusEconomico : AutoBus
{
    public AutoBusEconomico(string ruta, double precio, int asientos)
        : base("Auto Bus Económico", ruta, precio, asientos)
    {
    }

    public override void mostrarInfo()
    {
        Console.WriteLine($"🚌 {getNombre()} | Ruta: {getRuta()}");
        Console.WriteLine($"   Precio pasaje : RD$ {getPrecioPasaje():N2}");
        Console.WriteLine($"   Total ventas  : RD$ {getTotalVentas():N2}");
        Console.WriteLine($"   Asientos disp.: {getAsientosDisponibles()}");
        Console.WriteLine($"   Tarifa básica sin servicios adicionales");
    }
}

// ============================================================
//  CLASE Ruta - gestiona los buses de una ruta
// ============================================================
class Ruta
{
    private string nombreRuta;
    private string origen;
    private string destino;
    private double distanciaKm;
    private List<AutoBus> buses;

    public Ruta(string nombreRuta, string origen, string destino, double distancia)
    {
        this.nombreRuta  = nombreRuta;
        this.origen      = origen;
        this.destino     = destino;
        this.distanciaKm = distancia;
        this.buses       = new List<AutoBus>();
    }

    public void agregarBus(AutoBus bus)
    {
        buses.Add(bus);
    }

    public void cobrarPasaje(AutoBus bus, int cantidad)
    {
        if (bus.venderPasaje(cantidad))
        {
            double total = cantidad * bus.getPrecioPasaje();
            Console.WriteLine($"  ✅ {bus.getNombre()} | {cantidad} Pasajero(s) " +
                              $"| Venta: RD$ {total:N2} " +
                              $"| Quedan {bus.getAsientosDisponibles()} asientos disponibles");
        }
    }

    public void mostrarInforme()
    {
        Console.WriteLine($"\n╔══════════════════════════════════════════════════════╗");
        Console.WriteLine($"║         INFORME DE RUTA - {nombreRuta,-27}║");
        Console.WriteLine($"╠══════════════════════════════════════════════════════╣");
        Console.WriteLine($"║  Origen  : {origen,-43}║");
        Console.WriteLine($"║  Destino : {destino,-43}║");
        Console.WriteLine($"║  Distancia: {distanciaKm} km{new string(' ', 39 - distanciaKm.ToString().Length)}║");
        Console.WriteLine($"╚══════════════════════════════════════════════════════╝");

        double totalGeneral = 0;
        foreach (var bus in buses)
        {
            Console.WriteLine();
            bus.mostrarInfo();
            totalGeneral += bus.getTotalVentas();
        }

        Console.WriteLine($"\n💰 TOTAL RECAUDADO EN LA RUTA: RD$ {totalGeneral:N2}");
    }
}

// ============================================================
//  PROGRAMA PRINCIPAL
// ============================================================
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║       SISTEMA DE COBRO DE PASAJE - RUTA DE BUS      ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");

        // Crear ruta
        Ruta ruta = new Ruta("Ruta 1", "Santo Domingo", "Santiago", 155.0);

        // Crear buses (Herencia + Polimorfismo)
        AutoBusPlatinum  platinum  = new AutoBusPlatinum ("Santo Domingo → Santiago", 1000.00, 22);
        AutoBusGold      gold      = new AutoBusGold     ("Santo Domingo → Santiago",  800.00, 15);
        AutoBusEconomico economico = new AutoBusEconomico("Santo Domingo → Santiago",  500.00, 30);

        ruta.agregarBus(platinum);
        ruta.agregarBus(gold);
        ruta.agregarBus(economico);

        Console.WriteLine("📋 Registrando ventas de pasajes...\n");

        // Cobrar pasajes - Polimorfismo en acción
        ruta.cobrarPasaje(platinum,  5);
        ruta.cobrarPasaje(gold,      3);
        ruta.cobrarPasaje(economico, 8);
        ruta.cobrarPasaje(platinum,  2);
        ruta.cobrarPasaje(gold,      4);

        // Intento de venta que supera asientos disponibles
        ruta.cobrarPasaje(gold, 20);

        // Mostrar informe final
        ruta.mostrarInforme();

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
