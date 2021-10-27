using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
	class TexasHoldEm
	{
		private Deck mainDeck;
		private List<Player> players;
		private List<Card> community;

		public TexasHoldEm()
		{
			mainDeck = new Deck();
			players = new List<Player>();
			community = new List<Card>();
		}

		public void GameLoop()
		{
			mainDeck.Shuffle();
			Flop();
			Show();
			Console.ReadLine();
			Turn();
			Show();
			Console.ReadLine();
			River();
			Show();
			Console.ReadLine();
		}

		private void Show()
		{
			foreach (Card c in community)
				c.PrintSuit();
			Console.WriteLine();
			foreach (Card c in community)
				c.PrintRank();
			Console.WriteLine();
		}

		public void Deal()
		{
			foreach(Player p in players)
				p.GetCards(mainDeck.Give(), mainDeck.Give());
		}

		public void Flop()
		{
			if (community.Count == 0)
			{
				community.Add(mainDeck.Give()); //It ain't
				community.Add(mainDeck.Give()); //pretty but
				community.Add(mainDeck.Give()); //it works
			}
		}

		public void Turn()
		{
			if(community.Count == 3)
				community.Add(mainDeck.Give());
		}

		public void River()
		{
			if(community.Count == 4)
				community.Add(mainDeck.Give());
		}
	}
}
