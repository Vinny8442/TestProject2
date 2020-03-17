using System.Collections.Generic;

namespace test.project.Services
{
	abstract public class ModelViewFactory<TController, TModel>
		where TModel : IModel
		where TController : IModelController<TModel>
	{
		private readonly Dictionary<string, PrefabsPool> _pools = new Dictionary<string, PrefabsPool>();
		private readonly IPrefabsPoolingService _poolingService;

		public ModelViewFactory(IPrefabsPoolingService poolingService)
		{
			_poolingService = poolingService;
		}

		public TController Create(TModel model)
		{
			if (!_pools.TryGetValue(model.PrefabLinkage, out var pool))
			{
				pool = _poolingService.GetPool(model.PrefabLinkage);
				_pools.Add(model.PrefabLinkage, pool);
			}

			var instance = pool.Get();
			TController result = instance.GetComponent<TController>();
			result.Init(model);
			return result;
		}

		public void Release(TController value)
		{
			if (_pools.TryGetValue(value.Model.PrefabLinkage, out var pool))
			{
				pool.Release(value.gameObject);
			}
		}
	}
}