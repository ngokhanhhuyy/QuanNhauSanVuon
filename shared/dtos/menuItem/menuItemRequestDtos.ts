export type MenuItemListSortedByFields =
  | "name"
  | "defaultAmount"
  | "defaultVatPercentage"
  | "createdDateTime";

export type MenuItemListRequestDto = Partial<{
  sortedByField: MenuItemListSortedByFields;
  sortedByAscending: boolean;
  page: number;
  resultsPerPage: number;
}>;