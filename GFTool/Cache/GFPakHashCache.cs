﻿using GFTool.Math.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFTool.Cache
{
    public class GFPakHashCache
    {
        private const string CachePath = "GFPAKHashCache.bin";
        private static Dictionary<ulong, string> Cache = new Dictionary<ulong, string>();

        public static void Init(string path = CachePath)
        {
            Cache = new Dictionary<ulong, string>();
            if (File.Exists(path)) {
                BinaryReader br = new BinaryReader(File.OpenRead(path));
                var version = br.ReadUInt64();
                var count = br.ReadUInt32();
                for (int i = 0; i < count; i++)
                {
                    var hash = br.ReadUInt64();
                    var name = br.ReadString();
                    Cache.Add(hash, name);
                }
            }
        }

        public static void Write(string path = CachePath)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(path));
            bw.Write(GFFNV.Hash(""));
            bw.Write((uint)Cache.Count);
            foreach(KeyValuePair<ulong, string> pair in Cache)
            {
                bw.Write(pair.Key);
                bw.Write(pair.Value);
            }
        }

        public static void AddHashName(UInt64 hash, string name)
        {
            UInt64 hashCheck = GFFNV.Hash(name);
            if (hashCheck == hash)
            {
                Cache[hash] = name;
            }
        }

        public static void AddHash(string name)
        {
            UInt64 hash = GFFNV.Hash(name);
            Cache[hash] = name;
        }

        public static string? GetName(UInt64 hash)
        {
            if (Cache.ContainsKey(hash))
            {
                return Cache[hash];
            }
            return null;
        }

    }
}
