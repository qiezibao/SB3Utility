﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SlimDX;

using SB3Utility;

namespace UnityPlugin
{
	public class Node : IObjInfo
	{
		public int m_ParentId { get; set; }
		public int m_AxesId { get; set; }

		public Node(int parentId, int axesId)
		{
			m_ParentId = parentId;
			m_AxesId = axesId;
		}

		public Node(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			m_ParentId = reader.ReadInt32();
			m_AxesId = reader.ReadInt32();
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(m_ParentId);
			writer.Write(m_AxesId);
		}
	}

	public class Limit : IObjInfo
	{
		public Vector4 m_Min { get; set; }
		public Vector4 m_Max { get; set; }

		public Limit(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			m_Min = reader.ReadVector4();
			m_Max = reader.ReadVector4();
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(m_Min);
			writer.Write(m_Max);
		}
	}

	public class Axes : IObjInfo
	{
		public Vector4 m_PreQ { get; set; }
		public Vector4 m_PostQ { get; set; }
		public Vector4 m_Sgn { get; set; }
		public Limit m_Limit { get; set; }
		public float m_Length { get; set; }
		public uint m_Type { get; set; }

		public Axes(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			m_PreQ = reader.ReadVector4();
			m_PostQ = reader.ReadVector4();
			m_Sgn = reader.ReadVector4();
			m_Limit = new Limit(stream);
			m_Length = reader.ReadSingle();
			m_Type = reader.ReadUInt32();
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(m_PreQ);
			writer.Write(m_PostQ);
			writer.Write(m_Sgn);
			m_Limit.WriteTo(stream);
			writer.Write(m_Length);
			writer.Write(m_Type);
		}
	}

	public class Skeleton : IObjInfo
	{
		public List<Node> m_Node { get; set; }
		public List<uint> m_ID { get; set; }
		public List<Axes> m_AxesArray { get; set; }

		public Skeleton(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);

			int numNodes = reader.ReadInt32();
			m_Node = new List<Node>(numNodes);
			for (int i = 0; i < numNodes; i++)
			{
				m_Node.Add(new Node(stream));
			}

			int numIDs = reader.ReadInt32();
			m_ID = new List<uint>(numIDs);
			for (int i = 0; i < numIDs; i++)
			{
				m_ID.Add(reader.ReadUInt32());
			}

			int numAxes = reader.ReadInt32();
			m_AxesArray = new List<Axes>(numAxes);
			for (int i = 0; i < numAxes; i++)
			{
				m_AxesArray.Add(new Axes(stream));
			}
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);

			writer.Write(m_Node.Count);
			for (int i = 0; i < m_Node.Count; i++)
			{
				m_Node[i].WriteTo(stream);
			}

			writer.Write(m_ID.Count);
			for (int i = 0; i < m_ID.Count; i++)
			{
				writer.Write(m_ID[i]);
			}

