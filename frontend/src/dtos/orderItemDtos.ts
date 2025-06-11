declare global {
  type OrderItemDto = {
    id: number;
    quantity: number;
    netPricePerUnit: number;
    vatAmountPerUnit: number;
    orderedDateTime: string;
    menuItem: MenuItemBasicDto;
  };

  type OrderItemUpsertDto = {
    id: number | null;
    quantity: number;
    netPricePerUnit: number | null;
    vatAmountPerUnit: number | null;
    menuItemId: number;
    hasBeenChanged: boolean;
    hasBeenDeleted: boolean;
  };
}

export { };