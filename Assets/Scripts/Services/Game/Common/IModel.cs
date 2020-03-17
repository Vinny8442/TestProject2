using UnityEngine;

namespace test.project.Services
{
	public interface IModel
	{
		string PrefabLinkage { get; }
	}

	public interface IModelController<TModel> where TModel : IModel
	{
		void Init(TModel model);
		TModel Model { get; }
		GameObject gameObject { get; }
	}
}