using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour {

    [SerializeField]
    private AudioSource audioPlayer;

    [SerializeField]
    private AudioClip constructionMusic, planetZMusic, redPlanetMusic, iceGiantMusic;

    private AudioClip playingClip;

    //List of track level names
    private const string planetZTrackName = "RaceTrackPlanetZ";
    private const string redPlanetTrackName = "RaceTrackRedPlanet";
    private const string iceGiantTrackName = "RaceTrackIceGiant";

    // Use this for initialization
    void Start()
    {
        SceneManager.sceneLoaded += SwitchMusicTrack;
        SwitchMusicTrack(SceneManager.GetActiveScene());

        //Persist this object through the whole of the game
        DontDestroyOnLoad(this);
        gameObject.name = "Music Controller";
    }

    /// <summary>
    /// Changes the current music track based on the current scene
    /// </summary>
    /// <param name="scene">Current Scene</param>
    /// <param name="loadMode">(Optional) Scene Loading Mode, does not effect function, defaults to single</param>
    void SwitchMusicTrack(Scene scene, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        string currentLevelName = scene.name;
        AudioClip newClip = ChooseMusic(currentLevelName);

        //If the new clip is different to the new clip then switch clips,
        //otherwise don't restart the clip
        if (newClip != playingClip)
        {

            //Stop the current track
            if (playingClip != null)
            {
                audioPlayer.Stop();
            }

            //Insert and play new clip
            audioPlayer.clip = newClip;
            audioPlayer.Play();
            playingClip = newClip;
        }
    }
    
    /// <summary>
    /// Chooses the correct audio clip based on the given level name
    /// </summary>
    private AudioClip ChooseMusic(string levelName)
    {
        switch (levelName)
        {
            case planetZTrackName:
                return planetZMusic;
            case redPlanetTrackName:
                return redPlanetMusic;
            case iceGiantTrackName:
                return iceGiantMusic;
            default:
                //If we have not loaded a track then load the construction music
                return constructionMusic;
        }

    }
}
