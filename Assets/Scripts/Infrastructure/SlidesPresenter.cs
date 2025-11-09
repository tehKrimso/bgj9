using System;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Infrastructure
{
    public class SlidesPresenter : MonoInstaller
    {
        public GameObject[] Slides;

        private int _currentSlideCount;
        
        [Inject]
        private SceneLoader _sceneLoader;

        public override void InstallBindings()
        {
            //_sceneLoader = Container.Resolve<SceneLoader>();
        }

        public void LoadNextSlide()
        {
            if (_currentSlideCount >= Slides.Length - 1)
            {
                _sceneLoader.LoadNextScene();
            }
            
            Slides[_currentSlideCount].SetActive(false);
            _currentSlideCount++;
            Slides[_currentSlideCount].SetActive(true);
        }

        public void LoadPreviousSlide()
        {
            if (_currentSlideCount == 0)
            {
                return;
            }
            
            Slides[_currentSlideCount].SetActive(false);
            _currentSlideCount--;
            Slides[_currentSlideCount].SetActive(true);
        }
    }
}
