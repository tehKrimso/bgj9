using Infrastructure.Scriptables;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class BootstrapperInstaller : MonoInstaller, ICoroutineRunner
    {
        public LoadingCurtain loadingCurtain;
        
        [Header("Settings")]
        public SceneOrderSettings SceneOrderSettings;
        
        
        public override void InstallBindings()
        {
            BindLoadingCurtain();
            BindServices();
        }

        public override void Start()
        {
            base.Start();
            Container.Resolve<SceneLoader>().LoadInitialScene();
        }
        
        private void BindLoadingCurtain() =>
            Container
                .Bind<LoadingCurtain>()
                .FromInstance(loadingCurtain)
                .AsSingle();

        private void BindServices()
        {
            SceneLoader sceneLoader = new SceneLoader(this, loadingCurtain ,SceneOrderSettings);
            Container.Bind<SceneLoader>().FromInstance(sceneLoader).AsSingle().NonLazy();

            //Container.Bind<IInputService>().FromInstance(new InputService()).AsSingle().NonLazy();
        }
        
    }
}
