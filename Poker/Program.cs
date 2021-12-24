using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poker
{
	static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		/*[STAThread]*/
		static void Main()
		{
			/*Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());*/

			/*Deck first = new Deck();
			first.Print();
			first.Shuffle();
			first.Print();*/

			/*Console.WriteLine("Specifically, red for ♥ and ♦, and black for ♣ and ♠");
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine("♠  ");
			Console.WriteLine("10 ");
			Console.BackgroundColor = ConsoleColor.Black;*/

			TexasHoldEm tx = new TexasHoldEm();
			Player Guille = new Player("Guille", 1000);
			Player Miguel = new Player("Miguel", 1000);
			tx.AddPlayer(Guille);
			tx.AddPlayer(Miguel);
			while (true)
			{
				tx.GameLoop();
				Console.Clear();
			}
		}
	}
}
