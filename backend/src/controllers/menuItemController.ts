import type { IMenuItemService } from "@shared/services/menuItemService";
import { MenuItemService } from "@/services/menuItemService";

export class MenuItemController {
  private readonly _service: IMenuItemService;

  constructor() {
    this._service = new MenuItemService();
  }

  public list()
}