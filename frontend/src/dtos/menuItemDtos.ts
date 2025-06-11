declare global {
  type MenuItemListDto = {
    pageCount: number;
    items: MenuItemBasicDto[];
  };

  type MenuItemListFiltersDto = Partial<{
    sortedByAscending: boolean;
    sortedByField: string;
    page: number;
    resultsPerPage: number;
    categoryId: number | null;
  }>;

  type MenuItemBasicDto = {
    id: number;
    name: string;
    defaultNetPrice: number;
    defaultVatPercentage: number;
    unit: string;
    thumbnailUrl: string | null;
  };

  type MenuItemDetailDto = {
    createdDateTime: string;
    lastUpdatedDateTime: string | null;
    category: MenuCategoryDto | null;
  } & MenuItemBasicDto;

  type MenuItemUpsertDto = {
    name: string;
    defaultNetPrice: number;
    defaultVatPercentage: number;
    unit: string;
    thumbnailUrl: string | null;
    categoryId: number | null;
  };
}

export { };