using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour
{
    private const float TIME_PER_SPAWN = 0.01f;
    private const float SPAWN_ANIM_TIME = 0.4f;

    [SerializeField] private ElementTypeDataScriptable elementTypeData;
    [SerializeField] private MeshRenderer cellPrefab;

    private List<MeshRenderer> renderers = new List<MeshRenderer>();


    public void Setup(int[,] gridData)
    {
        StopAllCoroutines();

        foreach (MeshRenderer renderer in renderers)
            Destroy(renderer.gameObject);
        renderers.Clear();

        StartCoroutine(SetupCoroutine(gridData));
    }

    private IEnumerator SetupCoroutine(int[,] gridData)
    {
        WaitForSeconds wait = new WaitForSeconds(TIME_PER_SPAWN);

        for (int i = 0; i < gridData.GetLength(0); i++)
        {
            for (int j = 0; j < gridData.GetLength(1); j++)
            {
                int elementType = gridData[i, j];

                Vector3 position = new Vector3(i - gridData.GetLength(0) / 2f, j - gridData.GetLength(1) / 2f, 0);
                MeshRenderer cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cell.material.SetTexture("_MainTex", elementTypeData.GetTextureByID(elementType));
                renderers.Add(cell);

                StartCoroutine(AnimateCellCoroutine(cell.transform));

                yield return wait;
            }
        }
    }

    private IEnumerator AnimateCellCoroutine(Transform tr)
    {
        float currentTime = 0;
        while (currentTime < SPAWN_ANIM_TIME)
        {
            float alpha = currentTime / SPAWN_ANIM_TIME;
            tr.localScale = (1 - (1 - alpha) * (1 - alpha)) * Vector3.one;

            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}