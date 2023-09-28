using UnityEngine;
using Rewired;

namespace NWH.Common.Input
{
    /// <summary>
    ///     Class for handling scene input through Rewired.
    /// </summary>
    public class RewiredSceneInputProvider : SceneInputProviderBase
    {
        private Rewired.Player player;

        private bool _rotationModifier;
        private bool _panningModifier;


        public override void Awake()
        {
            base.Awake();

            player = Rewired.ReInput.players.GetPlayer(0);
        }

        public override bool ChangeCamera()
        {
            return player.GetButtonDown("ChangeCamera");
        }


        public override Vector2 CameraRotation()
        {
            return player.GetAxis2D("CameraRotationX", "CameraRotationY");
        }


        public override Vector2 CameraPanning()
        {
            return player.GetAxis2D("CameraPanningX", "CameraPanningY");
        }


        public override bool CameraRotationModifier()
        {
            return player.GetButton("CameraRotationModifier") || !requireCameraRotationModifier;
        }


        public override bool CameraPanningModifier()
        {
            return player.GetButton("CameraPanningModifier") || !requireCameraPanningModifier;
        }


        public override float CameraZoom()
        {
            return player.GetAxis("CameraZoom");
        }


        public override bool ChangeVehicle()
        {
            return player.GetButtonDown("ChangeVehicle");
        }


        public override Vector2 CharacterMovement()
        {
            return player.GetAxis2D("FPSMovementX", "FPSMovementY");
        }


        public override bool ToggleGUI()
        {
            return player.GetButtonDown("ToggleGUI");
        }
    }
}