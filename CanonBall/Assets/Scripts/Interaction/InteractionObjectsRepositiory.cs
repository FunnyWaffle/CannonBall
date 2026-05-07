using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Input;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interaction
{
    public class InteractionObjectsRepositiory
    {
        private readonly Dictionary<Collider, IController> _controllers = new();
        private readonly Dictionary<Collider, InteractionableTypes> _interactionables = new();
        private readonly Dictionary<InteractionableTypes, IUIWindow> _uis = new();

        public void AddControllers(Collider collider, IController controller)
        {
            _controllers[collider] = controller;
        }

        public bool TryGetControllers(Collider collider, out IController controller)
        {
            return _controllers.TryGetValue(collider, out controller);
        }

        public void AddUIWindow(InteractionableTypes interactionableType, IUIWindow uIWindow)
        {
            _uis[interactionableType] = uIWindow;
        }

        public void AddInteactionable(Collider collider, InteractionableTypes interactionableType)
        {
            _interactionables[collider] = interactionableType;
        }

        public bool TryGetUIWindow(Collider collider, out IUIWindow uIWindow)
        {
            if (_interactionables.TryGetValue(collider, out var interactionableType)
                && _uis.TryGetValue(interactionableType, out uIWindow))
                return true;

            uIWindow = default;
            return false;
        }
    }

    public enum InteractionableTypes
    {
        Vendor
    }
}
