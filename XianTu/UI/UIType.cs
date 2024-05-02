namespace XianTu.UI
{
	internal class UIType
	{
		public UIType(string prefabPath)
		{
			this.PrefabPath = prefabPath;
			Name = prefabPath.Substring(prefabPath.LastIndexOf('/') + 1);
		}

		public string PrefabPath;

		public string Name;
	}
}
