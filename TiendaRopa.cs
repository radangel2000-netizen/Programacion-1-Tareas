using System;
using System.Collections.Generic;

// ============================================================
//  CLASE BASE ABSTRACTA: Producto
//  Aplica: Abstracción + Encapsulamiento
// ============================================================
abstract class Producto
{
    private string codigo;
    private string nombre;
    private double precio;
    private int    stock;

    public Producto(string codigo, string nombre, double precio, int stock)
    {
        this.codigo = codigo;
        this.nombre = nombre;
        this.precio = precio;
        this.stock  = stock;
    }

    public string getCodigo() => codigo;
    public string getNombre() => nombre;
    public double getPrecio() => precio;
    public int    getStock()  => stock;

    public void reducirStock(int cantidad) => stock -= cantidad;

    public abstract string getCategoria();
    public abstract double calcularDescuento();

    public double getPrecioFinal() => precio - calcularDescuento();

    public virtual void mostrarInfo()
    {
        Console.WriteLine($"  [{getCodigo()}] {getNombre()}");
        Console.WriteLine($"  Categoría  : {getCategoria()}");
        Console.WriteLine($"  Precio     : RD$ {precio:F2}");
        Console.WriteLine($"  Descuento  : RD$ {calcularDescuento():F2}");
        Console.WriteLine($"  Precio final: RD$ {getPrecioFinal():F2}");
        Console.WriteLine($"  Stock      : {stock} unidades");
    }
}

// ============================================================
//  CLASE HIJA: Ropa (Camisas, Pantalones, etc.)
//  Aplica: Herencia + Polimorfismo
// ============================================================
class Ropa : Producto
{
    private string talla;
    private string material;
    private string genero;

    public Ropa(string codigo, string nombre, double precio, int stock,
                string talla, string material, string genero)
        : base(codigo, nombre, precio, stock)
    {
        this.talla    = talla;
        this.material = material;
        this.genero   = genero;
    }

    public string getTalla()    => talla;
    public string getMaterial() => material;
    public string getGenero()   => genero;

    public override string getCategoria()      => "Ropa";
    public override double calcularDescuento() => getPrecio() * 0.10; // 10% descuento

    public override void mostrarInfo()
    {
        base.mostrarInfo();
        Console.WriteLine($"  Talla      : {talla}");
        Console.WriteLine($"  Material   : {material}");
        Console.WriteLine($"  Género     : {genero}");
    }
}

// ============================================================
//  CLASE HIJA: Calzado
//  Aplica: Herencia + Polimorfismo
// ============================================================
class Calzado : Producto
{
    private int    numeroZapato;
    private string tipo;

    public Calzado(string codigo, string nombre, double precio, int stock,
                   int numero, string tipo)
        : base(codigo, nombre, precio, stock)
    {
        this.numeroZapato = numero;
        this.tipo         = tipo;
    }

    public int    getNumero() => numeroZapato;
    public string getTipo()   => tipo;

    public override string getCategoria()      => "Calzado";
    public override double calcularDescuento() => getPrecio() * 0.15; // 15% descuento

    public override void mostrarInfo()
    {
        base.mostrarInfo();
        Console.WriteLine($"  Número     : {numeroZapato}");
        Console.WriteLine($"  Tipo       : {tipo}");
    }
}

// ============================================================
//  CLASE HIJA: Accesorio
//  Aplica: Herencia + Polimorfismo
// ============================================================
class Accesorio : Producto
{
    private string tipo;
    private string color;

    public Accesorio(string codigo, string nombre, double precio, int stock,
                     string tipo, string color)
        : base(codigo, nombre, precio, stock)
    {
        this.tipo  = tipo;
        this.color = color;
    }

    public string getTipo()  => tipo;
    public string getColor() => color;

    public override string getCategoria()      => "Accesorio";
    public override double calcularDescuento() => 0; // Sin descuento

    public override void mostrarInfo()
    {
        base.mostrarInfo();
        Console.WriteLine($"  Tipo       : {tipo}");
        Console.WriteLine($"  Color      : {color}");
    }
}

// ============================================================
//  CLASE: ItemCarrito
//  Representa un producto en el carrito con cantidad
// ============================================================
class ItemCarrito
{
    private Producto producto;
    private int      cantidad;

    public ItemCarrito(Producto producto, int cantidad)
    {
        this.producto = producto;
        this.cantidad = cantidad;
    }

    public Producto getProducto() => producto;
    public int      getCantidad() => cantidad;
    public double   getSubtotal() => producto.getPrecioFinal() * cantidad;

    public override string ToString()
    {
        return $"  {producto.getNombre(),-25} x{cantidad}  " +
               $"RD$ {producto.getPrecioFinal():F2} c/u  " +
               $"= RD$ {getSubtotal():F2}";
    }
}

// ============================================================
//  CLASE: Carrito
//  Maneja los productos seleccionados por el cliente
// ============================================================
class Carrito
{
    private List<ItemCarrito> items;
    private string            nombreCliente;

