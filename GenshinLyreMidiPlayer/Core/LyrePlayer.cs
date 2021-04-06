﻿using System.Collections.Generic;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;
using static GenshinLyreMidiPlayer.Core.Keyboard;

namespace GenshinLyreMidiPlayer.Core
{
    public class LyrePlayer
    {
        private static readonly IInputSimulator Input = new InputSimulator();

        private static readonly List<int> LyreNotes = new()
        {
            48, // C3
            50, // D3
            52, // E3
            53, // F3
            55, // G3
            57, // A3
            59, // B3

            60, // C4
            62, // D4
            64, // E4
            65, // F4
            67, // G4
            69, // A4
            71, // B4

            72, // C5
            74, // D5
            76, // E5
            77, // F5
            79, // G5
            81, // A5
            83  // B5
        };

        public static int TransposeNote(int noteId)
        {
            while (true)
            {
                if (LyreNotes.Contains(noteId))
                    return noteId;

                if (noteId < LyreNotes.First())
                    noteId += 12;
                else if (noteId > LyreNotes.Last())
                    noteId -= 12;
                else
                    noteId++;
            }
        }

        public static void PlayNote(int noteId, Layout selectedLayout)
        {
            var key = GetKey(noteId, selectedLayout);
            Input.Keyboard.KeyPress(key);
        }

        public static void NoteDown(int noteId, Layout selectedLayout)
        {
            var key = GetKey(noteId, selectedLayout);
            Input.Keyboard.KeyDown(key);
        }

        public static void NoteUp(int noteId, Layout selectedLayout)
        {
            var key = GetKey(noteId, selectedLayout);
            Input.Keyboard.KeyUp(key);
        }

        private static VirtualKeyCode GetKey(int noteId, Layout selectedLayout)
        {
            var keyIndex = LyreNotes.IndexOf(noteId);
            var key = GetLayout(selectedLayout)[keyIndex];
            return key;
        }
    }
}