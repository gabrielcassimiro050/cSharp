using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


Main();

async Task Main()
{
    const string host = "127.0.0.1";
    const int porta = 9000;
    using var cliente = new TcpClient();
    await cliente.ConnectAsync(host, porta);
    using var stream = cliente.GetStream();

    var dados = Encoding.UTF8.GetBytes("Olá servidor!");
    await stream.WriteAsync(dados, 0, dados.Length);

    var buffer = new byte[1024];
    int lidos = await stream.ReadAsync(buffer, 0, buffer.Length);
    Console.WriteLine($"Resposta: {Encoding.UTF8.GetString(buffer, 0, lidos)}");
}

