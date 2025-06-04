using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

double lastTemperature, currentTemperature = 0;

async Task Main()
{   
    System.Console.Write("Unidade (Celsius, Fahrenheit, Kelvin): ");
    var unidade = Console.ReadLine();
    string op;
    using var client = new HttpClient();
    
    var response = await client.GetStringAsync($"http://localhost:5158/temperatura/{unidade}");
    var temperatura = JsonSerializer.Deserialize<Temperatura>(response);

    var holder = currentTemperature;
    currentTemperature = temperatura.valor;
    lastTemperature = holder;

    System.Console.WriteLine(currentTemperature);
    System.Console.Write("Digite 0 para sair: ");
    op = Console.ReadLine();

    do
    {
        response = await client.GetStringAsync($"http://localhost:5158/temperatura/{unidade}");
        temperatura = JsonSerializer.Deserialize<Temperatura>(response);

        holder = currentTemperature;
        currentTemperature = temperatura.valor;
        lastTemperature = holder;

        System.Console.Write(currentTemperature);
        Console.ForegroundColor = ConsoleColor.Red;
        if (currentTemperature - lastTemperature > 0) System.Console.WriteLine(" - SUBIU");
        Console.ForegroundColor = ConsoleColor.Blue;
        if (currentTemperature - lastTemperature < 0) System.Console.WriteLine(" - DESCEU");
        Console.ForegroundColor = ConsoleColor.White;
        if (currentTemperature - lastTemperature == 0) System.Console.WriteLine(" - SEM ALTERAÇÃO");
        System.Console.Write("Digite 0 para sair: ");
        op = Console.ReadLine();
    } while (op != "0");
}

await Main();

public class Temperatura
{
    public string unidade { get; set; }
	public double valor { get; set; }
}
