using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;

namespace Extraction
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Host: ");
            string host = Console.ReadLine();
            while (host != "exit")
            {
                Console.Clear();
                IPAddress ip_address = ResolveHost(host);
                execWMIC(ip_address, "cpu get name, systemname");
                execWMIC(ip_address, "memorychip get banklabel, capacity, speed");
                Console.Write("Host: ");
                host = Console.ReadLine();
            }
        }

        private static IPAddress ResolveHost(string host)
        {
            IPHostEntry hostEntry;
            IPAddress ip = IPAddress.None;
            try
            {
                hostEntry = Dns.GetHostEntry(host);

                if (hostEntry.AddressList.Length > 0)
                {
                    ip = hostEntry.AddressList[0];
                    return ip;
                }
                throw new System.Net.Sockets.SocketException();
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Console.WriteLine("No pudo resolverse el nombre del host...");
            }
            return ip;
        }

        private static void execWMIC(IPAddress ip, string arguments)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "wmic",
                    Arguments = "/NODE:" + ip.ToString() + " " + arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }
        }
    }
}
