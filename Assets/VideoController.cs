// 2023-11-13 AI-Tag 
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private Material videoMaterial;
    
    public VideoPlayer videoPlayer;
    

    void Start()
    {
        videoPlayer.url = "D:\\Racing\\Assets\\lobby.mp4";

        videoPlayer.Play();
    }
}
