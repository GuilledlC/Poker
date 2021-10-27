using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
	class Card
	{
		private Suits _suit;
		private int _rank;

		public Suits Suit
		{
			get { return _suit; }
			set { _suit = (Suits)((int)value % 4); }
		}

		public int Rank
		{
			get { return _rank; }
			set { _rank = (value % 13) + 1; }
		}

		public Card(Suits suit, int rank)
		{
			Suit = suit;
			Rank = rank;
		}
	}
}
