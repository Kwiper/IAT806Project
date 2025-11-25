using UnityEngine;
using System.IO;
using SimpleFileBrowser; // File browser library
using static SimpleFileBrowser.FileBrowser;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;

public class AudioLoader : MonoBehaviour
{
    [SerializeField] string fileName;

    AudioSource audioSource;

    private void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Audio", ".wav", ".mp3", ".ogg")); // Set filters to only look for audio files
        FileBrowser.SetDefaultFilter(".wav"); // Set default browser filter to audio files

        audioSource = FindFirstObjectByType<AudioSource>();
    }

    public void LoadDialog() // Public function for button
    {
        StartCoroutine(ShowLoadDialogueCoroutine());
    }

    IEnumerator ShowLoadDialogueCoroutine() // Runs the functions to browse files
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Select Files", "Load");

        if (FileBrowser.Success)
        {
            SetFileName(FileBrowser.Result);
            StartCoroutine(SetAudioClip());
        }
    }

    IEnumerator SetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileName, GetAudioType(fileName))) 
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (audioSource != null)
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogError("AudioSource component not assigned!");
                }
            }
            else
            {
                Debug.LogError("Failed to load audio clip: " + www.error);
            }
        }
    }

    void SetFileName(string[] filePath)
    {
        fileName = "file:///" + filePath[0]; // Set filepath to selected file in file browser
    }

    AudioType GetAudioType(string fileName)
    {
        if (fileName.Contains(".wav"))
        {
            return AudioType.WAV;
        }
        else if (fileName.Contains(".mp3"))
        {
            return AudioType.MPEG;        
        }
        else if (fileName.Contains(".ogg"))
        {
            return AudioType.OGGVORBIS;
        }
        else
        {
            return AudioType.UNKNOWN; // Return unknown file type
        }
    }

}
