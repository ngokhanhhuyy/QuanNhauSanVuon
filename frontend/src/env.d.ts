/// <reference types="@rsbuild/core/types" />

interface ImportMetaEnv {
  readonly MODE: 'development' | 'production' | 'test';
  readonly DEV: boolean;
  readonly PROD: boolean;
  readonly BASE_URL: string;
  readonly ASSET_PREFIX: string;
  readonly API_URL_DEV: string;
  readonly API_URL_PROD: string;
}