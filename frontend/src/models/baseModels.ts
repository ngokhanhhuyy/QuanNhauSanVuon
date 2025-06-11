declare global {
  interface IClonableModel<TModel extends object> {
    mutate(updatedData: Partial<TModel>): TModel;
  }

  type ModelPropertyState<T> = Readonly<{
    get(): T;
    set: ModelPropertySetter<T>;
  }>;

  type ModelPropertySetter<T> = React.Dispatch<React.SetStateAction<T>>;

  type FunctionPropertyNames<TModel> = {
    [TKey in keyof TModel]: TModel[TKey] extends Function ? TKey : never;
  }[keyof TModel];

  type MethodsOmitted<TModel> = Omit<TModel, FunctionPropertyNames<TModel>>;

  type Writable<TModel extends object> = {
    -readonly [TKey in keyof TModel]: TModel[TKey];
  };
}

export function createBaseModel<TModel extends object>(): IClonableModel<TModel> {
  return {
    mutate(updatedData: Partial<TModel>): TModel {
      return {
        ...(this as TModel),
        ...updatedData,
      };
    },
  };
}
