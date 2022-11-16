using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K3log
{
	class pipeServer
	{


        public void StartServer()
        {

            Console.WriteLine("test");


            Task.Factory.StartNew(() =>
            {
                var server = new NamedPipeServerStream("PipesOfPiece");
                server.WaitForConnection();
                StreamReader reader = new StreamReader(server);
                StreamWriter writer = new StreamWriter(server);
                while (true)
                {
                    try
                    {
                        var line = reader.ReadLine();
                        writer.WriteLine("Recieved!");
                        writer.Flush();
                    }catch{}
                }
            });
        }

    }
}
