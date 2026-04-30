using Assets.Scripts.Camera;
using Assets.Scripts.Creations.Player.Components;
using R3;
using System;
using UnityEngine;

namespace Assets.Scripts.Creations.Player
{
    public class PlayerAvatarCore
    {
        private readonly Aimer _aimer;
        private readonly PlayerAvatarMover _mover;

        public PlayerAvatarCore(Aimer aimer, PlayerAvatarMover mover)
        {
            _aimer = aimer;
            _mover = mover;
            CurrentViewType.Value = CameraViewType.FirstPerson;
        }

        public PlayerAvatarMover Mover => _mover;

        public ReactiveProperty<CameraViewType> CurrentViewType = new();

        public void Move(Vector2 input)
        {
            _mover.UpdateVelocity(input);
        }

        public void Aim(Vector2 input)
        {
            _aimer.Aim(input);
        }

        public void SetViewMode(int viewModeIndex)
        {
            CurrentViewType.Value = viewModeIndex switch
            {
                0 => CameraViewType.FirstPerson,
                1 => CameraViewType.ThirdPerson,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
