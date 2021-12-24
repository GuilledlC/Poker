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
			Player p = WinningHand();
			UI();
			Console.WriteLine(p.Name);
			Console.ReadLine();
		}

		public void AddPlayer(Player player)
		{
			players.Add(player);
		}

		private void UI()
		{
			//Console.Clear();
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
				p.TakeCards(mainDeck.Give(), mainDeck.Give());
		}

		private void Flop()
		{
			if (community.Count == 0) //Should throw Exception
			{
				community.Add(mainDeck.Give()); //It ain't
				community.Add(mainDeck.Give()); //pretty but
				community.Add(mainDeck.Give()); //it works
			}
		}

		private void Turn()
		{
			if(community.Count == 3) //Should throw Exception
				community.Add(mainDeck.Give());
		}

		private void River()
		{
			if(community.Count == 4) //Should throw Exception
				community.Add(mainDeck.Give()); //Should burn the first card
		}

		private Player WinningHand()
		{
			HandValues royalFlush = RoyalFlush;
			HandValues straightFlush = StraightFlush;
			HandValues fourOfAKind = FourOfAKind;
			HandValues fullHouse = FullHouse;
			HandValues flush = Flush;
			HandValues straight = Straight;
			HandValues threeOfAKind = ThreeOfAKind;
			HandValues twoPairs = TwoPairs;
			HandValues pair = Pair;
			HandValues highCard = HighCard;
			HandValues[] handValuesList = { royalFlush, straightFlush, fourOfAKind, fullHouse, flush, straight, threeOfAKind, twoPairs, pair, highCard };

			List<Card> checking = new List<Card>(community);

			List<List<Card>> winningHands = new List<List<Card>>();
			List<Player> winners = new List<Player>();

			foreach(HandValues hv in handValuesList)
			{
				foreach(Player p in players)
				{
					List<Card> aux;
					checking.AddRange(p.ShowCards());
					if (hv == highCard)
					{
						List<Card> aux2 = p.ShowCards();
						aux = hv(ref aux2);
					}
					else
						aux = hv(ref checking);

					if(aux != null)
					{
						winners.Add(p);
						winningHands.Add(aux);
						Console.WriteLine(hv.Method);
					}
					checking.RemoveRange(checking.Count - 2, 2);
				}
				if (winners.Count != 0)
					break;
			}

			Console.WriteLine("\nWinning hand everybody!!");
			foreach (Card c in winningHands[0])
				c.PrintSuit();
			Console.WriteLine();
			foreach (Card c in winningHands[0])
				c.PrintRank();
			Console.WriteLine("-----------\n");
			return winners[0];
		}

		private delegate List<Card> HandValues(ref List<Card> hand);

		private static List<Card> HighCard(ref List<Card> hand)
		{
			List<Card> checking = new List<Card>(hand);
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Rank

			List<Card> highCard = new List<Card>();
			highCard.Add(checking[checking.Count - 1]);
			return highCard;
		}

		private static List<Card> Pair(ref List<Card> hand)
		{
			List<Card> checking = new List<Card>(hand);
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Rank

			List<Card> pairHand = new List<Card>();

			for (int i = checking.Count - 2; i >= 0; i--)
			{
				if (compRank.Compare(checking[i + 1], checking[i]) != 0)
				{
					pairHand.Clear();
					continue;
				}

				pairHand.Add(checking[i + 1]);
				pairHand.Add(checking[i]);
				return pairHand;	
			}
			return null;
		}

		private static List<Card> TwoPairs(ref List<Card> hand)
		{
			List<Card> checking = new List<Card>(hand);
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Rank

			List<Card> pair1 = Pair(ref checking);
			if (pair1 != null)
			{
				for (int i = checking.Count - 1; i >= 0; i--)
				{
					if (pair1.Contains(checking[i]))
						checking.RemoveAt(i);
				}
				List<Card> pair2 = Pair(ref checking);

				if (pair2 != null)
				{
					pair1.AddRange(pair2);
					return pair1;
				}
			}
			return null;
		}

		private static List<Card> ThreeOfAKind(ref List<Card> hand)
		{
			List<Card> checking = new List<Card>(hand);
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Rank

			List<Card> threeHand = new List<Card>();

			for (int i = 4; i >= 0; i--)
			{
				for (int j = 1; j >= 0; j--)
				{
					if (compRank.Compare(checking[i + j + 1], checking[i + j]) != 0)
					{
						threeHand.Clear();
						break;
					}

					threeHand.Add(checking[i + j + 1]);
					if (j == 0)
					{
						threeHand.Add(checking[i + j]);
						return threeHand;
					}

				}
			}
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

		private static List<Card> FullHouse(ref List<Card> hand)
		{
			List<Card> checking = new List<Card>(hand);
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Rank

			List<Card> threeHand = ThreeOfAKind(ref checking);
			if(threeHand != null)
			{
				for(int i = checking.Count - 1; i >= 0; i--)
				{
					if (threeHand.Contains(checking[i]))
						checking.RemoveAt(i);
				}
				List<Card> pairHand = Pair(ref checking);

				if (pairHand != null)
				{
					threeHand.AddRange(pairHand);
					return threeHand;
				}
			}
			return null;
		}

		private static List<Card> FourOfAKind(ref List<Card> hand)
		{
			List<Card> checking = new List<Card>(hand);
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Rank

			List<Card> fourHand = new List<Card>();

			for (int i = 3; i >= 0; i--)
			{
				for (int j = 2; j >= 0; j--)
				{
					if (compRank.Compare(checking[i + j + 1], checking[i + j]) != 0)
					{
						fourHand.Clear();
						break;
					}

					fourHand.Add(checking[i + j + 1]);
					if (j == 0)
					{
						fourHand.Add(checking[i + j]);
						return fourHand;
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
						|| compSuit.Compare(checking[i + j + 1], checking[i + j]) != 0)
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

			if (royalFlush != null && royalFlush[4].Rank == 1)
				return royalFlush;
			else
				return null;
		}
	}
}
