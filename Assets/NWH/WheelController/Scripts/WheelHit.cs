using System;
using UnityEngine;

namespace NWH.WheelController3D
{
    [Serializable]
    /// <summary>
    ///     Represents single ground ray hit.
    /// </summary>
    public struct WheelHit
    {
        /// <summary>
        /// Collider that was hit. If no hit, null.
        /// </summary>
        [Tooltip("Collider that was hit. If no hit, null.")]
        public Collider collider;

        /// <summary>
        ///     The normal at the point of contact
        /// </summary>
        [Tooltip("The normal at the point of contact")]
        public Vector3 normal;

        /// <summary>
        ///     The point of contact between the wheel and the ground.
        /// </summary>
        [Tooltip("The point of contact between the wheel and the ground.")]
        public Vector3 point;
    }
}