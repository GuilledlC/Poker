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

		public string Name
		{
			get { return _name; }
			set { }
		}

		public Player(string name, uint chips)
		{
			_name = name;
			_chips = chips;
			_holeCards.Add(null); //To print the back of the
			_holeCards.Add(null); //card. Subject to change
		}

		public void GetCards(Card holeCard1, Card holeCard2)
		{
			if (holeCard1 == null || holeCard2 == null)
				throw new ArgumentNullException();
			_holeCards.Clear();
			_holeCards.Add(holeCard1);
			_holeCards.Add(holeCard2);
		}

		public List<Card> ShowCards()
		{
			return _holeCards;
		}

		public List<Card> ReturnCards()
		{
			List<Card> aux = new List<Card>(_holeCards);
			_holeCards.Clear();
			return aux;
		}
	}
}
