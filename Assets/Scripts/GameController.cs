using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using SixDegrees;

public class GameController : MonoBehaviour
{

#if UNITY_IOS
   [DllImport("__Internal")]
   public static extern void GetAPIKey(StringBuilder apiKey, int bufferSize);
#else
   public static void GetAPIKey(StringBuilder apiKey, int bufferSize) { }
#endif
   public Camera ARCamera;

public GameObject ballReference;

public FileControl fileControl;

public SDKController sdkController;

private List<GameObject> balls;

private static string apiKey = "";

private string filename;

void Start()
   {
       balls = new List<GameObject>();
       sdkController.OnSaveSucceededEvent += SaveCSV;
       sdkController.OnLoadSucceededEvent += RetrieveFile;
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
          Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, .5f);
           position = ARCamera.ScreenToWorldPoint(position);
           GameObject ball = Instantiate(ballReference, position, ballReference.transform.rotation);
           balls.Add(ball);
       }
   }

private void GetFilename()
   {
       if (string.IsNullOrEmpty(apiKey))
       {
           StringBuilder sb = new StringBuilder(32);
           GetAPIKey(sb, 32);
           apiKey = sb.ToString();
       }

if (string.IsNullOrEmpty(apiKey))
       {
           Debug.Log("API Key cannot be found");
           filename = "";
       }

if (string.IsNullOrEmpty(SDPlugin.LocationID))
       {
           Debug.Log("Location ID is missing");
           filename = "";
       }

filename = apiKey + "-" + SDPlugin.LocationID;
   }

public void SaveCSV()
   {
       GetFilename();
       if (string.IsNullOrEmpty(filename))
       {
           Debug.Log("Error evaluating the filename, will not save content CSV");
           return;
       }
       string filePath = GetPath();
       StreamWriter writer = new StreamWriter(filePath);
       writer.WriteLine(balls.Count);
       for (int i = 0; i < balls.Count; i++)
       {
           writer.WriteLine(balls[i].transform.position.x + "," + balls[i].transform.position.y + "," + balls[i].transform.position.z);
       }
       writer.Flush();
       writer.Close();
       StartCoroutine(fileControl.UploadFileCoroutine(filename));
   }

public void ReadTextFile(string csv)
   {
       StringReader reader = new StringReader(csv);
       string line = reader.ReadLine();
       int ballCount = int.Parse(line);
       for (int i = 0; i < ballCount; i++)
       {
           line = reader.ReadLine();
           string[] parts = line.Split(',');
           Vector3 ballPosition = new Vector3();
           ballPosition.x = float.Parse(parts[0]);
           ballPosition.y = float.Parse(parts[1]);
           ballPosition.z = float.Parse(parts[2]);
           GameObject ball = Instantiate(ballReference, ballPosition, ballReference.transform.rotation);
           balls.Add(ball);
       }
       reader.Close();
   }

public string GetPath()
   {
       return Application.persistentDataPath + "/" + SDPlugin.LocationID + ".csv";
   }

public void RetrieveFile()
   {
       GetFilename();
       if (string.IsNullOrEmpty(filename))
       {
           Debug.Log("Error evaluating the filename, will not load content CSV");
           return;
       }
       StartCoroutine(fileControl.GetTextCoroutine(filename));
   }
}
