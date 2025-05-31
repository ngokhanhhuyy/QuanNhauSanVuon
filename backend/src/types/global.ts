declare global {
  type SortedByConditions<TFieldName extends string | number> = {
    [FieldName in TFieldName]?: "asc" | "desc";
  }
}

export {  };