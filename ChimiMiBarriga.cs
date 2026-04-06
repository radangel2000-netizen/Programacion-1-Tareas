using System;
using System.Collections.Generic;

// ============================================================
//  CLASE: Ingrediente
//  Representa un ingrediente adicional con nombre y precio
// ============================================================
class Ingrediente
{
    private string nombre;
    private double precio;

    public Ingrediente(string nombre, double precio)
    {
        this.nombre = nombre;
        this.precio = precio;
    }

    public string getNombre() => nombre;
    public double getPrecio() => precio;

    public override string ToString()
    {
        return $"  + {nombre,-20} RD$ {precio:F2}";
    }
}

// ============================================================
//  CLASE BASE: Hamburguesa
//  Maneja pan, carne, precio base y hasta 4 ingredientes
// ============================================================
class Hamburguesa
{
    private string tipoPan;
    private string tipoCarne;
    private double precioBase;
    protected List<Ingrediente> ingredientes;
    protected int maxIngredientes;

    // Constructor solo recibe pan, carne y precio
    public Hamburguesa(string tipoPan, string tipoCarne, double precioBase)
    {
        this.tipoPan       = tipoPan;
        this.tipoCarne     = tipoCarne;
        this.precioBase    = precioBase;
        this.ingredientes  = new List<Ingrediente>();
        this.maxIngredientes = 4;
    }

    // Getters
    public string getTipoPan()    => tipoPan;
    public string getTipoCarne()  => tipoCarne;
    public double getPrecioBase() => precioBase;

    // Agregar ingrediente adicional
    public virtual bool agregarIngrediente(Ingrediente ingrediente)
    {
        if (ingredientes.Count >= maxIngredientes)
        {
            Console.WriteLine($"  ⚠️  Máximo de ingredientes alcanzado ({maxIngredientes}).");
            return false;
        }
        ingredientes.Add(ingrediente);
        return true;
    }

    // Calcular precio total
    public virtual double calcularTotal()
    {
        double total = precioBase;
        foreach (var ing in ingredientes)
            total += ing.getPrecio();
        return total;
    }

    // Mostrar detalle completo
    public virtual void mostrarDetalle()
    {
        Console.WriteLine("╔══════════════════════════════════════════════════╗");
        Console.WriteLine("║         🍔 CHIMI MIBARRIGA - Sr. Billy Navaja   ║");
        Console.WriteLine("╠══════════════════════════════════════════════════╣");
        Console.WriteLine($"║  Tipo       : {getTipoHamburguesa(),-36}║");
        Console.WriteLine($"║  Pan        : {tipoPan,-36}║");
        Console.WriteLine($"║  Carne      : {tipoCarne,-36}║");
        Console.WriteLine($"║  Precio base: RD$ {precioBase,-32:F2}║");
        Console.WriteLine("╠══════════════════════════════════════════════════╣");

        if (ingredientes.Count > 0)
        {
            Console.WriteLine("║  INGREDIENTES ADICIONALES:                       ║");
            foreach (var ing in ingredientes)
                Console.WriteLine($"║  {ing,-48}║");
        }
        else
        {
            Console.WriteLine("║  Sin ingredientes adicionales                    ║");
        }

        Console.WriteLine("╠══════════════════════════════════════════════════╣");
        Console.WriteLine($"║  TOTAL: RD$ {calcularTotal(),-38:F2}║");
        Console.WriteLine("╚══════════════════════════════════════════════════╝");
    }

    protected virtual string getTipoHamburguesa() => "Hamburguesa Clásica";
}

// ============================================================
//  CLASE HIJA 1: HamburguesaSaludable
//  Pan integral, hasta 6 ingredientes adicionales
//  Los 2 ingredientes base propios de esta clase
// ============================================================
class HamburguesaSaludable : Hamburguesa
{
    // Constructor agrega 2 ingredientes propios automáticamente
    public HamburguesaSaludable(double precioBase)
        : base("Pan Integral", "Carne de Pollo a la Plancha", precioBase)
    {
        maxIngredientes = 6;

        // 2 ingredientes propios de la hamburguesa saludable
        ingredientes.Add(new Ingrediente("Aguacate",       35.00));
        ingredientes.Add(new Ingrediente("Espinaca Fresca", 20.00));
    }

