using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class LoadSceneInstaller : MonoBehaviour
    {
        [Inject]
        private SceneLoader _sceneLoader;
        
        public void LoadNextScene()
        {
            _sceneLoader.LoadNextScene();
        }

        public void ReloadCurrentScene() => _sceneLoader.ReloadCurrentScene();

    }
}
