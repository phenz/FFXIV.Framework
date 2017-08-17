﻿using System;
using System.Xml.Serialization;

namespace FFXIV.Framework.TTS.Common.Models
{
    [Serializable]
    public class CevioTalkerModel
    {
        public uint Alpha { get; set; }

        [XmlIgnore]
        public string[] AvailableCasts { get; set; }

        public string Cast { get; set; }

        public CevioTalkerComponent[] Components { get; set; }

        public uint Speed { get; set; }

        public uint Tone { get; set; }

        public uint ToneScale { get; set; }

        public uint Volume { get; set; }

        [Serializable]
        public class CevioTalkerComponent
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public uint Value { get; set; }
        }
    }
}
