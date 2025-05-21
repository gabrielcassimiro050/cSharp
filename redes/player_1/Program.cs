using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

Dictionary<string, char> map = new Dictionary<string, char>();
const string letters = "abcdefghij";

Main();

void Main()
{
    System.Console.WriteLine("1- Posicionamento Aleatório");
    System.Console.WriteLine("2- Posicionamento Manual");
    int op = Convert.ToInt32(Console.ReadLine());
    if (op == 1) posicionamentoAleatorio();
    else posicionamentoManual();

    displayMap();
    StartServer(8000);
}


async Task StartServer(int port)
{
    var listener = new TcpListener(IPAddress.Any, port);
    listener.Start();
    Console.WriteLine($"Aguardando Player2 na porta {port}...");
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    Console.WriteLine("Player2 conectado!");
}

void displayMap()
{
    System.Console.WriteLine("      A   B   C   D   E   F   G   H   I   J  ");
    System.Console.WriteLine("    -----------------------------------------");
    for (int x = 0; x < 10; ++x)
    {
        if(x+1<10) System.Console.Write((x+1)+"   |");
        else System.Console.Write((x+1)+"  |");
        for (int y = 0; y < 10; ++y)
        {
            string key = letters[x] + "" + (y + 1);
            if (map.ContainsKey(key)) System.Console.Write(" " + map[key] + " |");
            else System.Console.Write("   |");
        }
        System.Console.WriteLine(); System.Console.WriteLine("    -----------------------------------------");
    }
}

void setMap()
{
    for (int x = 0; x < 10; ++x)
    {
        for (int y = 0; y < 10; ++y)
        {
            string key = letters[x] + "" + (y + 1);
            map.Add(key, ' ');
        }
    }
}

void posicionamentoAleatorio()
{
    Random rand = new Random();

    for (int i = 0; i < 10; ++i)
    {
        string key = letters[rand.Next(10)] + "" + rand.Next(10);

        do
        {
            key = letters[rand.Next(10)] + "" + rand.Next(10);
        } while (map.ContainsKey(key));

        map[key] = '*';
    }
}   

void posicionamentoManual()
{

    for (int i = 0; i < 10; ++i)
    {
        string coord = Console.ReadLine();
        map[coord] = '*';
    }

}

