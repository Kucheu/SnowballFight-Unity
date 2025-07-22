using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RandomObjectPlacer : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private Vector3 placeAreaSize = new Vector3(100, 10, 100);
    [SerializeField]
    private GameObject[] listOfObjectToPlace;
    [SerializeField, Range(0, 10)]
    private int density = 1;
    [SerializeField]
    private int densityFactor = 10;
    [SerializeField]
    private bool isRandomHeight = false;
    [SerializeField]
    private Vector2 randomHeight = new Vector2(1, 2);
    [SerializeField]
    private LayerMask layerToPut;

    [ContextMenu("Place objects")]
    public void PlaceObjects()
    {
        if(densityFactor <= 0)
        {
            Debug.LogError("density factor can't be negative or 0");
            return;
        }

        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        Vector3 startingPoint = transform.position - placeAreaSize / 2;
        startingPoint.y += placeAreaSize.y;

        for (int i = 0; i < placeAreaSize.x - (densityFactor - 1); i += densityFactor)
        {
            for (int j = 0; j < placeAreaSize.z - (densityFactor - 1); j += densityFactor)
            {
                RaycastHit hit;
                Vector3 currentPoint = startingPoint + new Vector3(i, 0, j);
                for (int k = 0; k < density; k++)
                {
                    Vector3 pointToPut = currentPoint + new Vector3(Random.Range(0f, densityFactor), 0, Random.Range(0f, densityFactor));
                    if (Physics.Raycast(pointToPut, Vector3.down, out hit, placeAreaSize.y, layerToPut))
                    {
                        GameObject objectToSpawn = listOfObjectToPlace[Random.Range(0, listOfObjectToPlace.Length)];
                        GameObject x = (GameObject)PrefabUtility.InstantiatePrefab(objectToSpawn, transform);
                        x.transform.position = hit.point;
                        x.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                        if(isRandomHeight)
                        {
                            float height = Random.Range(randomHeight.x, randomHeight.y);
                            x.transform.localScale = new Vector3(height, height, height);
                        }
                        
                    }
                    else
                    {
                        Debug.LogError("Dont hit");
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, placeAreaSize);
    }
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(RandomObjectPlacer))]
public class RandomObjectPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        if (GUILayout.Button("Place objects"))
        {
            (target as RandomObjectPlacer).PlaceObjects();
        }
    }
}
#endif