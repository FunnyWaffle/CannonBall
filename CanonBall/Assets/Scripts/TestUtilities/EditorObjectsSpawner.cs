using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.TestUtilities
{
#if UNITY_EDITOR
    [Serializable]
    public class EditorObjectsSpawner
    {
        [SerializeField] private string _name;
        [SerializeField] private Texture2D _texture;

        private GameObject[] _visuals;

        public void Spawn(IReadOnlyList<Vector3> positions)
        {
            var spawnData = CreateData(positions, _name, _texture);

            var objects = new GameObject[spawnData.Length];

            for (int i = 0; i < spawnData.Length; i++)
            {
                var data = spawnData[i];
                var go = Spawn(data.Name, data.Position, data.Texture);

                objects[i] = go;
            }

            _visuals = objects;
        }

        public GameObject Spawn(string name, Vector3 position, Texture2D texture)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            EditorGUIUtility.SetIconForObject(go, texture);

            return go;
        }

        public void Clear()
        {
            foreach (var obj in _visuals)
            {
                GameObject.Destroy(obj);
            }
        }

        private SpawnData[] CreateData(IReadOnlyList<Vector3> positions, string name, Texture2D texture)
        {
            var data = new SpawnData[positions.Count];

            for (int i = 0; i < positions.Count; i++)
            {
                data[i] = new SpawnData
                {
                    Name = $"{name} {i}",
                    Position = positions[i],
                    Texture = texture
                };
            }

            return data;
        }
    }

    public class SpawnData
    {
        public string Name;
        public Vector3 Position;
        public Texture2D Texture;
    }
#endif
}