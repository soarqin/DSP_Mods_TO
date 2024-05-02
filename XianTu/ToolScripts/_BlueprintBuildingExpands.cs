using System.Collections.Generic;

namespace ToolScripts
{
	public static class _BlueprintBuildingExpands
	{
		static _BlueprintBuildingExpands()
		{
			BeltProtoDict.Add(2001, null);
			BeltProtoDict.Add(2002, null);
			BeltProtoDict.Add(2003, null);
			SoltProtoDict.Add(2011, null);
			SoltProtoDict.Add(2012, null);
			SoltProtoDict.Add(2013, null);
            SoltProtoDict.Add(2014, null);
		}

		public static bool IsBelt(this BlueprintBuilding bb)
		{
			return BeltProtoDict.ContainsKey(bb.itemId);
		}

		public static bool IsSlot(this BlueprintBuilding bb)
		{
			return SoltProtoDict.ContainsKey(bb.itemId);
		}

        private static readonly Dictionary<int, ItemProto> BeltProtoDict = new();

        private static readonly Dictionary<int, ItemProto> SoltProtoDict = new();
	}
}
