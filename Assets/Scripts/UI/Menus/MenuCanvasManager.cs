using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Solace {
  public class MenuCanvasManager : MonoBehaviour {
    public Button backButton;
    public MenuType activeMenuType = MenuType.MAIN_MENU;
    public static MenuCanvasManager instance;
    private MenuController lastActiveMenu;
    private Stack<MenuType> history;

    private List<MenuController> menuList;

    public void SwitchMenu(MenuType menuType, bool pushToStack = true) {
      MenuController desiredMenu = menuList.Find(menu => menu.menuType == menuType);
      if (lastActiveMenu != null) {
        lastActiveMenu.gameObject.SetActive(false);
      }
      if (desiredMenu != null) {
        desiredMenu.gameObject.SetActive(true);
        lastActiveMenu = desiredMenu;
        activeMenuType = desiredMenu.menuType;
        if (pushToStack) {
          history.Push(desiredMenu.menuType);
        }
        ToggleBackButton(desiredMenu.menuType != MenuType.MAIN_MENU);
      } else {
        Debug.LogWarning($"The desired menu is not found of menu type {Enum.GetName(typeof(MenuType), menuType)}");
      }
    }

    private void OnBackButtonClick() {
      if (history.TryPeek(out var _)) {
        history.Pop();
        if (history.TryPeek(out var result)) {
          SwitchMenu(result, false);
        }
      }
    }

    private void OnEnable() {
      backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnDisable() {
      backButton.onClick.RemoveAllListeners();
    }

    private void ToggleBackButton(bool active) {
      backButton.gameObject.SetActive(active);
    }

    private void Awake() {
      menuList = GetComponentsInChildren<MenuController>(true).ToList();
      menuList.ForEach(menu => menu.gameObject.SetActive(false));
      ToggleBackButton(false);
      history = new();
      SwitchMenu(activeMenuType);

      if (instance == null) {
        instance = this;
      } else {
        Destroy(gameObject);
      }
    }
  }
}
