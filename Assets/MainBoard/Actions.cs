using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{

    [SerializeField]
    private GameObject _ResultTextObj;

    public void ThrowDice()
    {
        int _battleDecider = Random.Range(1, 12);
        int _landNo = Random.Range(1, 12);

        if (_battleDecider % 2 == 0)
            _ResultTextObj.GetComponent<UnityEngine.UI.InputField>().text = @"ATTACKER. Occupy " + _landNo + " grid";

        else
            _ResultTextObj.GetComponent<UnityEngine.UI.InputField>().text = @"DEFENDER. Occupy " + _landNo + " grid";

    }

    void ChangeLandOwner()
    {

    }

    private void Update()
    {
        int speed = 1;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * speed * Time.deltaTime, -touchDeltaPosition.y * speed * Time.deltaTime, 0);

            Vector3 tmpPosX = transform.position;
            tmpPosX.x = Mathf.Clamp(tmpPosX.x, -13.0f, 14.0f);
            transform.position = tmpPosX;


            Vector3 tmpPosY = transform.position;
            tmpPosY.y = Mathf.Clamp(tmpPosY.y, -7.0f, 18.0f);
            transform.position = tmpPosY;

        }

    }
}
