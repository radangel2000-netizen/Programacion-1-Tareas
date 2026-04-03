using System;
using System.Collections.Generic;

// ============================================================
//  PILAR 1: ABSTRACCIÓN
//  Clase abstracta LLamada - representa el concepto general
//  de una llamada sin implementar el precio específico
// ============================================================
abstract class LLamada
{
    // PILAR 2: ENCAPSULAMIENTO - atributos privados
    private string numOrigen;
    private string numDestino;
    private double duracion;

    // Constructor
    public LLamada(string param1, string param2, double param3)
    {
        numOrigen  = param1;
        numDestino = param2;
        duracion   = param3;
    }

    // Getters (Encapsulamiento)
    public string getNumOrigen()   => numOrigen;
    public string getNumDestino()  => numDestino;
    public double getDuracion()    => duracion;

    // PILAR 4: POLIMORFISMO
    // Método abstracto - cada tipo de llamada calcula su precio distinto
    public abstract double calcularPrecio();
}

// ============================================================
//  PILAR 3: HERENCIA
//  LlamadaLocal hereda de LLamada
//  PILAR 4: POLIMORFISMO - sobreescribe calcularPrecio()
// ============================================================
class LlamadaLocal : LLamada
{
    // PILAR 2: ENCAPSULAMIENTO
    private double precio;

    // Llamada local: 15 céntimos por segundo
    public LlamadaLocal(string param1, string param2, int param3)
        : base(param1, param2, param3)
    {
        precio = 0.15;
    }

    // PILAR 4: POLIMORFISMO - implementación específica del precio
    public override double calcularPrecio()
    {
        return getDuracion() * precio;
    }

    public override string ToString()
    {
        return $"[LOCAL]      Origen: {getNumOrigen(),-12} " +
               $"Destino: {getNumDestino(),-12} " +
               $"Duración: {getDuracion(),6}s  " +
               $"Coste: {calcularPrecio(),8:F2} €";
    }
}

// ============================================================
//  PILAR 3: HERENCIA
//  LlamadaProvincial hereda de LLamada
//  PILAR 4: POLIMORFISMO - sobreescribe calcularPrecio()
// ============================================================
class LLamadaProvincial : LLamada
{
    // PILAR 2: ENCAPSULAMIENTO - atributos privados
    private double precio1;  // franja 1: 20 céntimos
    private double precio2;  // franja 2: 25 céntimos
    private double precio3;  // franja 3: 30 céntimos
    private int    franja;

    // Llamada provincial: precio según franja horaria
    public LLamadaProvincial(string param1, string param2, int param3, int param4)
        : base(param1, param2, param3)
    {
        precio1 = 0.20;
        precio2 = 0.25;
        precio3 = 0.30;
        franja  = param4;
    }

    // PILAR 4: POLIMORFISMO - implementación específica del precio
    public override double calcularPrecio()
    {
        switch (franja)
        {
            case 1:  return getDuracion() * precio1;
            case 2:  return getDuracion() * precio2;
            case 3:  return getDuracion() * precio3;
            default: return 0;
        }
    }

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
//  PILAR 1: ABSTRACCIÓN
//  Clase Centralita - gestiona y controla todas las llamadas
//  PILAR 2: ENCAPSULAMIENTO - sus datos internos son privados
// ============================================================
class Centralita
{
    // PILAR 2: ENCAPSULAMIENTO - atributos privados
    private int    cont;  // contador de llamadas
    private double acum;  // acumulado total en euros
    private List<LLamada> llamadas;

    public Centralita()
    {
        cont    = 0;
        acum    = 0.0;
        llamadas = new List<LLamada>();
    }

    // Getters
    public int    getTotalLlamadas()  => cont;
    public double getTotalFacturado() => acum;

    // Registrar una llamada
    // PILAR 4: POLIMORFISMO - acepta cualquier tipo de LLamada
    public void registrarLlamada(LLamada param)
    {
        cont++;
        acum += param.calcularPrecio();
        llamadas.Add(param);

        // Muestra la llamada según se registra
        Console.WriteLine($"  #{cont:D2} → {param}");
    }

    // Informe final
    public void mostrarInforme()
    {
        Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║              INFORME FINAL - CENTRALITA              ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════╣");
        Console.WriteLine($"║  Total de llamadas registradas : {cont,5}               ║");
        Console.WriteLine($"║  Facturación total             : {acum,8:F2} €          ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝");
    }
}

// ============================================================
//  CLASE PRINCIPAL - Practica2
// ============================================================
class Practica2
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║         CENTRALITA TELEFÓNICA - POO en C#           ║");
        Console.WriteLine("║   Abstracción | Encapsulamiento | Herencia | Polim. ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");

        // Crear la centralita
        Centralita centralita = new Centralita();

        Console.WriteLine("📞 Registrando llamadas...\n");

        // Registrar llamadas locales
        centralita.registrarLlamada(new LlamadaLocal("600111222", "600333444", 120));
        centralita.registrarLlamada(new LlamadaLocal("600555666", "600777888", 45));
        centralita.registrarLlamada(new LlamadaLocal("611000111", "611222333", 200));

        // Registrar llamadas provinciales en distintas franjas
        centralita.registrarLlamada(new LLamadaProvincial("700100200", "700300400", 90,  1)); // franja 1: 0.20€/s
        centralita.registrarLlamada(new LLamadaProvincial("700500600", "700700800", 60,  2)); // franja 2: 0.25€/s
        centralita.registrarLlamada(new LLamadaProvincial("711000111", "711222333", 150, 3)); // franja 3: 0.30€/s
        centralita.registrarLlamada(new LLamadaProvincial("722111222", "722333444", 30,  1)); // franja 1
        centralita.registrarLlamada(new LLamadaProvincial("733444555", "733666777", 75,  2)); // franja 2

        // Pedir informe a la centralita
        centralita.mostrarInforme();

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
