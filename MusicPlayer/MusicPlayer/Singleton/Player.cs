﻿using Microsoft.Win32;
using MusicPlayer.CommandPattern;
using MusicPlayer.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TagLib;

namespace MusicPlayer
{
    

    public sealed class Player
    {
        public static MediaPlayer mplayer;
        public ObservableCollection<Song> queue { get; set; }
        public int currentSongPointer { get; set; }
        int slot;
        CommandInvoker cmdControl;

        //testing observer pattern
        Subject subject;

        private Player()
        {
            mplayer = new MediaPlayer();
            queue = new ObservableCollection<Song>();
            currentSongPointer = -1;
            cmdControl = new CommandInvoker();
            slot = 0;               //slot to be used when setting commands
            subject = new Subject();
            subject.setState(currentSongPointer);
            new Observer1(subject);
        }

        private static Player instance = null;

        public static Player Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Player();
                }
                return instance;
            }
        }

        public void skipBack()
        {
            if (currentSongPointer <= 0)
            {
                //do nothing
            }
            else
            {
                subject.setState(--currentSongPointer);
                //currentSongPointer--;
                //cmdControl.playbtnPushed(currentSongPointer);
                cmdControl.playbtnPushed(subject.getState());
                
            }
        }

        public void pause()
        {
            if(currentSongPointer > -1 && currentSongPointer <= queue.Count)
            {
                //cmdControl.pausebtnPushed(currentSongPointer);
                subject.setState(currentSongPointer);
                cmdControl.pausebtnPushed(subject.getState());
            }
            
        }

        public void play()
        {
            if(currentSongPointer > -1 && currentSongPointer <= queue.Count)
            {
                //cmdControl.playbtnPushed(currentSongPointer);
                subject.setState(currentSongPointer);
                cmdControl.playbtnPushed(subject.getState());
            }
            
        }

        public void skipForward()
        {
            if (currentSongPointer >= queue.Count || currentSongPointer <= -1)
            {
                //do nothing
            }
            else
            {
                //currentSongPointer++;
                //cmdControl.playbtnPushed(currentSongPointer);
                subject.setState(++currentSongPointer);
                cmdControl.playbtnPushed(subject.getState());
            }
        }



        public void import(Song song)
        {
            mplayer.Open(song.filePath);
            queue.Add(song);
            currentSongPointer++;
            cmdControl.setCommand(slot, new PlayCommand((Song)queue[currentSongPointer]), new PauseCommand((Song)queue[currentSongPointer]));
            slot++;
        }
    }
}