    public override bool agregarIngrediente(Ingrediente ingrediente)
    {
        if (ingredientes.Count >= maxIngredientes)
        {
            Console.WriteLine($"  ⚠️  Máximo de {maxIngredientes} ingredientes alcanzado.");
            return false;
        }
        ingredientes.Add(ingrediente);
        return true;
    }

    public override double calcularTotal()
    {
        double total = getPrecioBase();
        foreach (var ing in ingredientes)
            total += ing.getPrecio();
        return total;
    }

    protected override string getTipoHamburguesa() => "Hamburguesa Saludable";
}

// ============================================================
//  CLASE HIJA 2: HamburguesaPremium
//  Viene con papitas y bebida incluidas
//  NO permite ingredientes adicionales
// ============================================================
class HamburguesaPremium : Hamburguesa
{
    private bool bloqueada;

    // Constructor agrega papitas y bebida automáticamente
    public HamburguesaPremium(double precioBase)
        : base("Pan Brioche", "Doble Carne Angus", precioBase)
    {
        bloqueada = false;

        // Adicionales automáticos propios del Premium
        ingredientes.Add(new Ingrediente("Papitas Fritas",  75.00));
        ingredientes.Add(new Ingrediente("Bebida Grande",   60.00));

        // Bloquear después de agregar los adicionales propios
        bloqueada = true;
    }

    // Previene agregar más ingredientes tras la creación
    public override bool agregarIngrediente(Ingrediente ingrediente)
    {
        if (bloqueada)
        {
            Console.WriteLine("  ⛔ La Hamburguesa Premium no permite ingredientes adicionales.");
            return false;
        }
        return base.agregarIngrediente(ingrediente);
    }

    public override double calcularTotal()
    {
        double total = getPrecioBase();
        foreach (var ing in ingredientes)
            total += ing.getPrecio();
        return total;
    }

    protected override string getTipoHamburguesa() => "Hamburguesa Premium";
}

// ============================================================
//  PROGRAMA PRINCIPAL
// ============================================================
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔══════════════════════════════════════════════════╗");
        Console.WriteLine("║     🍔 SISTEMA DE VENTAS - CHIMI MIBARRIGA      ║");
        Console.WriteLine("║           Sr. Billy Navaja                       ║");
        Console.WriteLine("╚══════════════════════════════════════════════════╝\n");

        // ── HAMBURGUESA CLÁSICA ──
        Console.WriteLine("━━━ ORDEN 1: Hamburguesa Clásica ━━━\n");
        Hamburguesa clasica = new Hamburguesa("Pan Suave", "Carne de Res", 150.00);
        clasica.agregarIngrediente(new Ingrediente("Lechuga",    15.00));
        clasica.agregarIngrediente(new Ingrediente("Tomate",     15.00));
        clasica.agregarIngrediente(new Ingrediente("Bacon",      40.00));
        clasica.agregarIngrediente(new Ingrediente("Pepinillo",  10.00));
        clasica.agregarIngrediente(new Ingrediente("Cebolla",    10.00)); // excede el máximo
        clasica.mostrarDetalle();

        Console.WriteLine();

        // ── HAMBURGUESA SALUDABLE ──
        Console.WriteLine("━━━ ORDEN 2: Hamburguesa Saludable ━━━\n");
        HamburguesaSaludable saludable = new HamburguesaSaludable(180.00);
        saludable.agregarIngrediente(new Ingrediente("Tomate Cherry",  20.00));
        saludable.agregarIngrediente(new Ingrediente("Pepino",         15.00));
        saludable.agregarIngrediente(new Ingrediente("Zanahoria",      15.00));
        saludable.agregarIngrediente(new Ingrediente("Queso Bajo Grasa", 30.00));
        saludable.agregarIngrediente(new Ingrediente("Extra Aguacate", 35.00)); // excede el máximo
        saludable.mostrarDetalle();

        Console.WriteLine();

        // ── HAMBURGUESA PREMIUM ──
        Console.WriteLine("━━━ ORDEN 3: Hamburguesa Premium ━━━\n");
        HamburguesaPremium premium = new HamburguesaPremium(320.00);
        premium.agregarIngrediente(new Ingrediente("Jalapeño", 20.00)); // bloqueado
        premium.mostrarDetalle();

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
