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
		private uint pot;

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
			//Player p = WinningHand();
			Winners(players);
			UI();
			//Console.WriteLine(p.Name);
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

		/*private Player WinningHand()
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
						aux = hv(in aux2);
					}
					else
						aux = hv(in checking);

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
		}*/

		private void Winners(List<Player> winners)
		{
			HandValues BestHand = null;
			if (winners.Count > 1)
				winners = WinningHands(winners, ref BestHand);
			if (winners.Count > 1)
				winners = HighestHands(winners, BestHand);
			if (winners.Count > 1)
				winners = HighestKickers(winners, BestHand, BestHand(winners[0]).Count);

			foreach (Player p in winners)
			{
				Console.WriteLine(p.Name + " receives $" + (uint)(pot / winners.Count));
				p.GiveChips((uint)(pot/winners.Count));

			}
			//give the remainder to the first players after the button in clockwise order
		}

		private List<Player> WinningHands(in List<Player> players, ref HandValues BestHand)
		{ //This loop gets the winning type of hand (Flush, pair, etc...) and returns the winners of this phase
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
			HandValues[] handValues = { royalFlush, straightFlush, fourOfAKind, fullHouse, flush, straight, threeOfAKind, twoPairs, pair, highCard };

			bool exit = false;
			List<Player> winners = new List<Player>();

			foreach (HandValues hv in handValues) //https://en.wikipedia.org/wiki/Texas_hold_%27em#Hand_values
			{
				foreach (Player p in players)
				{
					if (hv(in p) != null)
					{
						exit = true;
						winners.Add(p);
						BestHand = hv;
					}
				}
				if (exit)
					break;
			}

			return winners;
		}

		private List<Player> HighestHands(List<Player> players, HandValues BestHand)
		{ //This loop determines the player(s) with the highest of that type of hand
			int highestHighcard = 0;
			List<Player> winners = new List<Player>();
			foreach (Player p in players)
			{
				if (GetHighcard(BestHand(in p)) > highestHighcard)
				{
					highestHighcard = GetHighcard(BestHand(in p));
					winners.Clear(); winners.Add(p);
				}
				else if (GetHighcard(BestHand(in p)) == highestHighcard)
					winners.Add(p);
			}

			return winners;
		}

		private int GetHighcard(List<Card> cards)
		{ //Returns the Rank of the highest card in the List passed
			CompareRank compRank = new CompareRank();
			cards.Sort(compRank); cards.Reverse();
			return cards[0].Rank;
		}

		private List<Player> HighestKickers(List<Player> players, HandValues BestHand, int bestHandCount)
		{ //Returns the player(s) with the highest kickers
			List<List<Card>> cards = new List<List<Card>>();
			List<Player> winners = new List<Player>();
			CompareRank compRank = new CompareRank();
			//This here makes a list of all the kickers for each player
			//The list index of each List of kickers is the same as the
			//player to whom those kickers belong.
			foreach (Player p in players)
			{
				List<Card> aux = new List<Card>(community);    //All the cards from community
				aux.AddRange(p.ShowCards());                   //+ hole cards that aren't
				aux.RemoveAll((a) => BestHand(in p).Contains(a)); //in the winning hand
				aux.Sort(compRank);                            //sorted from highest
				aux.Reverse();                                 //to lowest
				aux.RemoveRange(5 - bestHandCount, aux.Count - (5 - bestHandCount)); //No more than 5 - cards in the best hand
				cards.Add(aux);
			}

			for (int i = 0; i < 5 - bestHandCount; i++)
			{
				int highestKicker = 0;
				for (int j = 0; j < players.Count; j++)
				{
					if (cards[j][i].Rank > highestKicker)
					{
						highestKicker = cards[j][i].Rank;
						winners.Clear();
						winners.Add(players[j]);
					}
					else if (cards[j][i].Rank == highestKicker)
						winners.Add(players[j]);
				}
			}
			return winners;
		}

		private delegate List<Card> HandValues(in Player player);
		//Using "ref" is more performance friendly (it avoids copying)

		private List<Card> HighCard(in Player player)
		{
			List<Card> checking = new List<Card>(community);
			checking.AddRange(player.ShowCards());
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Rank

			List<Card> highCard = new List<Card>();
			highCard.Add(checking[checking.Count - 1]);
			return highCard;
		}

		private List<Card> Pair(in Player player)
		{
			List<Card> checking = new List<Card>(community);
			checking.AddRange(player.ShowCards());
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

		private List<Card> TwoPairs(in Player player)
		{
			List<Card> checking = new List<Card>(community);
			checking.AddRange(player.ShowCards());
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Rank

			List<Card> pair1 = new List<Card>();
			for (int i = checking.Count - 2; i >= 0; i--)
			{
				if (compRank.Compare(checking[i + 1], checking[i]) != 0)
				{
					pair1.Clear();
					continue;
				}
				pair1.Add(checking[i + 1]);
				pair1.Add(checking[i]);
			}
			if (pair1.Count != 2)
				return null;
			checking.RemoveAll((a) => pair1.Contains(a));

			List<Card> pair2 = new List<Card>();
			for (int i = checking.Count - 2; i >= 0; i--)
			{
				if (compRank.Compare(checking[i + 1], checking[i]) != 0)
				{
					pair2.Clear();
					continue;
				}
				pair2.Add(checking[i + 1]);
				pair2.Add(checking[i]);
			}
			if (pair2.Count != 2)
				return null;
			pair1.AddRange(pair2);

			return pair1;
		}

		private List<Card> ThreeOfAKind(in Player player)
		{
			List<Card> checking = new List<Card>(community);
			checking.AddRange(player.ShowCards());
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

		private List<Card> Straight(in Player player)
		{
			List<Card> checking = new List<Card>(community);
			checking.AddRange(player.ShowCards());
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

		private List<Card> Flush(in Player player)
		{
			List<Card> checking = new List<Card>(community);
			checking.AddRange(player.ShowCards());
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

		private List<Card> FullHouse(in Player player)
		{
			List<Card> checking = new List<Card>(community);
			checking.AddRange(player.ShowCards());
			CompareRank compRank = new CompareRank();
			checking.Sort(compRank);
			//Adds both the community cards and the hand of the
			//current player and sorts them according to Rank

			List<Card> threeHand = new List<Card>();
			bool exit = false;
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
						exit = true;
						break;
					}
				}
				if (exit)
					break;
			}
			if (threeHand.Count != 3)
				return null;

			checking.RemoveAll((a) => threeHand.Contains(a));

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
			}
			if (pairHand.Count != 2)
				return null;
			threeHand.AddRange(pairHand);

			return threeHand;
		}

		private List<Card> FourOfAKind(in Player player)
		{
			List<Card> checking = new List<Card>(community);
			checking.AddRange(player.ShowCards());
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

		private List<Card> StraightFlush(in Player player)
		{
			List<Card> checking = new List<Card>(community);
			checking.AddRange(player.ShowCards());
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

		private List<Card> RoyalFlush(in Player player)
		{
			List<Card> royalFlush = StraightFlush(in player);

			if (royalFlush != null && royalFlush[4].Rank == 1)
				return royalFlush;
			else
				return null;
		}
	}
}
