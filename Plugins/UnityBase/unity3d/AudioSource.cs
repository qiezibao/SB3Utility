﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using SB3Utility;

namespace UnityPlugin
{
	public class AudioSource : Component, LinkedByGameObject, StoresReferences
	{
		public AssetCabinet file { get; set; }
		public int pathID { get; set; }
		public UnityClassID classID1 { get; set; }
		public UnityClassID classID2 { get; set; }

		public PPtr<GameObject> m_GameObject { get; set; }
		public byte m_Enabled { get; set; }
		public PPtr<AudioClip> m_audioClip { get; set; }
		public bool m_PlayOnAwake { get; set; }
		public float m_Volume { get; set; }
		public float m_Pitch { get; set; }
		public bool Loop { get; set; }
		public bool Mute { get; set; }
		public int Priority { get; set; }
		public float DopplerLevel { get; set; }
		public float MinDistance { get; set; }
		public float MaxDistance { get; set; }
		public float Pan2D { get; set; }
		public int rolloffMode { get; set; }
		public bool BypassEffects { get; set; }
		public bool BypassListenerEffects { get; set; }
		public bool BypassReverbZones { get; set; }
		public AnimationCurve<float> rolloffCustomCurve { get; set; }
		public AnimationCurve<float> panLevelCustomCurve { get; set; }
		public AnimationCurve<float> spreadCustomCurve { get; set; }

		public AudioSource(AssetCabinet file, int pathID, UnityClassID classID1, UnityClassID classID2)
		{
			this.file = file;
			this.pathID = pathID;
			this.classID1 = classID1;
			this.classID2 = classID2;
		}

		public AudioSource(AssetCabinet file) :
			this(file, 0, UnityClassID.AudioSource, UnityClassID.AudioSource)
		{
			file.ReplaceSubfile(-1, this, null);
		}

		public void LoadFrom(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			m_GameObject = new PPtr<GameObject>(stream, file);
			m_Enabled = reader.ReadByte();
			stream.Position += 3;
			m_audioClip = new PPtr<AudioClip>(stream, file);
			m_PlayOnAwake = reader.ReadBoolean();
			stream.Position += 3;
			m_Volume = reader.ReadSingle();
			m_Pitch = reader.ReadSingle();
			Loop = reader.ReadBoolean();
			Mute = reader.ReadBoolean();
			stream.Position += 2;
			Priority = reader.ReadInt32();
			DopplerLevel = reader.ReadSingle();
			MinDistance = reader.ReadSingle();
			MaxDistance = reader.ReadSingle();
			Pan2D = reader.ReadSingle();
			rolloffMode = reader.ReadInt32();
			BypassEffects = reader.ReadBoolean();
			BypassListenerEffects = reader.ReadBoolean();
			BypassReverbZones = reader.ReadBoolean();
			stream.Position += 1;
			rolloffCustomCurve = new AnimationCurve<float>(reader, reader.ReadSingle);
			panLevelCustomCurve = new AnimationCurve<float>(reader, reader.ReadSingle);
			spreadCustomCurve = new AnimationCurve<float>(reader, reader.ReadSingle);
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			m_GameObject.WriteTo(stream);
			writer.Write(m_Enabled);
			stream.Position += 3;
			m_audioClip.WriteTo(stream);
			writer.Write(m_PlayOnAwake);
			stream.Position += 3;
			writer.Write(m_Volume);
			writer.Write(m_Pitch);
			writer.Write(Loop);
			writer.Write(Mute);
			stream.Position += 2;
			writer.Write(Priority);
			writer.Write(DopplerLevel);
			writer.Write(MinDistance);
			writer.Write(MaxDistance);
			writer.Write(Pan2D);
			writer.Write(rolloffMode);
			writer.Write(BypassEffects);
			writer.Write(BypassListenerEffects);
			writer.Write(BypassReverbZones);
			stream.Position += 1;
			rolloffCustomCurve.WriteTo(writer, writer.Write);
			panLevelCustomCurve.WriteTo(writer, writer.Write);
			spreadCustomCurve.WriteTo(writer, writer.Write);
		}
	}
}
