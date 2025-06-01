declare global {
  type SortedByConditions<TFieldName extends string | number> = {
    [fieldName in TFieldName]?: "asc" | "desc";
  }
}

declare namespace NodeJS {
  interface ProcessEnv {
    NODE_ENV: 'development' | 'production' | 'test';
    PORT: string;
    DATABASE_URL: string;
    JWT_SECRET: string;
    BASE_URL: string;
  }
}


export {  };