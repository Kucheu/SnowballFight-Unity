using UnityEngine;
using Eflatun.SceneReference;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Kucheu/SceneChooserData")]
public class SceneChooserData : ScriptableObject
{
    [SerializeField]
    private List<SceneReference> scenes;

    public List<SceneReference> Scenes => scenes;
}
