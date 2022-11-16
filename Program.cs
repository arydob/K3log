using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K3log
{
	static class Program
	{

		
		static void Main(String[] args)
		{
			// init string collection
			//StringsCollector strc = new StringsCollector();
			//strc.startCollector();

			///DLL injection
			InDinLib dllinj = new InDinLib();
			dllinj.startInj(Int32.Parse(args[0]), args[1]);
			
		}
	}
}