			writer.Write(m_AxesArray.Count);
			for (int i = 0; i < m_AxesArray.Count; i++)
			{
				m_AxesArray[i].WriteTo(stream);
			}
		}
	}

	public class SkeletonPose : IObjInfo
	{
		public List<xform> m_X { get; set; }

		public SkeletonPose(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);

			int numXforms = reader.ReadInt32();
			m_X = new List<xform>(numXforms);
			for (int i = 0; i < numXforms; i++)
			{
				m_X.Add(new xform(stream));
			}
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);

			writer.Write(m_X.Count);
			for (int i = 0; i < m_X.Count; i++)
			{
				m_X[i].WriteTo(stream);
			}
		}
	}

	public class Hand : IObjInfo
	{
		public List<int> m_HandBoneIndex { get; set; }

		public Hand(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);

			int numIndexes = reader.ReadInt32();
			m_HandBoneIndex = new List<int>(numIndexes);
			for (int i = 0; i < numIndexes; i++)
			{
				m_HandBoneIndex.Add(reader.ReadInt32());
			}
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);

			writer.Write(m_HandBoneIndex.Count);
			for (int i = 0; i < m_HandBoneIndex.Count; i++)
			{
				writer.Write(m_HandBoneIndex[i]);
			}
		}
	}

	public class Handle : IObjInfo
	{
		public xform m_X { get; set; }
		public uint m_ParentHumanIndex { get; set; }
		public uint m_ID { get; set; }

		public Handle(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			m_X = new xform(stream);
			m_ParentHumanIndex = reader.ReadUInt32();
			m_ID = reader.ReadUInt32();
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			m_X.WriteTo(stream);
			writer.Write(m_ParentHumanIndex);
			writer.Write(m_ID);
		}
	}

	public class Collider : IObjInfo
	{
		public xform m_X { get; set; }
		public uint m_Type { get; set; }
		public uint m_XMotionType { get; set; }
		public uint m_YMotionType { get; set; }
		public uint m_ZMotionType { get; set; }
		public float m_MinLimitX { get; set; }
		public float m_MaxLimitX { get; set; }
		public float m_MaxLimitY { get; set; }
		public float m_MaxLimitZ { get; set; }

		public Collider(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			m_X = new xform(stream);
			m_Type = reader.ReadUInt32();
			m_XMotionType = reader.ReadUInt32();
			m_YMotionType = reader.ReadUInt32();
			m_ZMotionType = reader.ReadUInt32();
			m_MinLimitX = reader.ReadSingle();
			m_MaxLimitX = reader.ReadSingle();
			m_MaxLimitY = reader.ReadSingle();
			m_MaxLimitZ = reader.ReadSingle();
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			m_X.WriteTo(stream);
			writer.Write(m_Type);
			writer.Write(m_XMotionType);
			writer.Write(m_YMotionType);
			writer.Write(m_ZMotionType);
			writer.Write(m_MinLimitX);
			writer.Write(m_MaxLimitX);
			writer.Write(m_MaxLimitY);
			writer.Write(m_MaxLimitZ);
		}
	}

	public class Human : IObjInfo
	{
		public xform m_RootX { get; set; }
		public Skeleton m_Skeleton { get; set; }
		public SkeletonPose m_SkeletonPose { get; set; }
		public Hand m_LeftHand { get; set; }
		public Hand m_RightHand { get; set; }
		public List<Handle> m_Handles { get; set; }
		public List<Collider> m_ColliderArray { get; set; }
		public List<int> m_HumanBoneIndex { get; set; }
		public List<float> m_HumanBoneMass { get; set; }
		public List<int> m_ColliderIndex { get; set; }
		public float m_Scale { get; set; }
		public float m_ArmTwist { get; set; }
		public float m_ForeArmTwist { get; set; }
		public float m_UpperLegTwist { get; set; }
		public float m_LegTwist { get; set; }
		public float m_ArmStretch { get; set; }
		public float m_LegStretch { get; set; }
		public float m_FeetSpacing { get; set; }
		public bool m_HasLeftHand { get; set; }
		public bool m_HasRightHand { get; set; }

		public Human(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			m_RootX = new xform(stream);
			m_Skeleton = new Skeleton(stream);
			m_SkeletonPose = new SkeletonPose(stream);
			m_LeftHand = new Hand(stream);
			m_RightHand = new Hand(stream);

			int numHandles = reader.ReadInt32();
			m_Handles = new List<Handle>(numHandles);
			for (int i = 0; i < numHandles; i++)
			{
				m_Handles.Add(new Handle(stream));
			}

			int numColliders = reader.ReadInt32();
			m_ColliderArray = new List<Collider>(numColliders);
			for (int i = 0; i < numColliders; i++)
			{
				m_ColliderArray.Add(new Collider(stream));
			}

			int numIndexes = reader.ReadInt32();
			m_HumanBoneIndex = new List<int>(numIndexes);
			for (int i = 0; i < numIndexes; i++)
			{
				m_HumanBoneIndex.Add(reader.ReadInt32());
			}

			int numMasses = reader.ReadInt32();
			m_HumanBoneMass = new List<float>(numMasses);
			for (int i = 0; i < numMasses; i++)
			{
				m_HumanBoneMass.Add(reader.ReadSingle());
			}

			int numColliderIndexes = reader.ReadInt32();
			m_ColliderIndex = new List<int>(numColliderIndexes);
			for (int i = 0; i < numColliderIndexes; i++)
			{
				m_ColliderIndex.Add(reader.ReadInt32());
			}

			m_Scale = reader.ReadSingle();
			m_ArmTwist = reader.ReadSingle();
			m_ForeArmTwist = reader.ReadSingle();
			m_UpperLegTwist = reader.ReadSingle();
			m_LegTwist = reader.ReadSingle();
			m_ArmStretch = reader.ReadSingle();
			m_LegStretch = reader.ReadSingle();
			m_FeetSpacing = reader.ReadSingle();
			m_HasLeftHand = reader.ReadBoolean();
			m_HasRightHand = reader.ReadBoolean();
			reader.ReadBytes(2);
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			m_RootX.WriteTo(stream);
			m_Skeleton.WriteTo(stream);
			m_SkeletonPose.WriteTo(stream);
			m_LeftHand.WriteTo(stream);
			m_RightHand.WriteTo(stream);

			writer.Write(m_Handles.Count);
			for (int i = 0; i < m_Handles.Count; i++)
			{
				m_Handles[i].WriteTo(stream);
			}

			writer.Write(m_ColliderArray.Count);
			for (int i = 0; i < m_ColliderArray.Count; i++)
			{
				m_ColliderArray[i].WriteTo(stream);
			}

			writer.Write(m_HumanBoneIndex.Count);
			for (int i = 0; i < m_HumanBoneIndex.Count; i++)
			{
				writer.Write(m_HumanBoneIndex[i]);
			}

			writer.Write(m_HumanBoneMass.Count);
			for (int i = 0; i < m_HumanBoneMass.Count; i++)
			{
				writer.Write(m_HumanBoneMass[i]);
			}

			writer.Write(m_ColliderIndex.Count);
			for (int i = 0; i < m_ColliderIndex.Count; i++)
			{
				writer.Write(m_ColliderIndex[i]);
			}

			writer.Write(m_Scale);
			writer.Write(m_ArmTwist);
			writer.Write(m_ForeArmTwist);
			writer.Write(m_UpperLegTwist);
			writer.Write(m_LegTwist);
			writer.Write(m_ArmStretch);
			writer.Write(m_LegStretch);
			writer.Write(m_FeetSpacing);
			writer.Write(m_HasLeftHand);
			writer.Write(m_HasRightHand);
			writer.Write(new byte[2]);
		}
	}

	public class AvatarConstant : IObjInfo
	{
		public Skeleton m_AvatarSkeleton { get; set; }
		public SkeletonPose m_AvatarSkeletonPose { get; set; }
		public SkeletonPose m_DefaultPose { get; set; }
		public List<uint> m_SkeletonNameIDArray { get; set; }
		public Human m_Human { get; set; }
		public List<int> m_HumanSkeletonIndexArray { get; set; }
		public List<int> m_HumanSkeletonReverseIndexArray { get; set; }
		public int m_RootMotionBoneIndex { get; set; }
		public xform m_RootMotionBoneX { get; set; }
		public Skeleton m_RootMotionSkeleton { get; set; }
		public SkeletonPose m_RootMotionSkeletonPose { get; set; }
		public List<int> m_RootMotionSkeletonIndexArray { get; set; }

		public AvatarConstant(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			m_AvatarSkeleton = new Skeleton(stream);
			m_AvatarSkeletonPose = new SkeletonPose(stream);
			m_DefaultPose = new SkeletonPose(stream);

			int numIDs = reader.ReadInt32();
			m_SkeletonNameIDArray = new List<uint>(numIDs);
			for (int i = 0; i < numIDs; i++)
			{
				m_SkeletonNameIDArray.Add(reader.ReadUInt32());
			}

			m_Human = new Human(stream);

			int numIndexes = reader.ReadInt32();
			m_HumanSkeletonIndexArray = new List<int>(numIndexes);
			for (int i = 0; i < numIndexes; i++)
			{
				m_HumanSkeletonIndexArray.Add(reader.ReadInt32());
			}

			int numReverseIndexes = reader.ReadInt32();
			m_HumanSkeletonReverseIndexArray = new List<int>(numReverseIndexes);
			for (int i = 0; i < numReverseIndexes; i++)
			{
				m_HumanSkeletonReverseIndexArray.Add(reader.ReadInt32());
			}

			m_RootMotionBoneIndex = reader.ReadInt32();
			m_RootMotionBoneX = new xform(stream);
			m_RootMotionSkeleton = new Skeleton(stream);
			m_RootMotionSkeletonPose = new SkeletonPose(stream);

			int numMotionIndexes = reader.ReadInt32();
			m_RootMotionSkeletonIndexArray = new List<int>(numMotionIndexes);
			for (int i = 0; i < numMotionIndexes; i++)
			{
				m_RootMotionSkeletonIndexArray.Add(reader.ReadInt32());
			}
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			m_AvatarSkeleton.WriteTo(stream);
			m_AvatarSkeletonPose.WriteTo(stream);
			m_DefaultPose.WriteTo(stream);

			writer.Write(m_SkeletonNameIDArray.Count);
			for (int i = 0; i < m_SkeletonNameIDArray.Count; i++)
			{
				writer.Write(m_SkeletonNameIDArray[i]);
			}

			m_Human.WriteTo(stream);

			writer.Write(m_HumanSkeletonIndexArray.Count);
			for (int i = 0; i < m_HumanSkeletonIndexArray.Count; i++)
			{
				writer.Write(m_HumanSkeletonIndexArray[i]);
			}

			writer.Write(m_HumanSkeletonReverseIndexArray.Count);
			for (int i = 0; i < m_HumanSkeletonReverseIndexArray.Count; i++)
			{
				writer.Write(m_HumanSkeletonReverseIndexArray[i]);
			}

			writer.Write(m_RootMotionBoneIndex);
			m_RootMotionBoneX.WriteTo(stream);
			m_RootMotionSkeleton.WriteTo(stream);
			m_RootMotionSkeletonPose.WriteTo(stream);

			writer.Write(m_RootMotionSkeletonIndexArray.Count);
			for (int i = 0; i < m_RootMotionSkeletonIndexArray.Count; i++)
			{
				writer.Write(m_RootMotionSkeletonIndexArray[i]);
			}
		}
	}

	public class Avatar : Component
	{
		public AssetCabinet file { get; set; }
		public int pathID { get; set; }
		public UnityClassID classID1 { get; set; }
		public UnityClassID classID2 { get; set; }

		public string m_Name { get; set; }
		public uint m_AvatarSize { get; set; }
		public AvatarConstant m_Avatar { get; set; }
		public List<KeyValuePair<uint, string>> m_TOS { get; set; }

		public Avatar(AssetCabinet file, int pathID, UnityClassID classID1, UnityClassID classID2)
		{
			this.file = file;
			this.pathID = pathID;
			this.classID1 = classID1;
			this.classID2 = classID2;
		}

		public Avatar(AssetCabinet file) :
			this(file, 0, UnityClassID.Avatar, UnityClassID.Avatar)
		{
			file.ReplaceSubfile(-1, this, null);
		}

		public void LoadFrom(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			m_Name = reader.ReadNameA4();
			m_AvatarSize = reader.ReadUInt32();
			m_Avatar = new AvatarConstant(stream);

			int numTOS = reader.ReadInt32();
			m_TOS = new List<KeyValuePair<uint, string>>(numTOS);
			for (int i = 0; i < numTOS; i++)
			{
				m_TOS.Add(new KeyValuePair<uint, string>(reader.ReadUInt32(), reader.ReadNameA4()));
			}
		}

		public static string LoadName(Stream stream)
		{
			try
			{
				BinaryReader reader = new BinaryReader(stream);
				return reader.ReadNameA4();
			}
			catch
			{
				return null;
			}
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			writer.WriteNameA4(m_Name);
			writer.Write(m_AvatarSize);
			m_Avatar.WriteTo(stream);

			writer.Write(m_TOS.Count);
			for (int i = 0; i < m_TOS.Count; i++)
			{
				writer.Write(m_TOS[i].Key);
				writer.WriteNameA4(m_TOS[i].Value);
			}
		}

		public Avatar Clone(AssetCabinet file)
		{
			Component avatar = file.Components.Find
			(
				delegate (Component asset)
				{
					return asset.classID1 == UnityClassID.Avatar &&
						(asset is NotLoaded ? ((NotLoaded)asset).Name : ((Avatar)asset).m_Name) == m_Name;
				}
			);
			if (avatar == null)
			{
				file.MergeTypeDefinition(this.file, UnityClassID.Avatar);

				Avatar dest = new Avatar(file);
				using (MemoryStream mem = new MemoryStream())
				{
					this.WriteTo(mem);
					mem.Position = 0;
					dest.LoadFrom(mem);
				}
				return dest;
			}
			else if (avatar is NotLoaded)
			{
				NotLoaded notLoaded = (NotLoaded)avatar;
				avatar = file.LoadComponent(file.SourceStream, notLoaded);
			}
			return (Avatar)avatar;
		}

		public string FindBoneName(uint hash)
		{
			foreach (var pair in m_TOS)
			{
				if (pair.Key == hash)
				{
					return pair.Value.Substring(pair.Value.LastIndexOf('/') + 1);
				}
			}
			return null;
		}

		public string BonePath(string boneName)
		{
			return m_TOS.Find
			(
				delegate(KeyValuePair<uint, string> data)
				{
					return data.Value.Substring(data.Value.LastIndexOf('/') + 1) == boneName;
				}
			).Value;
		}

		public uint BoneHash(string boneName)
		{
			return m_TOS.Find
			(
				delegate(KeyValuePair<uint, string> data)
				{
					return data.Value.Substring(data.Value.LastIndexOf('/') + 1) == boneName;
				}
			).Key;
		}

		public void AddBone(Transform parent, Transform bone)
		{
			string parentPath = BonePath(parent.m_GameObject.instance.m_Name);
			string bonePath = parentPath + "/" + bone.m_GameObject.instance.m_Name;
			uint bonePathHash = Animator.StringToHash(bonePath);
			uint boneHash = Animator.StringToHash(bone.m_GameObject.instance.m_Name);

			int idx;
			for (idx = 0; idx < m_TOS.Count; idx++)
			{
				var data = m_TOS[idx];
				if (data.Key > bonePathHash)
				{
					break;
				}
			}
			m_TOS.Insert(idx, new KeyValuePair<uint, string>(bonePathHash, bonePath));

			uint parentHash = BoneHash(parent.m_GameObject.instance.m_Name);
			for (int i = 0; i < m_Avatar.m_AvatarSkeleton.m_ID.Count; i++)
			{
				if (m_Avatar.m_AvatarSkeleton.m_ID[i] == parentHash)
				{
					int cIdx = i + BoneIndex(parent, bone);
					m_Avatar.m_AvatarSkeleton.m_Node.Insert(cIdx, new Node(i, -1));
					m_Avatar.m_AvatarSkeleton.m_ID.Insert(cIdx, bonePathHash);
					m_Avatar.m_SkeletonNameIDArray.Insert(cIdx, boneHash);
					Matrix m = Matrix.Transpose(Matrix.Invert(Transform.WorldTransform(bone)));
					Vector3 t, s;
					Quaternion q;
					m.Decompose(out s, out q, out t);
					xform boneXform = new xform(new Vector4(t, 0), q, new Vector4(s, 1));
					m_Avatar.m_AvatarSkeletonPose.m_X.Insert(cIdx, boneXform);
					m_Avatar.m_DefaultPose.m_X.Insert(cIdx, boneXform);

					for (int j = cIdx + 1; j < m_Avatar.m_AvatarSkeleton.m_ID.Count; j++)
					{
						if (m_Avatar.m_AvatarSkeleton.m_Node[j].m_ParentId >= cIdx)
						{
							m_Avatar.m_AvatarSkeleton.m_Node[j].m_ParentId++;
						}
					}
					break;
				}
			}
			if (m_TOS.Count != m_Avatar.m_AvatarSkeleton.m_Node.Count)
			{
				Report.ReportLog("Warning! Parent Transform " + parent.m_GameObject.instance.m_Name + " not found in Avatar member m_ID");
			}
		}

		int BoneIndex(Transform parent, Transform bone)
		{
			int index = 1;
			for (int i = 0; i < parent.Count; i++)
			{
				if (parent[i] == bone)
				{
					return index;
				}
				index += BoneIndex(parent[i], null);
			}
			return index;
		}

		public SortedDictionary<uint, uint> RenameBone(string oldName, string newName)
		{
			SortedDictionary<uint, uint> boneHashTranslation = new SortedDictionary<uint, uint>();
			for (int i = 0; i < m_TOS.Count; i++)
			{
				var pair = m_TOS[i];
				int beginPos = pair.Value.IndexOf(oldName);
				if (beginPos >= 0)
				{
					string begin = pair.Value.Substring(0, beginPos);
					string end = pair.Value.Substring(beginPos + oldName.Length);
					if ((beginPos == 0 || beginPos > 0 && begin[begin.Length - 1] == '/') && (end.Length == 0 || end[0] == '/'))
					{
						string path = begin + newName + end;
						uint hash = Animator.StringToHash(path);
						boneHashTranslation.Add(m_TOS[i].Key, hash);
						var newPair = new KeyValuePair<uint, string>(hash, path);
						m_TOS[i] = newPair;
					}
				}
			}
			m_TOS.Sort
			(
				delegate(KeyValuePair<uint, string> x, KeyValuePair<uint, string> y)
				{
					return x.Key > y.Key ? 1 : x.Key < y.Key ? -1 : 0;
				}
			);
			return boneHashTranslation;
		}

		public void RemoveBone(string name)
		{
			string bonePath = "/" + name;
			for (int i = 0; i < m_TOS.Count; i++)
			{
				var pair = m_TOS[i];
				int beginPos = pair.Value.IndexOf(bonePath);
				if (beginPos >= 0)
				{
					if (pair.Value.Length == beginPos + bonePath.Length || pair.Value[beginPos + bonePath.Length] == '/')
					{
						m_TOS.RemoveAt(i);
						for (int j = 0; j < m_Avatar.m_AvatarSkeleton.m_ID.Count; j++)
						{
							if (m_Avatar.m_AvatarSkeleton.m_ID[j] == pair.Key)
							{
								m_Avatar.m_AvatarSkeleton.m_ID.RemoveAt(j);
								m_Avatar.m_AvatarSkeleton.m_Node.RemoveAt(j);
								m_Avatar.m_SkeletonNameIDArray.RemoveAt(j);
								m_Avatar.m_AvatarSkeletonPose.m_X.RemoveAt(j);
								m_Avatar.m_DefaultPose.m_X.RemoveAt(j);

								for (int k = j; k < m_Avatar.m_AvatarSkeleton.m_ID.Count; k++)
								{
									if (m_Avatar.m_AvatarSkeleton.m_Node[k].m_ParentId >= j)
									{
										m_Avatar.m_AvatarSkeleton.m_Node[k].m_ParentId--;
									}
								}
								break;
							}
						}
					}
				}
			}
		}
	}
}
