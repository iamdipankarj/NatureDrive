using System;
using NWH.Common.Input;
using UnityEngine;
using Rewired;

namespace NWH.VehiclePhysics2.Input
{
    /// <summary>
    ///     Class for handling vehicle input through Rewired.
    /// </summary>
    public class RewiredVehicleInputProvider : VehicleInputProviderBase
    {
        /// <summary>
        ///     Number of H-shifter gears. Apart from changing this value when adding gears, also add/remove the events in the
        ///     Awake() method
        ///     to match the change.
        /// </summary>
        private const int GearCount = 10;

        private Rewired.Player player;

        private readonly bool[] _shiftIntoHeld = new bool[GearCount];
        
        /// <summary>
        ///     Names of input bindings for each individual gears. If you need to add more gears modify this and the corresponding
        ///     iterator in the
        ///     ShiftInto() function.
        /// </summary>
        [NonSerialized]
        [Tooltip(
            "Names of input bindings for each individual gears. If you need to add more gears modify this and the corresponding\r\niterator in the\r\nShiftInto() function.")]
        public string[] shiftInputNames =
        {
            "ShiftIntoR1",
            "ShiftIntoN",
            "ShiftInto1",
            "ShiftInto2",
            "ShiftInto3",
            "ShiftInto4",
            "ShiftInto5",
            "ShiftInto6",
            "ShiftInto7",
            "ShiftInto8",
            "ShiftInto9",
        };

        private float _throttle;
        private float _brakes;
        private float _steering;
        private float _clutch;
        private float _handbrake;


        public override void Awake()
        {
            base.Awake();

            player = Rewired.ReInput.players.GetPlayer(0);
        }


        public void Update()
        {

            _throttle = player.GetAxis("Throttle");
            _brakes = player.GetAxis("Brakes");
            _steering = player.GetAxis("Steering");
            _clutch    = player.GetAxis("Clutch");
            _handbrake = player.GetAxis("Handbrake");
        }

        
        public override float Throttle()
        {
            return _throttle;
        }
        
        
        public override float Brakes()
        {
            return _brakes;
        }


        public override float Steering()
        {
            return _steering;
        }


        public override float Clutch()
        {
            return _clutch;
        }


        public override float Handbrake()
        {
            return _handbrake;
        }


        public override bool EngineStartStop()
        {
            return player.GetButtonDown("EngineStartStop");
        }


        public override bool ExtraLights()
        {
            return player.GetButtonDown("ExtraLights");
        }


        public override bool HighBeamLights()
        {
            return player.GetButtonDown("HighBeamLights");
        }


        public override bool HazardLights()
        {
            return player.GetButtonDown("HazardLights");
        }


        public override bool Horn()
        {
            return player.GetButton("Horn");
        }


        public override bool LeftBlinker()
        {
            return player.GetButtonDown("LeftBlinker");
        }


        public override bool LowBeamLights()
        {
            return player.GetButtonDown("LowBeamLights");
        }


        public override bool RightBlinker()
        {
            return player.GetButtonDown("RightBlinker");
        }


        public override bool ShiftDown()
        {
            return player.GetButtonDown("ShiftDown");
        }


        public override bool ShiftUp()
        {
            return player.GetButtonDown("ShiftUp");
        }


        public override void OnDestroy()
        {
            base.OnDestroy();

            _throttle  = 0;
            _brakes    = 0;
            _steering  = 0;
            _clutch    = 0;
            _handbrake = 0;
        }


        /// <summary>
        ///     Used for H-shifters and direct shifting into gear on non-sequential gearboxes.
        /// </summary>
        public override int ShiftInto()
        {
            for (int i = -1; i < 9; i++)
            {
                if (player.GetButton(shiftInputNames[i + 1]))
                {
                    return i;
                }
            }

            return -999;
        }


        public override bool TrailerAttachDetach()
        {
            return player.GetButtonDown("TrailerAttachDetach");
        }


        public override bool FlipOver()
        {
            return player.GetButtonDown("FlipOver");
        }


        public override bool Boost()
        {
            return player.GetButton("Boost");
        }


        public override bool CruiseControl()
        {
            return player.GetButtonDown("CruiseControl");
        }
    }
}