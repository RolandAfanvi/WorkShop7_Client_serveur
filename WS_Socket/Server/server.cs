using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    private static Socket SeConnecter()
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 11000;
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
        serverSocket.Bind(ipEndPoint);
        serverSocket.Listen(10);
        return serverSocket;
    }

    private static Socket AccepterConnexion(Socket serverSocket)
    {
        Socket clientSocket = serverSocket.Accept();
        IPEndPoint clientEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
        Console.WriteLine($"Client connecté: {clientEndPoint.Address}:{clientEndPoint.Port}");
        return clientSocket;
    }

    private static void EcouterReseau(Socket clientSocket)
    {
        byte[] buffer = new byte[1024];
        int bytesReceived;
        while ((bytesReceived = clientSocket.Receive(buffer)) > 0)
        {
            string message = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
            Console.WriteLine($"Message reçu du client: {message}");
            byte[] response = Encoding.ASCII.GetBytes("Message reçu");
            clientSocket.Send(response);
        }
    }

    private static void Deconnecter(Socket socket)
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }

    static void Main(string[] args)
    {
        Socket serverSocket = SeConnecter();
        Socket clientSocket = AccepterConnexion(serverSocket);
        EcouterReseau(clientSocket);
        Deconnecter(clientSocket);
        Deconnecter(serverSocket);
    }
}