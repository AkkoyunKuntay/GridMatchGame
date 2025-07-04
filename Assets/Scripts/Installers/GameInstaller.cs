using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {

        Container.Bind<GridService>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GridPresenter>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ScoreService>().FromComponentInHierarchy().AsSingle();
    }
}