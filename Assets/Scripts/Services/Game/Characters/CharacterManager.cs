using System.Collections;
using System.Collections.Generic;
using test.project.Controllers;
using UnityEngine;

namespace test.project.Services
{
	public class CharacterManager : ICharacterManager, CharacterModel.CharacterEventHandler
	{
		private IConfigStorage _configStorage;
		private IUnityEventManager _eventManager;
		private int[] _characterIds;
		private float _spawnTimeout;
		private int _maxChars;
		private Coroutine _spawnCoroutine;
		private Transform _charControllersStash;

		private Dictionary<CharacterModel, CharController> _aliveChars = new Dictionary<CharacterModel, CharController>();
		private List<CharacterModel> _deadCharacters = new List<CharacterModel>();
		private CharacterFactory _charControllerFactory;
		private IPrefabsPoolingService _prefabsPoolingService;
		private Rect _charSpawnArea;
		private Transform _container;

		public CharacterManager()
		{
		}

		public void Inject(IContainer container)
		{
			_configStorage = container.Get<IConfigStorage>();
			_eventManager = container.Get<IUnityEventManager>();
			_prefabsPoolingService = container.Get<IPrefabsPoolingService>();
		}

		public void Prepare()
		{
			_charControllerFactory = new CharacterFactory(_prefabsPoolingService);
			MainSceneConfig sceneConfig = _configStorage.Get<MainSceneConfig>();
			_characterIds = sceneConfig.CharacterIds;
			_spawnTimeout = sceneConfig.CharsSpawnTimeout;
			_maxChars = sceneConfig.MaxChars;
			_charSpawnArea = sceneConfig.CharSpawnArea;
		}

		public void Start()
		{
			_container = new GameObject("CharactersContainer").transform;
			_spawnCoroutine = _eventManager.StartCoroutine(SpawnCoroutine());
		}

		public void Reset()
		{
			Clear();
		}

		public void Clear()
		{
			if (_spawnCoroutine != null)
			{
				_eventManager.StopCoroutine(_spawnCoroutine);
				_spawnCoroutine = null;
			}

			foreach (var pair in _aliveChars)
			{
				_charControllerFactory.Release(pair.Value);
			}
			_aliveChars.Clear();
		}

		private IEnumerator SpawnCoroutine()
		{
			while (true)
			{
				if (_aliveChars.Count < _maxChars)
				{
					SpawnChar();
				}
				yield return new WaitForSeconds(_spawnTimeout);
			}
		}

		private void SpawnChar()
		{
			int index0 = Random.Range(0, _characterIds.Length);
			CharacterConfig charToSpawnConfig = _configStorage.Get<CharacterConfig>(_characterIds[index0]);
			Vector3 position = new Vector3(Random.Range(_charSpawnArea.xMin, _charSpawnArea.xMax), 0, Random.Range(_charSpawnArea.yMin, _charSpawnArea.yMax));
			CharacterModel character = new CharacterModel(charToSpawnConfig, position, this);
			CharController charController = _charControllerFactory.Create(character);
			charController.transform.SetParent(_container, true);
			
			_aliveChars.Add(character, charController);
		}

		public void HandleIsDead(CharacterModel character)
		{
			CharController charController = _aliveChars[character];
			_aliveChars.Remove(character);
			_charControllerFactory.Release(charController);
		}
	}
}