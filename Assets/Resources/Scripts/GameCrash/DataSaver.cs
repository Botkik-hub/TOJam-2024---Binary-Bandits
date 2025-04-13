using System;
using System.IO;
using UnityEngine;

public class DataSaver
{
    public class Data
    {
        public class SettingsData
        {
            public Color Color;
            public float Master = 0.5f;
            public float Music = 1.0f;
            public float SoundEffects = 1.0f;
            public float VoiceLines = 1.0f;
        }
        public class Statistics
        {
            public int Jumps = 0;
            public int Deaths = 0;
            public int Dashes = 0;
            public DateTime StartTime;
        }
        public int Checkpoint;
        public int BlameStas = 0;
        public SettingsData Settings = new(); 
        public Statistics Stats = new();
    }
    public static string SavePath => Application.persistentDataPath;
    
    public const string SaveFileName = "GameData.txt";


    public void Save(Data dataToSave)
    {
        using StreamWriter streamWriter = new StreamWriter(SavePath + '\\' + SaveFileName);
        streamWriter.WriteLine(dataToSave.Checkpoint);
        streamWriter.WriteLine(dataToSave.BlameStas);
        streamWriter.WriteLine($"{dataToSave.Settings.Color.r} {dataToSave.Settings.Color.g} {dataToSave.Settings.Color.b}");
        streamWriter.WriteLine($"{dataToSave.Settings.Master} {dataToSave.Settings.Music} {dataToSave.Settings.SoundEffects} {dataToSave.Settings.VoiceLines}");
        streamWriter.WriteLine($"{dataToSave.Stats.Jumps} {dataToSave.Stats.Deaths} {dataToSave.Stats.Dashes}");
        streamWriter.WriteLine($"{dataToSave.Stats.StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
    }

    public bool TryLoad(out Data data)
    {
        try
        { 
            using StreamReader streamReader = new StreamReader(SavePath + '\\' + SaveFileName);
            int checkpoint;
            int blame;
            Color color;
            float master, music, soundEffects, voiceLines;
            int jumps, deaths, dashes;
            DateTime startTime;
            
            string sCheckpoint = streamReader.ReadLine();
            checkpoint = int.Parse(sCheckpoint);
            string sBlame= streamReader.ReadLine();
            blame = int.Parse(sBlame);
            string sColor = streamReader.ReadLine();
            string[] values = sColor.Split(' ');
            color = new Color(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), 1.0f);
            string sSounds= streamReader.ReadLine();
            values = sSounds.Split(' ');
            master = float.Parse(values[0]);
            music= float.Parse(values[1]);
            soundEffects = float.Parse(values[2]);
            voiceLines = float.Parse(values[3]);
            string sStats= streamReader.ReadLine();
            values = sStats.Split(' ');
            jumps = int.Parse(values[0]);
            deaths = int.Parse(values[1]);
            dashes = int.Parse(values[2]);
            string sDate = streamReader.ReadLine();
            startTime = DateTime.ParseExact(sDate, "yyyy-MM-dd HH:mm:ss.fff", null);
            
            data = new Data()
            {
                Checkpoint = checkpoint,
                BlameStas = blame,
                Settings = new Data.SettingsData()
                {
                    Color =  color,
                    Master = master,
                    Music = music,
                    SoundEffects = soundEffects,
                    VoiceLines = voiceLines
                },
                Stats = new Data.Statistics()
                {
                    Deaths = deaths,
                    Jumps = jumps,
                    Dashes = dashes,
                    StartTime = startTime
                }
            };
        }
        catch 
        {
            data = null;
            return false;
        }

        return true;
    }
}
