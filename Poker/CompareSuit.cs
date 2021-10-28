using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Poker
{
	class CompareSuit : Comparer<Card>
	{
		public override int Compare([AllowNull] Card x, [AllowNull] Card y)
		{
			if (x == null || y == null)
				return Comparer<Card>.Default.Compare(x, y);
			else
				return Comparer<int>.Default.Compare((int)x.Suit, (int)y.Suit);
		}
	}
}
