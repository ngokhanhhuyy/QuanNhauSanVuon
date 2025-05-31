import type {
  MenuItemListRequestDto,
  MenuItemListResponseDto,
  MenuItemDetailResponseDto,
} from "../dtos";

export interface IMenuItemService {
  /**
   * Gets a list of menu items with the specified sorting sorting paginating conditions.
   * 
   * @param requestDto (Optional) A DTO containing the conditions for the results.
   * @return A {@link Promise} representing the asynchronous operation, which result is a DTO
   * containing the menu items and other information for pagination.
   * 
   * @throw {ValidationError} Throws when the conditions specified in the {@link requestDto} 
   * argument is invalid.
   */
  getListAsync(requestDto?: MenuItemListRequestDto): Promise<MenuItemListResponseDto>;

  /**
   * Gets an existing menu item, specified by its id.
   * 
   * @param id The id of the menu item to retrieve.
   * @returns A {@link Promise} representing the asynchronous operation, which result is a DTO
   * containing the information of the menu item.
   * 
   * @throws {NotFoundError} Throws when the menu item with the specified {@link id} doesn't
   * exist.
   */
  getDetailAsync(id: number): Promise<MenuItemDetailResponseDto>;
}