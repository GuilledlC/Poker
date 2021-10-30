using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Poker
{
	class CompareRank : Comparer<Card>
	{
		public override int Compare([AllowNull] Card x, [AllowNull] Card y)
		{
			if (x == null || y == null)
				return Comparer<Card>.Default.Compare(x, y);
			else if (x.Rank == 2 && y.Rank == 14)
				return 1;
			else if (x.Rank == 14 && y.Rank == 2)
				return -1;
			else
				return x.Rank - y.Rank;
		}
	}
}
