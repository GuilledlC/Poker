using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
	class Card
	{
		private readonly Suits _suit;
		private readonly int _rank;

		public Suits Suit
		{
			get { return _suit; }
			set { }// _suit = (Suits)((int)value % 4); }
		}
		//I like .NET 5.0 >:(
		public int Rank
		{
			get { return _rank; }
			set { }// _rank = ((value - 1) % 13) + 1; }
		}

		public Card(Suits suit, int rank)
		{
			_suit = (Suits)((int)suit % 4);
			_rank = ((rank - 2) % 13) + 2;
		}

		public string Show()
		{
			string card = "";

			switch (Rank)
			{
				case 14:
					card = "Ace";
					break;
				case 2:
					card = "2nd";
					break;
				case 3:
					card = "3rd";
					break;
				case 11:
					card = "Jack";
					break;
				case 12:
					card = "Queen";
					break;
				case 13:
					card = "King";
					break;
				default:
					card += Rank.ToString() + "th";
					break;
			}

			card += " of " + Suit.ToString();
			return card;
		}

		public void PrintSuit()
		{
			Console.Write(" ");
			Console.BackgroundColor = ConsoleColor.White;
			if (Suit == Suits.Diamonds || Suit == Suits.Hearts)
				Console.ForegroundColor = ConsoleColor.Red;
			else
				Console.ForegroundColor = ConsoleColor.Black;

			if(Suit == Suits.Clubs)
				Console.Write("♣  ");
			else if (Suit == Suits.Diamonds)
				Console.Write("♦  ");
			else if (Suit == Suits.Hearts)
				Console.Write("♥  ");
			else if (Suit == Suits.Spades)
				Console.Write("♠  ");
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
		}

		public void PrintRank()
		{
			Console.Write(" ");
			Console.BackgroundColor = ConsoleColor.White;
			if (Suit == Suits.Diamonds || Suit == Suits.Hearts)
				Console.ForegroundColor = ConsoleColor.Red;
			else
				Console.ForegroundColor = ConsoleColor.Black;

			if (Rank == 14)
				Console.Write(" A ");
			else if (Rank == 10)
				Console.Write("10 ");
			else if (Rank == 11)
				Console.Write(" J ");
			else if (Rank == 12)
				Console.Write(" Q ");
			else if (Rank == 13)
				Console.Write(" K ");
			else
				Console.Write(" " + Rank + " ");
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void PrintBack()
		{
			Console.Write(" ");
			Console.BackgroundColor = ConsoleColor.Cyan;
			Console.Write("   ");
			Console.BackgroundColor = ConsoleColor.Black;
		}
	}
}
