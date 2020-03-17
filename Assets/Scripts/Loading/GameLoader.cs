using System;
using System.Collections;
using test.project.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace test.project
{
	public class GameLoader : MonoBehaviour, IGameLoader
	{
		private ConfigStorage _configStorage;
		private IUnityEventManager _unityEventManager;
		private Container _container;
		private MainSceneInitialization _mainSceneInitialization;

		private void Start()
		{
			_container = new Container();

			_configStorage = new ConfigStorage();
			_unityEventManager = new UnityEventManager();
			
			_container.Register<IUnityEventManager>(_unityEventManager);
			_container.Register<IPrefabLoader>(new PrefabLoader());
			_container.Register<IConfigStorage>(_configStorage);
			_container.Register<IPrefabsPoolingService>(new PrefabsPoolingService());

			_container.Inject();
			_container.PrepareAll();
			_container.StartAll();
			
			LoadScene(_configStorage.Get<GameConfig>(0).MainScene, null);
		}

		public void LoadScene(string sceneLinkage, Action callback)
		{
			var gameConfig = _configStorage.Get<GameConfig>(0);
			_unityEventManager.StartCoroutine(SceneLoaderCoroutine(gameConfig.PreloaderScene, sceneLinkage, callback));
		}
		
		private IEnumerator SceneLoaderCoroutine(string preloaderLinkage, string sceneLinkage, Action callback)
		{
			_mainSceneInitialization?.Clear();
			SceneManager.LoadScene(preloaderLinkage);
			yield return SceneManager.LoadSceneAsync(sceneLinkage);
			_mainSceneInitialization = FindObjectOfType<MainSceneInitialization>();
			if (_mainSceneInitialization != null)
			{
				_mainSceneInitialization.Init(_container);
			}
		}
	}
}