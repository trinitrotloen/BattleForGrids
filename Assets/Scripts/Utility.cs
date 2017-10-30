using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{

    private static List<Color> colorList = new List<Color>(new Color[] { Color.red, Color.blue, Color.green, Color.yellow });
    public static int _battleDecider, _landNo;
    public static int PlayerNo = 1, OppPlayerNo = 4;
    public static string winner;

    private static bool _isDiceThrown = false;

    [SerializeField]
    private GameObject _ResTextObj;
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random rnd = new System.Random(seed);

        for (int i = 0; i < array.Length - 1; i++)
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

    public void Throwdice()
    {
        _battleDecider = Random.Range(1, 12);
        _landNo = Random.Range(1, 12);

        if (_battleDecider % 2 == 0)
        {
            winner = "ATTACKER";
            _ResTextObj.GetComponent<UnityEngine.UI.Text>().text = "ATTACKER\nOccupy " + _landNo + " grid";
            print("ATTACKER\nOccupy " + _landNo + " grid");
        }
        else
        {
            winner = "DEFENDER";
            _ResTextObj.GetComponent<UnityEngine.UI.Text>().text = "DEFENDER\nOccupy " + _landNo + " grid";
            print("DEFENDER\nOccupy " + _landNo + " grid");
        }
        _isDiceThrown = true;
        GameObject.Find("ThrowDiceButton").gameObject.SetActive(false);
    }

    public static void ChangeLandOwner(GameObject _loser)
    {
        Renderer _loserRenderer = _loser.GetComponent<Renderer>();

        if (winner.Equals("ATTACKER"))
        {
            _loserRenderer.material.color = colorList[PlayerNo - 1];
        }
        else
        {
            _loserRenderer.material.color = colorList[OppPlayerNo - 1];
        }
        _landNo--;
        print(_landNo);
        if (_landNo == 0)
        {
            _isDiceThrown = false;
            GameObject.FindWithTag("ThrowDiceButton").gameObject.SetActive(true);
        }
    }
}
