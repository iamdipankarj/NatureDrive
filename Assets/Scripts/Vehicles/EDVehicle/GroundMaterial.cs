using UnityEngine;

namespace Solace {
  public class GroundMaterial {
    public PhysicMaterial physicMaterial;

    public float grip = 1.0f;
    public float drag = 0.1f;

    public TireMarksRenderer marksRenderer;
    public TireParticleEmitter particleEmitter;

    // Surface type affects the audio clips and other effects that are invoked
    // depending on the surface. See the VehicleAudio component.
    //
    // Hard: tire skid audio, hard impacts, hard body drag, body scratches
    // Soft: offroad rumble, soft impacts, soft body drag

    public enum SurfaceType { Hard, Soft };
    public SurfaceType surfaceType = SurfaceType.Hard;
  }
}
