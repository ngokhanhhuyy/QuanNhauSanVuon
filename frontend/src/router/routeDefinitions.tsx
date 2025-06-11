import React, { lazy } from "react";

// Page components.
const HomePage = lazy(() => import("@/pages/home"));
const SignInPage = lazy(() => import("@/pages/signIn"));

export type RouteDefinitions<TParams extends object | undefined = undefined> = {
  [routeName: string]: Route<TParams>;
};

export type Route<TParams extends object | undefined = undefined> = {
  path: string | RegExp;
  component: (params: Record<string, string>) => React.ReactElement;
  pageTitle: string;
  breadcrumbItems: BreadcrumbItem[];
  generate: (params: TParams) => string;
};

export type BreadcrumbItem = {
  name: string;
  path: string | null;
};

export const routes = {
  home: {
    path: "/",
    component: () => <HomePage />,
    pageTitle: "Đăng nhập",
    breadcrumbItems: [],
    generate: () => "/"
  },
  signIn: {
    path: "/signIn",
    component: () => <SignInPage />,
    pageTitle: "Đăng nhập",
    breadcrumbItems: [],
    generate: () => "/signIn"
  },

} satisfies RouteDefinitions;