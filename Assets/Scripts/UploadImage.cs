using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Android;
using UnityEngine.Rendering;

public class UploadImage : MonoBehaviour
{
    public Button button;
    public string FinalPath;

    public Image myButtonImage;
    public Sprite newSprite;

    public PuzzleSelector inst1;


    public void LoadFile()
    {
        string FileType = NativeFilePicker.ConvertExtensionToFileType("png,jpeg,jpg");

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.Log("Operations Cancelled");

            }
            else
            {
                FinalPath = path;
                Debug.Log("Picked Files: " + FinalPath);
                StartCoroutine("LoadImage");
            }
        }, new string[] { FileType });
    }

    public IEnumerator LoadImage()
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + FinalPath);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(uwr.error);

        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
            newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            GameObject myButton = GameObject.FindWithTag("UploadedImage");
            myButtonImage = myButton.GetComponent<Image>();
            myButtonImage.sprite = newSprite;
            inst1 = GetComponent<PuzzleSelector>();
            inst1.SetPuzzlePhoto(newSprite);
        }
    }
    void Start()
    {

        
        if (button == null)
        {
            Debug.LogError("Button object is not assigned in the inspector");
            return;
        }
        button.onClick.AddListener(LoadFile);
    }
    private void ButtonClicked()
    {
        Debug.Log("Button was clicked!");
    }


    // Update is called once per frame
    void Update()
    {

    }
}
