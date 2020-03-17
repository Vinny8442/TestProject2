using System.Collections.Generic;
using UnityEngine;

namespace test.project.Services
{
	public class PrefabsPoolingService : IPrefabsPoolingService
	{
		// Dictionary<string, PrefabsPool> _pools = new Dictionary<string, PrefabsPool>();
		private IPrefabLoader _prefabLoader;
		private Transform _stashTransform;

		public void Inject(IContainer container)
		{
			_prefabLoader = container.Get<IPrefabLoader>();
		}

		public void Prepare()
		{
		}

		public void Start()
		{
			var stash = new GameObject("PrefabsStash");
			stash.SetActive(false);
			GameObject.DontDestroyOnLoad(stash);
			_stashTransform = stash.transform;
		}

		public void Reset()
		{
		}

		public void Clear()
		{
			GameObject.Destroy(_stashTransform.gameObject);
		}

		public PrefabsPool GetPool(string prefabLinkage)
		{
			var result = new PrefabsPool(_prefabLoader, prefabLinkage, _stashTransform);
			return result;
		}
	}
}