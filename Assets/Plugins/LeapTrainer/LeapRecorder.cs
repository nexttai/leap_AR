/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Leap;
using System.Runtime.Serialization.Formatters.Binary;
using Utils;
using Leap.Unity;



    /** The states of the record-playback machine. */
    public enum RecorderState
    {
        Stopped = 0,
        Paused = 1,
        Recording = 2,
        Playing = 3
    }

    /**
     * Maintains a buffer of recorded frames and tracks the state of playback and recording.
     */
    public class LeapRecorder : MonoBehaviour
    {

        /** Playback speed. */
        public float speed = 1.0f;
        /** Whether to lop back to the beginning when the last recorded frame is played.
         // 最後に記録されたフレームが再生されたときに先頭に戻るかどうか。*/
        public bool loop = true;
        /** The current play state. */
        public RecorderState state = RecorderState.Playing;

        protected List<byte[]> frames_;
        protected float frame_index_;
        protected Frame current_frame_ = new Frame();

        /** Creates a new LeapRecorder object. This doesn't make sense outside the context of a HandController object. */
        //public LeapRecorder()
        //{
        //    Debug.Log("毎回呼び出されている?");
        //    Reset();

        //}




        /** Sets the play state to stopped. Also resets the frame index to 0. */
        public void Stop()
        {
            state = RecorderState.Stopped;
            frame_index_ = 0.0f;
        }

        /** Sets the play state to paused. */
        public void Pause()
        {
            state = RecorderState.Paused;
        }

        /** Sets the play state to playing. */
        public void Play()
        {
            state = RecorderState.Playing;
        }

        /** Sets the play state to recording. */
        public void Record()
        {
            state = RecorderState.Recording;
        }

        /** Discards any recorded frames. */
        public void Resets()
        {

            frames_ = new List<byte[]>();
            frame_index_ = 0;
            if (frames_ != null)
            {
                Debug.Log("frames_入ってる");
            }



        }

        /** Restores the default behaviors. */
        public void SetDefault()
        {
            speed = 1.0f;
            loop = true;
        }

        /** Returns the ratio of the current playback position to the total recording length. */
        public float GetProgress()
        {
            return frame_index_ / frames_.Count;
        }

        /** Returns the playback position. */
        public int GetIndex()
        {
            return (int)frame_index_;
        }

        /** 
         * Sets the playback position to the specified frame count (or the last frame if the 
         * specified index is after the last frame.
         */
        public void SetIndex(int new_index)
        {
            if (new_index >= frames_.Count)
            {
                frame_index_ = frames_.Count - 1;
            }
            else
            {
                frame_index_ = new_index;
            }
        }

        /** Serializes a Leap Frame object and adds it to the end of the recording.
         * // Leap Frameオブジェクトをシリアル化し、それを記録の最後に追加します。*/
        public void AddFrame(Frame frame)
        {



            frames_.Add(frame.SerializeToByteArray());
            //シリアライズできているか確認。
            //   Frame deserializedFrame = frames_[0].Deserialize<Frame>();
            //  Debug.Log(deserializedFrame.CurrentFramesPerSecond);

        }

        /** Returns the current frame without advancing the playhead. This frame could be invalid. */
        public Frame GetCurrentFrame()
        {
            return current_frame_;
        }

        /** Advances the playhead, deserializes the frame, and returns it.*/


        //public Frame NextFrame()
        //{
        //    current_frame_ = new Frame();

        //    if (frames_.Count > 0)
        //    {
        //        if (frame_index_ >= frames_.Count && loop)
        //        {
        //            frame_index_ -= frames_.Count;
        //        }
        //        else if (frame_index_ < 0 && loop)
        //        {
        //            frame_index_ += frames_.Count;
        //        }
        //        if (frame_index_ < frames_.Count && frame_index_ >= 0)
        //        {
        //            List<Frame> nextFrame = frames_[(int)frame_index_].Deserialize<List<Frame>>();

        //            foreach (Frame deFrame in nextFrame)
        //            {
        //                current_frame_ = deFrame;
        //            }


        //            // current_frame_.Deserialize(frames_[(int)frame_index_]);
        //            frame_index_ += speed;
        //        }
        //    }
        //    return current_frame_;
        //}
        public Frame NextFrame()
        {
            current_frame_ = new Frame();
            if (frames_.Count > 0)
            {
                if (frame_index_ >= frames_.Count && loop)
                {
                    frame_index_ -= frames_.Count;
                }
                else if (frame_index_ < 0 && loop)
                {
                    frame_index_ += frames_.Count;
                }
                if (frame_index_ < frames_.Count && frame_index_ >= 0)
                {
                    current_frame_ = frames_[(int)frame_index_].Deserialize<Frame>();
                    frame_index_ += speed;
                }
            }
            return current_frame_;
        }

        /** Deserializes all the recorded frames and returns them in a new list. */
        //今回、すべてのframeの記録をデシリアライズして、byteファイルにした後、新しいリストのframesを作る。
        //そこで、framesに入れたいものはバイト型の配列のframes_をデシリアライズしたframe型のframeを追加。
        //public List<Frame> GetFrames() {
        //    List<Frame> frames = new List<Frame>();
        //    for (int i = 0; i < frames_.Count; ++i) {

        //        foreach (byte[] frameGet in frames_) {

        //            List<Frame> hoge=frameGet.Deserialize<List<Frame>>();
        //            foreach (Frame preframe in hoge)
        //            {
        //                frames.Add(preframe);

        //            }
        //        }


        //    }
        //    return frames;
        //}

        //byte型の元のデータをFrame型に変える。
        //ここで、すべてデシリアライズ？？
        public List<Frame> GetFrames()
        {
            List<Frame> frames = new List<Frame>();
            for (int i = 0; i < frames_.Count; ++i)
            {
                Frame frame = new Frame();
                frame = frames_[(int)frame_index_].Deserialize<Frame>();
                frames.Add(frame);
            }
            return frames;
        }
        /** The number of recorded frames. */
        public int GetFramesCount()
        {
            return frames_.Count;
        }

        /** Saves the recorded frames to a file, overwriting an existing file. 
            The filename is automatically chosen and is stored in Unity's persistant data path. これは、examモードのときの結果を記録しているの感
              */
        //recorderFからRecordingUに書き換える。
        public string SaveToNewFile()
        {
            string RecordingU = Application.persistentDataPath + "/Recording_" +
                          System.DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".bytes";
            Debug.Log(RecordingU);
            return SaveToNewFile(RecordingU);
        }

        /** Saves the recorded frames to a file, overwriting an existing file.
         * // 記録したフレームをファイルに保存し、既存のファイルを上書きします。*/
        //frameデータ
        public string SaveToNewFile(string path)
        {
            if (File.Exists(@path))
            {
                File.Delete(@path);
            }

            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            for (int i = 0; i < frames_.Count; ++i)
            {
                byte[] frame_size = new byte[4];
                frame_size = System.BitConverter.GetBytes(frames_[i].Length);
                stream.Write(frame_size, 0, frame_size.Length);
                stream.Write(frames_[i], 0, frames_[i].Length);
            }

            stream.Close();
            return path;
        }

        /** Loads saved frames from a file. */

        public void Load(TextAsset text_asset)
        {
            Load(text_asset.bytes);
        }

        /** Loads saved frames from byte array. */
        //frames_にすべてのdataの情報を格納する。リスト化しただけ。
        public void Load(byte[] data)
        {
            frame_index_ = 0;
            if (frames_ == null)
            {
                Debug.Log("Loadにてframes_入ってない");
            }
            frames_.Clear();
            int i = 0;
            while (i < data.Length)
            {
                byte[] frame_size = new byte[4];
                //バイト型の４つの要素にdataをコピー
                Array.Copy(data, i, frame_size, 0, frame_size.Length);
                i += frame_size.Length;
                //先ほどのdataをコピーされたframe_sizeを32ビット型に変換したものをframeに入れる。
                byte[] frame = new byte[System.BitConverter.ToUInt32(frame_size, 0)];
                //dataをframeにframe.Length分(3562?)個文、入れる。リストの要素である
                Array.Copy(data, i, frame, 0, frame.Length);
                //iにコピーした数を入れる。
                i += frame.Length;
                //リスト型のframes_にframeを加える。
                frames_.Add(frame);
            }
        }



    }
