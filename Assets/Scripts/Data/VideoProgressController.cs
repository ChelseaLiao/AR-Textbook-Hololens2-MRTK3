using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


namespace MixedReality.Toolkit.UX
{
    public class VideoProgressController : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private VideoPlayer videoPlayer;
        public GameObject playButton;
        public GameObject pauseButton;
        private bool isDragging = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(videoPlayer.isPlaying)
                switchToPause();
            else
                switchToPlay();

            if(!isDragging && videoPlayer.frameCount>0)
                slider.Value = (float)videoPlayer.frame / (float)videoPlayer.frameCount;

        }

        public void startDragging()
        {
            isDragging = true;
        }
        public void readProgress()
        {
            isDragging = false;
            var frame = videoPlayer.frameCount * slider.Value;
            videoPlayer.frame = (long)frame;
        }
        public void switchToPlay()
        {
            pauseButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
        }

        public void switchToPause()
        {
            playButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
        }
    }

}
