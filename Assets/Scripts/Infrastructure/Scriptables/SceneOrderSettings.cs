using UnityEngine;

namespace Infrastructure.Scriptables
{
	[CreateAssetMenu(fileName = "SceneOrderSettings", menuName = "Scriptable Objects/SceneOrderSettings")]
    public class SceneOrderSettings : ScriptableObject
    {
        public int InitialSceneIndex;
        public string[] SceneNames;
    }
}