﻿using System.Collections.Generic;

namespace Cawotte.Toolbox.Audio 
{
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;

    /// <summary>
    /// Act as a Singleton Dictionary so other objects can access to any sound they want to play.
    /// It must be filled in the inspector with all music and sounds.
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {

        [SerializeField] private SoundBank soundBank = null;

        private AudioSourcePlayer player; //Used for musics
        [SerializeField, ReadOnly]
        private string musicCurrentlyPlaying = "";

        public AudioSourcePlayer Player => player;


        protected override void OnAwake()
        {
            player = gameObject.AddComponent<AudioSourcePlayer>();
            soundBank.Awake();
        }

        private void Start()
        {
            //PlayMusic("");
        }
        public void PlaySound(string soundname)
        {
            GetComponent<AudioSourcePlayer>().PlaySound(soundname);
        }

        public void PlayMusic(string musicName)
        {

            Sound music = soundBank.FindMusic(musicName);

            if (music == null)
            {
                player.InterruptSound(musicCurrentlyPlaying);
                player.Play(music);
                musicCurrentlyPlaying = musicName;
            }

        }
        #region Public Methods
        

        /// <summary>
        /// Return the Sound object with the given name. 
        /// Used by other objets to access the sounds they want to play.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sound FindSound(string name)
        {
            return soundBank.FindSound(name);
        }
        
        /// <summary>
        /// Return the music with the given name. 
        /// Used by other objets to access the musics they want to play.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sound FindMusic(string name)
        {
            return soundBank.FindMusic(name);
        }

        /// <summary>
        /// Return the SoundList with the given name. 
        /// Used by other objets to access the sounds they want to play.        
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SoundList FindList(string name)
        {
            return soundBank.FindList(name);
        }

        #endregion


    }
}