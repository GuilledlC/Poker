using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
	class Player
	{
		private string _name;
		private uint _chips;
		private List<Card> _holeCards = new List<Card>();

		public Player(string name, uint chips)
		{
			_name = name;
			_chips = chips;
		}

		public void GetCards(Card holeCard1, Card holeCard2)
		{
			if (holeCard1 == null || holeCard2 == null)
				throw new ArgumentNullException();
			_holeCards[0] = holeCard1;
			_holeCards[1] = holeCard2;
		}

		public List<Card> ShowCards()
		{
			List<Card> shownCards = _holeCards;
			_holeCards.Clear();
			return shownCards;
		}
	}
}
