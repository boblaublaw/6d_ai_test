using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

public Camera ARCamera;

public GameObject ballReference;

private List<GameObject> balls;

void Start()
   {
       balls = new List<GameObject>();
   }

void Update()
   {
       if (Input.touchCount > 0)
       {
           Touch touch = Input.GetTouch(0);
           LaunchBall(touch);
       }
   }

void LaunchBall(Touch touch)
   {
       if (EventSystem.current.currentSelectedGameObject == null &&
           touch.phase == TouchPhase.Began)
       {
           Vector3 position = new Vector3(Input.mousePosition.x,  Input.mousePosition.y, .5f);
           position = ARCamera.ScreenToWorldPoint(position);
           GameObject ball = Instantiate(ballReference, position, ballReference.transform.rotation);
           balls.Add(ball);
       }
   }
}
