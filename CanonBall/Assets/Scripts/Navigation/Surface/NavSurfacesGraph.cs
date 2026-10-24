using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Surface
{
    public class NavSurfacesGraph : MonoBehaviour
    {
        [SerializeField] private List<Connection> _connections;
    }

    [Serializable]
    public class Connection
    {
        [SerializeField] private NavGrid _source;
        [SerializeField] private List<NavGrid> _targets;
    }
}
