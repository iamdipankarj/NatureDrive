using UnityEngine;

public class ItemController : MonoBehaviour {
  [Header("Item Type")]
  public itemTypes itemType = new itemTypes();

  public enum itemTypes {
    Button,
    Slider,
    HorizontalSelector,
    Toggle
  }
}
