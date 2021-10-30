using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
	class Deck
	{
		private List<Card> deck = new List<Card>();

		public Deck()
		{
			deck.Add(new Card(Suits.Clubs, 14));
			deck.Add(new Card(Suits.Clubs, 2));
			deck.Add(new Card(Suits.Clubs, 3));
			deck.Add(new Card(Suits.Clubs, 4));
			deck.Add(new Card(Suits.Clubs, 5));
			deck.Add(new Card(Suits.Clubs, 6));
			deck.Add(new Card(Suits.Clubs, 7));
			deck.Add(new Card(Suits.Clubs, 8));
			deck.Add(new Card(Suits.Clubs, 9));
			deck.Add(new Card(Suits.Clubs, 10));
			deck.Add(new Card(Suits.Clubs, 11));
			deck.Add(new Card(Suits.Clubs, 12));
			deck.Add(new Card(Suits.Clubs, 13));
			deck.Add(new Card(Suits.Diamonds, 14));
			deck.Add(new Card(Suits.Diamonds, 2));
			deck.Add(new Card(Suits.Diamonds, 3));
			deck.Add(new Card(Suits.Diamonds, 4));
			deck.Add(new Card(Suits.Diamonds, 5));
			deck.Add(new Card(Suits.Diamonds, 6));
			deck.Add(new Card(Suits.Diamonds, 7));
			deck.Add(new Card(Suits.Diamonds, 8));
			deck.Add(new Card(Suits.Diamonds, 9));
			deck.Add(new Card(Suits.Diamonds, 10));
			deck.Add(new Card(Suits.Diamonds, 11));
			deck.Add(new Card(Suits.Diamonds, 12));
			deck.Add(new Card(Suits.Diamonds, 13));
			deck.Add(new Card(Suits.Hearts, 14));
			deck.Add(new Card(Suits.Hearts, 2));
			deck.Add(new Card(Suits.Hearts, 3));
			deck.Add(new Card(Suits.Hearts, 4));
			deck.Add(new Card(Suits.Hearts, 5));
			deck.Add(new Card(Suits.Hearts, 6));
			deck.Add(new Card(Suits.Hearts, 7));
			deck.Add(new Card(Suits.Hearts, 8));
			deck.Add(new Card(Suits.Hearts, 9));
			deck.Add(new Card(Suits.Hearts, 10));
			deck.Add(new Card(Suits.Hearts, 11));
			deck.Add(new Card(Suits.Hearts, 12));
			deck.Add(new Card(Suits.Hearts, 13));
			deck.Add(new Card(Suits.Spades, 14));
			deck.Add(new Card(Suits.Spades, 2));
			deck.Add(new Card(Suits.Spades, 3));
			deck.Add(new Card(Suits.Spades, 4));
			deck.Add(new Card(Suits.Spades, 5));
			deck.Add(new Card(Suits.Spades, 6));
			deck.Add(new Card(Suits.Spades, 7));
			deck.Add(new Card(Suits.Spades, 8));
			deck.Add(new Card(Suits.Spades, 9));
			deck.Add(new Card(Suits.Spades, 10));
			deck.Add(new Card(Suits.Spades, 11));
			deck.Add(new Card(Suits.Spades, 12));
			deck.Add(new Card(Suits.Spades, 13));
		}

		public void Shuffle()
		{
			long seed = DateTime.Now.Ticks;
			Random rng = new Random((int)seed);
			int n = deck.Count;
			while(n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				Card current = deck[k];
				deck[k] = deck[n];
				deck[n] = current;
			}
		} //Using the Fisher-Yates Shuffle

		public void Print()
		{
			foreach(Card c in deck)
				Console.Write(c.Show() + ", ");
			Console.WriteLine();
		}

		public Card Give()
		{
			Card given = deck[0];
			deck.Remove(given);
			return given;
		}

		public void Take(Card taken)
		{
			deck.Add(taken);
		}
	}
}
