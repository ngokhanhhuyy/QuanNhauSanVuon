declare global {
  type OrderBasicDto = {
    id: number;
    netAmount: number;
    vatAmount: number;
    createdDateTime: string;
    paidDateTime: string | null;
    createdUser: UserDto;
  };

  type OrderDetailDto = {
    lastUpdatedDateTime: string | number;
    lastUpdatedUser: UserDto | null;
    items: OrderItemDto[];
  } & OrderBasicDto;

  type OrderListDto = {
    pageCount: number;
    items: OrderBasicDto[];
  };

  type OrderListFiltersDto = Partial<{
    sortedByAscending: boolean;
    sortedByField: string;
    page: number;
    resultsPerPage: number;
    seatingId: number | null;
    createdMonthYear: MonthYearDto;
    paidMonthYear: MonthYearDto;
  }>;
}

export { };