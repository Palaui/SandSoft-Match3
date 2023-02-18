using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour
{
    [SerializeField] private ElementTypeDataScriptable elementTypeData;
    [SerializeField] private MeshRenderer cellPrefab;

    private List<MeshRenderer> renderers = new List<MeshRenderer>();


    public void Setup(int[,] gridData)
    {
        foreach (MeshRenderer renderer in renderers)
            Destroy(renderer.gameObject);
        renderers.Clear();

        for (int i = 0; i < gridData.GetLength(0); i++)
        {
            for (int j = 0; j < gridData.GetLength(1); j++)
            {
                int elementType = gridData[i, j];

                Vector3 position = new Vector3(i - gridData.GetLength(0) / 2f, j - gridData.GetLength(1) / 2f, 0);
                MeshRenderer cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cell.material.SetTexture("_MainTex", elementTypeData.GetTextureByID(elementType));
                renderers.Add(cell);
            }
        }
    }

    private IEnumerator SetupCoroutine()
    {
        yield return null;
    }
}