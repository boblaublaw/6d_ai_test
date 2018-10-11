using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FileControl : MonoBehaviour
{

public string downloadURL = "https://persistence-demo.api.6d.ai/?action=get&file=";

public string uploadURL = "https://persistence-demo.api.6d.ai/?action=post&file=";

public GameController gameController;

public IEnumerator GetTextCoroutine(string locID)
   {
       string fullDownloadURL = downloadURL + locID + ".csv";
       UnityWebRequest www = UnityWebRequest.Get(fullDownloadURL);
       yield return www.SendWebRequest();

if (www.isNetworkError || www.isHttpError)
       {
           Debug.Log(www.error);
       }
       else
       {
           string csv = www.downloadHandler.text;
           gameController.ReadTextFile(csv);
       }
       yield return null;
   }

public IEnumerator UploadFileCoroutine(string filename)
   {
       string localFileName = gameController.GetPath();
       string fullUploadURL = uploadURL + filename + ".csv";
       WWW localFile = new WWW("file:///" + localFileName);
       yield return localFile;

if (localFile.error == null)
       {
           Debug.Log("Loaded file successfully");
       }
       else
       {
           Debug.Log("Open file error: " + localFile.error);
           yield break;
       }

WWWForm postForm = new WWWForm();
       postForm.AddBinaryData("Datafile", localFile.bytes, localFileName, "text/plain");
       WWW upload = new WWW(fullUploadURL, postForm);
       yield return upload;
       if (upload.error == null)
       {
           Debug.Log("upload done :" + upload.text);
       }
       else
       {
           Debug.Log("Error during upload: " + upload.error);
       }
       yield return null;
   }
}
