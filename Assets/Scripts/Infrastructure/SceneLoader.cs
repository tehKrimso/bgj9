using System;
using System.Collections;
using Infrastructure.Scriptables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly SceneOrderSettings _sceneOrder;
        
        private int _currentSceneIndex;

        public SceneLoader(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain,
            SceneOrderSettings sceneOrder)
        {
            _coroutineRunner = coroutineRunner;
            _loadingCurtain = loadingCurtain;
            _sceneOrder = sceneOrder;
            _currentSceneIndex = _sceneOrder.InitialSceneIndex;
        }

       public void LoadInitialScene() => _coroutineRunner.StartCoroutine(LoadScene(_sceneOrder.SceneNames[_sceneOrder.InitialSceneIndex], _loadingCurtain.Hide));
       //public void LoadInitialScene() => LoadNextScene();
        
        public void LoadNextScene()
        {
            if (_currentSceneIndex == _sceneOrder.SceneNames.Length)
            {
                Debug.LogWarning($"You are currently on a last scene. There is no next scene to load.");
                return;
            }
            
            _coroutineRunner.StartCoroutine(LoadScene(_sceneOrder.SceneNames[++_currentSceneIndex],_loadingCurtain.Hide));
        }

        public void ReloadCurrentScene()
        {
            //_coroutineRunner.StartCoroutine(LoadScene(_sceneOrder.SceneNames[_currentSceneIndex]));
            _coroutineRunner.StartCoroutine(ReloadScene(_loadingCurtain.Hide));
            
        }

        public void LoadSceneByName(string sceneName) => _coroutineRunner.StartCoroutine(LoadScene(sceneName));


        private IEnumerator LoadScene(string nextSceneName, Action onLoaded = null)
        {
            _loadingCurtain.Show();
            
            if (SceneManager.GetActiveScene().name == nextSceneName)
            {
                onLoaded?.Invoke();
                yield break;
            }
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextSceneName);
            
            while (!waitNextScene.isDone)
                yield return null;
            
            onLoaded?.Invoke();
        }
        
        private IEnumerator ReloadScene(Action onLoaded = null)
        {
            _loadingCurtain.Show();
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(_sceneOrder.SceneNames[_currentSceneIndex]);
            
            while (!waitNextScene.isDone)
                yield return null;
            
            onLoaded?.Invoke();
        }
    }
}
