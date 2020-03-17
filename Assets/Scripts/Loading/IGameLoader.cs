using System;

namespace test.project
{
	public interface IGameLoader
	{
		void LoadScene(string sceneLinkage, Action callback);
		// void LoadScene<T>(string sceneLinkage) where T:class, ISceneInitable;
	}
}