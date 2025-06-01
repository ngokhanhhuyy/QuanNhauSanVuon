import { ValidationError } from "@shared/errors";
import type {
  IMenuItemListRequestDto,
  IMenuItemListResponseDto,
  IMenuItemDetailResponseDto,
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
  getListAsync(requestDto?: IMenuItemListRequestDto): Promise<IMenuItemListResponseDto>;

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
  getDetailAsync(id: number): Promise<IMenuItemDetailResponseDto>;

  /**
   * Creates a new menu item.
   * 
   * @param requestDto A DTO containing the data for the creating operation.
   * @returns A {@link Promise} representing the asynchronous operation, which result is the id
   * of the created menu item.
   * 
   * @throws {ValidationError} Throws when the data specified by {@link requestDto} is invalid.
   * @throws {OperationError} Throws when the category with the id specified by the property
   * `categoryId` in the {@link requestDto} doesn't exist.
   */
  createAsync(requestDto: IMenuItemListRequestDto): Promise<number>;
  
  /**
   * Updates an existing menu item.
   * 
   * @param id The id of the menu item to update.
   * @param requestDto A DTO containing the data for the updating operation.
   * @returns A {@link Promise} representing the asynchronous operation.
   * 
   * @throws {ValidationError} Throws when the data specified by {@link requestDto} is invalid.
   * @throws {NotFoundError} Throws when the menu item specified by {@link id} doesn't exist.
   * @throws {OperationError} Throws when the category with the id specified by the property
   * `categoryId` in the {@link requestDto} doesn't exist.
   */
  updateAsync(id: number, requestDto: IMenuItemListRequestDto): Promise<void>;

  /**
   * Deletes an existing menu item.
   * 
   * @param id The id of the menu item to delete.
   * @returns A {@link Promise} representing the asynchronous operation.
   * 
   * @throws {NotFoundError} Throws when the menu item specified by {@link id} doesn't exist.
   */
  deleteAsync(id: number): Promise<void>;
}