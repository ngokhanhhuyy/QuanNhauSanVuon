export type MenuItemListSortedByFields =
  | "name"
  | "defaultAmount"
  | "defaultVatPercentage"
  | "createdDateTime";

export type MenuItemListRequestDto = {
  sortedByField: MenuItemListSortedByFields;
  sortedByAscending: boolean;
  page: number;
  resultsPerPage: number;
};

export type MenuItemUpsertRequestDto = {
  name: string;
  defaultAmount: number;
  defaultVatPercentage: number;
  unit: string;
  thumbnailUrl: string | null;
  categoryId: number | null;
}