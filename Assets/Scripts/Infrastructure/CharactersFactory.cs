using UnityEngine;

namespace Infrastructure
{
    public class CharactersFactory : ICharacterFactory
    {
        public GameObject CreateBattleCharacter(GameObject prefab, Transform parent)
        {
            return GameObject.Instantiate(prefab, parent);
        }

        public GameObject CreateEnemyCharacter(GameObject prefab, Transform parent)
        {
            return GameObject.Instantiate(prefab, parent);
        }

        public GameObject CreateMainCharacter(GameObject prefab, Transform parent)
        {
            return GameObject.Instantiate(prefab, parent);
        }
    }

    public interface ICharacterFactory
    {
        public GameObject CreateBattleCharacter(GameObject prefab, Transform parent);
        public GameObject CreateEnemyCharacter(GameObject prefab, Transform parent);
        public GameObject CreateMainCharacter(GameObject prefab, Transform parent);
    }
}