    public Carrito(string cliente)
    {
        nombreCliente = cliente;
        items = new List<ItemCarrito>();
    }

    public bool agregarProducto(Producto producto, int cantidad)
    {
        if (cantidad > producto.getStock())
        {
            Console.WriteLine($"  ⚠️  Stock insuficiente. Disponible: {producto.getStock()}");
            return false;
        }
        items.Add(new ItemCarrito(producto, cantidad));
        producto.reducirStock(cantidad);
        Console.WriteLine($"  ✅ Agregado: {producto.getNombre()} x{cantidad}");
        return true;
    }

    public double calcularTotal()
    {
        double total = 0;
        foreach (var item in items)
            total += item.getSubtotal();
        return total;
    }

    public double calcularITBIS() => calcularTotal() * 0.18;

    public void mostrarFactura()
    {
        Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║           TIENDA DE ROPA - FASHION STORE            ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════╣");
        Console.WriteLine($"║  Cliente: {nombreCliente,-43}║");
        Console.WriteLine("╠══════════════════════════════════════════════════════╣");
        Console.WriteLine("║  PRODUCTOS:                                          ║");

        foreach (var item in items)
            Console.WriteLine($"║{item,-54}║");

        Console.WriteLine("╠══════════════════════════════════════════════════════╣");
        Console.WriteLine($"║  Subtotal : RD$ {calcularTotal(),36:F2}║");
        Console.WriteLine($"║  ITBIS 18%: RD$ {calcularITBIS(),36:F2}║");
        Console.WriteLine($"║  TOTAL    : RD$ {calcularTotal() + calcularITBIS(),36:F2}║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝");
    }
}

// ============================================================
//  CLASE: TiendaRopa
//  Gestiona el inventario de la tienda
// ============================================================
class TiendaRopa
{
    private string          nombre;
    private List<Producto>  inventario;

    public TiendaRopa(string nombre)
    {
        this.nombre     = nombre;
        this.inventario = new List<Producto>();
    }

    public void agregarProducto(Producto p) => inventario.Add(p);

    public void mostrarInventario()
    {
        Console.WriteLine($"\n╔══════════════════════════════════════════════════════╗");
        Console.WriteLine($"║        INVENTARIO - {nombre,-33}║");
        Console.WriteLine($"╚══════════════════════════════════════════════════════╝");

        // Polimorfismo en acción
        foreach (var p in inventario)
        {
            Console.WriteLine("\n  ─────────────────────────────────────");
            p.mostrarInfo();
        }
    }

    public Producto buscarPorCodigo(string codigo)
    {
        return inventario.Find(p => p.getCodigo() == codigo);
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
        Console.WriteLine("║        SISTEMA DE TIENDA DE ROPA - POO C#           ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");

        // Crear tienda
        TiendaRopa tienda = new TiendaRopa("Fashion Store RD");

        // Agregar productos al inventario (Herencia + Polimorfismo)
        tienda.agregarProducto(new Ropa("R001", "Camisa Polo Ralph Lauren", 2500, 20, "M",   "Algodón",   "Masculino"));
        tienda.agregarProducto(new Ropa("R002", "Vestido Zara Floral",      3800, 15, "S",   "Poliéster", "Femenino"));
        tienda.agregarProducto(new Ropa("R003", "Pantalón Vaquero Levi's",  4200, 10, "32",  "Denim",     "Masculino"));
        tienda.agregarProducto(new Ropa("R004", "Blusa H&M Casual",         1800, 25, "L",   "Lino",      "Femenino"));
        tienda.agregarProducto(new Calzado("C001", "Tenis Nike Air Max",    6500, 12, 42,    "Deportivo"));
        tienda.agregarProducto(new Calzado("C002", "Tacones Guess",         5200,  8, 37,    "Formal"));
        tienda.agregarProducto(new Calzado("C003", "Sandalias Steve Madden",3100, 18, 38,    "Casual"));
        tienda.agregarProducto(new Accesorio("A001", "Bolso Michael Kors",  8900,  5, "Bolso",  "Negro"));
        tienda.agregarProducto(new Accesorio("A002", "Gorra New Era",        950, 30, "Gorra",  "Azul"));
        tienda.agregarProducto(new Accesorio("A003", "Cinturón Guess",      1500, 20, "Cinturón","Café"));

        // Mostrar inventario completo
        tienda.mostrarInventario();

        // Simular compra de un cliente
        Console.WriteLine("\n\n╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║              PROCESO DE COMPRA                      ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");

        Carrito carrito = new Carrito("María González");

        carrito.agregarProducto(tienda.buscarPorCodigo("R002"), 1); // Vestido
        carrito.agregarProducto(tienda.buscarPorCodigo("C002"), 1); // Tacones
        carrito.agregarProducto(tienda.buscarPorCodigo("A001"), 1); // Bolso
        carrito.agregarProducto(tienda.buscarPorCodigo("A002"), 2); // 2 Gorras

        // Mostrar factura
        carrito.mostrarFactura();

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
