using UnityEngine;

namespace LastUI {
  public class ItemController : MonoBehaviour {
    [Header("Item Type")]
    public ItemTypes itemType = new();

    public enum ItemTypes {
      Button,
      Slider,
      HorizontalSelector,
      Toggle
    }
  }
}