using System.Collections.Generic;
using UnityEngine;

namespace test.project.Services
{
	public class PrefabsPool
	{
		private Queue<GameObject> _storage = new Queue<GameObject>();
		private IPrefabLoader _prefabLoader;
		private string _linkage;
		private Transform _stash;

		public PrefabsPool(IPrefabLoader prefabLoader, string linkage, Transform stash)
		{
			_stash = stash;
			_linkage = linkage;
			_prefabLoader = prefabLoader;
		}
		
		public GameObject Get()
		{
			GameObject result;
			if (_storage.Count == 0)
			{
				result = _prefabLoader.Get(_linkage);
			}

			else result = _storage.Dequeue();
			return result;
		}

		public void Release(GameObject value)
		{
			_storage.Enqueue(value);
			value.transform.SetParent(_stash);
		}
	}
}