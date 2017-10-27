using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    private static List<Color> colorList = new List<Color>(new Color[] { Color.red, Color.blue, Color.green, Color.yellow });

    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random rnd = new System.Random(seed);

        for (int i = 0; i < array.Length-1; i++)
        {
            int rndIndex = rnd.Next(i, array.Length);
            T temp = array[rndIndex];
            array[rndIndex] = array[i];
            array[i] = temp;
        }

        return array;
    }

    public static void RandomColor(ref Transform _transObj)
    {
        Renderer _rend = _transObj.GetComponent<Renderer>();
        _rend.material.color = colorList[Random.Range(0, colorList.Count)];
    }
}
