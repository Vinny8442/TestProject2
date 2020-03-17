using System;
using UnityEngine;

namespace test.project.Controllers
{
	public class DefaultColorController : MonoBehaviour
	{
		[SerializeField] private MeshRenderer _meshRenderer;
		[SerializeField] private string _shaderMainColorProp = "_BaseColor";
		
		private void Start()
		{
		}

		public void SetColor(Color color)
		{
			MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
			propertyBlock.SetColor(_shaderMainColorProp, color);
			_meshRenderer.SetPropertyBlock(propertyBlock);
		}
	}
}