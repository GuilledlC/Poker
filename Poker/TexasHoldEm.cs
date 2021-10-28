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
			UI();
			Console.ReadLine();
			Deal();
			UI();
			Console.ReadLine();
			Flop();
			UI();
			Console.ReadLine();
			Turn();
			UI();
			Console.ReadLine();
			River();
			UI();
			Console.ReadLine();
			WinningHand();
			UI();
			Console.ReadLine();
			CompareRank compRank = new CompareRank();
			community.Sort(compRank);
			UI();
			Console.ReadLine();
			CompareSuit compSuit = new CompareSuit();
			community.Sort(compSuit);
			UI();
			Console.ReadLine();
		}

		public void AddPlayer(Player player)
		{
			players.Add(player);
		}

		private void UI()
		{
			Console.Clear();
			Console.WriteLine("\n");
			ShowCommunity();
			Console.WriteLine("\n");
			ShowPlayers();
		}

		private void ShowPlayers()
		{
			List<Card> hand = new List<Card>();
			foreach (Player p in players)
			{
				Console.Write(p.Name);
				hand = p.ShowCards();
				foreach (Card c in hand)
				{
					if (c == null)
						Card.PrintBack();
					else
						c.PrintSuit();
				}
					
				Console.Write("  ");
			}
			Console.WriteLine();
			foreach (Player p in players)
			{
				Console.ForegroundColor = ConsoleColor.Black;
				Console.Write(p.Name);
				Console.ForegroundColor = ConsoleColor.White;
				hand = p.ShowCards();
				foreach (Card c in hand)
				{
					if (c == null)
						Card.PrintBack();
					else
						c.PrintRank();
				}
				Console.Write("  ");
			}
		}

		private void ShowCommunity()
		{
			foreach (Card c in community)
				c.PrintSuit();
			Console.WriteLine();
			foreach (Card c in community)
				c.PrintRank();
		}

		private void Deal()
		{
			foreach(Player p in players)
				p.GetCards(mainDeck.Give(), mainDeck.Give());
		}

		private void Flop()
		{
			if (community.Count == 0)
			{
				community.Add(mainDeck.Give()); //It ain't
				community.Add(mainDeck.Give()); //pretty but
				community.Add(mainDeck.Give()); //it works
			}
		}

		private void Turn()
		{
			if(community.Count == 3)
				community.Add(mainDeck.Give());
		}

		private void River()
		{
			if(community.Count == 4)
				community.Add(mainDeck.Give());
		}

		private Player WinningHand()
		{
			return null;
		}

		private static List<Card> Straight(ref List<Card> hand)
		{
			List<Card> checking = new List<Card>(hand);
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Rank

			List<Card> straightHand = new List<Card>();

			for (int i = 2; i >= 0; i--)
			{
				for(int j = 3; j >= 0; j--)
				{
					if(compRank.Compare(checking[i + j + 1], checking[i + j]) != 1)
					{
						straightHand.Clear();
						break;
					}

					straightHand.Add(checking[i + j + 1]);
					if (j == 0)
					{
						straightHand.Add(checking[i + j]);
						return straightHand;
					}
												
				}
			}
			return null;
		}

		private List<Card> FourOfAKind(List<Card> hand)
		{
			return null; //Finish later
		}

		private static List<Card> Flush(ref List<Card> hand)
		{
			List<Card> checking = new List<Card>(hand);
			CompareSuit compSuit = new CompareSuit();
			checking.Sort(compSuit);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Suit

			List<Card> flushHand = new List<Card>();

			for (int i = 2; i >= 0; i--)
			{
				for (int j = 3; j >= 0; j--)
				{
					if (compSuit.Compare(checking[i + j + 1], checking[i + j]) != 0)
					{
						flushHand.Clear();
						break;
					}

					flushHand.Add(checking[i + j + 1]);
					if (j == 0)
					{
						flushHand.Add(checking[i + j]);
						return flushHand;
					}

				}
			}
			return null;
		}

		private static List<Card> StraightFlush(ref List<Card> hand)
		{
			List<Card> checking = new List<Card>(hand);
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			CompareSuit compSuit = new CompareSuit();
			checking.Sort(compSuit);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to  Rank and Suit

			List<Card> straightFlushHand = new List<Card>();

			for (int i = 2; i >= 0; i--)
			{
				for (int j = 3; j >= 0; j--)
				{
					if (compRank.Compare(checking[i + j + 1], checking[i + j]) != 1
						&& compSuit.Compare(checking[i + j + 1], checking[i + j]) != 0)
					{
						straightFlushHand.Clear();
						break;
					}

					straightFlushHand.Add(checking[i + j + 1]);
					if (j == 0)
					{
						straightFlushHand.Add(checking[i + j]);
						return straightFlushHand;
					}

				}
			}
			return null;
		}

		private static List<Card> RoyalFlush(ref List<Card> hand)
		{
			List<Card> royalFlush = StraightFlush(ref hand);

			if (royalFlush[4].Rank == 1)
				return royalFlush;
			else
				return null;
		}
	}
}
