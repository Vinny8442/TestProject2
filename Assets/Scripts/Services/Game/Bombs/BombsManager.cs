using System;
using System.Collections;
using System.Collections.Generic;
using test.project.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace test.project.Services
{
	public class BombsManager : IBombsManager, BombModel.IBombEventHandler
	{
		private IConfigStorage _configStorage;
		private IUnityEventManager _eventManager;
		private IPrefabsPoolingService _prefabsPoolingService;
		private BombsFactory _controllerFactor;
		private int[] _bombIds;
		private Dictionary<float, int> _bombsRarity;
		private int _maxCount;
		private float _spawnTimeout;
		private Rect _spawnArea;
		private Coroutine _spawnCoroutine;
		private Dictionary<BombModel, BombController> _bombs = new Dictionary<BombModel, BombController>();

		private Transform _container;

		public void Inject(IContainer container)
		{
			_configStorage = container.Get<IConfigStorage>();
			_eventManager = container.Get<IUnityEventManager>();
			_prefabsPoolingService = container.Get<IPrefabsPoolingService>();
			// container.Get<ICharacterManager>().HandleExplosion
		}

		public void Prepare()
		{
			_controllerFactor = new BombsFactory(_prefabsPoolingService);
			MainSceneConfig sceneConfig = _configStorage.Get<MainSceneConfig>();

			_bombIds = sceneConfig.BombIds;
			float totalRarity = 0;
			foreach (int bombId in _bombIds)
			{
				totalRarity += _configStorage.Get<BombConfig>(bombId).Rarity;
			}

			float currentRarity = 0;
			_bombsRarity = new Dictionary<float, int>(_bombIds.Length);
			foreach (int bombId in _bombIds)
			{
				_bombsRarity[currentRarity] = bombId;
				currentRarity += _configStorage.Get<BombConfig>(bombId).Rarity / totalRarity;
			}

			_maxCount = sceneConfig.MaxBombs;
			_spawnTimeout = sceneConfig.BombSpawnTimeout;
			_spawnArea = sceneConfig.BombSpawnArea;
		}

		public void Start()
		{
			_spawnCoroutine = _eventManager.StartCoroutine(SpawnCoroutine());
			_container = (new GameObject("BombsContainer")).transform;
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			if (_spawnCoroutine != null)
			{
				_eventManager.StopCoroutine(_spawnCoroutine);
				_spawnCoroutine = null;
			}

			foreach (var pair in _bombs)
			{
				_controllerFactor.Release(pair.Value);
			}

			_bombs.Clear();
		}

		private IEnumerator SpawnCoroutine()
		{
			while (true)
			{
				if (_bombs.Count < _maxCount)
				{
					SpawnBomb();
				}

				yield return new WaitForSeconds(_spawnTimeout);
			}
		}

		private int MaxSCount = 0;
		private void SpawnBomb()
		{
			float rarityRatio = Random.Range(0f, 1f);
			float rarityIndex = 0;
			foreach (var pair in _bombsRarity)
			{
				if (pair.Key < rarityRatio && pair.Key > rarityIndex)
				{
					rarityIndex = pair.Key;
				}
			}

			int bombIndex = _bombsRarity[rarityIndex];
			BombConfig bombConfig = _configStorage.Get<BombConfig>(bombIndex);
			Vector3 position = new Vector3(Random.Range(_spawnArea.xMin, _spawnArea.xMax), 45,
				Random.Range(_spawnArea.yMin, _spawnArea.yMax));

			BombModel bomb = new BombModel(bombConfig, position, this);
			BombController bombController = _controllerFactor.Create(bomb);
			bombController.transform.SetParent(_container, true);
			_bombs[bomb] = bombController;

			if (MaxSCount < _bombs.Count) MaxSCount = _bombs.Count;
			Debug.Log($"{_bombs.Count}/{MaxSCount}");
		}


		public void HandleExplosion(BombModel bomb)
		{
			if (!_bombs.ContainsKey(bomb))
			{
				return;
			}

			Collider[] colliders = Physics.OverlapSphere(bomb.Position, bomb.Config.DamageRadius);
			foreach (Collider collider in colliders)
			{
				CharController charController = collider.GetComponent<CharController>();
				if (charController != null && IsDirectImpact(charController.transform.position, bomb.Position))
				{
					charController.Model.ReceiveDamage(bomb.Config.Damage);
				}
			}

			BombController bombController = _bombs[bomb];
			_bombs.Remove(bomb);
			_controllerFactor.Release(bombController);
		}

		private bool IsDirectImpact(Vector3 targetPosition, Vector3 bombPosition)
		{
			Ray ray = new Ray(bombPosition, targetPosition - bombPosition);
			if (Physics.Raycast(ray, out var hit, (targetPosition - bombPosition).magnitude,
				LayerMask.GetMask("Geometry")))
			{
				return false;
			}

			return true;
		}
	}
}