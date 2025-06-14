import { defineConfig } from '@rsbuild/core';
import { pluginReact } from '@rsbuild/plugin-react';

export default defineConfig({
  plugins: [pluginReact()],

  performance: {
    buildCache: false,
    chunkSplit: {
    	strategy: "all-in-one"
    }
  },
  resolve: {
    aliasStrategy: "prefer-alias",
    alias: {
      "@": "./src",
    },
  },
  server: {
    host: "0.0.0.0",
    port: 5173,
    historyApiFallback: true,
    publicDir: {
      name: "./src/assets",
    },
    headers: {
      "Allow-Control-Allow-Origin": "*",
      "Allow-Control-Allow-Methods": "GET,POST,PUT,DELETE,OPTIONS",
      "Allow-Control-Allow-Headers": "Content-Type,Authorization",
    },
    proxy: {
      "/api": {
        target: "http://localhost:5000",
        pathRewrite: { "^/api": "/api" },
        changeOrigin: true,
        secure: false,
        ws: true,
      },
    },
  },
  dev: {
    client: {
      host: "frontend-wsl.khanhhuy.dev",
    },
  },
});