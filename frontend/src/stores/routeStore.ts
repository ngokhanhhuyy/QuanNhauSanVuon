import { create } from "zustand";
import { routes, type Route } from "@/router/routeDefinitions";
import { useRouteUtils } from "@/utils/routeUtils";

// Utils.
const routeUtils = useRouteUtils();

// Type.
type RouteSelector = (routeUtils: ReturnType<typeof useRouteUtils>) => string;
type RouteSettingOptions = {
  replace: boolean;
};

interface IRouteStore {
  route: Route;
  routePath: string;
  routeName: keyof typeof routes;
  params: Record<string, string>;
  searchParams: URLSearchParams;
  setRoutePath(routeSelector: RouteSelector, options?: RouteSettingOptions): void;
  setRoutePath(routePath: string, options?: RouteSettingOptions): void;
}

const useRouteStore = create<IRouteStore>((set) => ({
  routePath: document.location.href,
  route: routes.home,
  routeName: "home",
  params: { },
  searchParams: new URLSearchParams(),
  setRoutePath(arg: RouteSelector | string, options?: RouteSettingOptions): void {
    let url: URL;
    if (typeof arg === "string") {
      url = new URL(arg, window.location.origin);
    } else {
      url = new URL(arg(routeUtils), window.location.origin);
    }

    const routePath = url.pathname;
    let matchedRoute: Route = routes.home;
    let routeName: keyof typeof routes = "home";
    let params = { };
    let searchParams = new URLSearchParams();
    for (const [name, route] of Object.entries(routes)) {
      if (typeof route.path === "string") {
        if (route.path === routePath) {
          matchedRoute = route;
          routeName = name as keyof typeof routes;
          params = { };
          break;
        }
      }

      const match = routePath.match(route.path);
      if (match) {
        matchedRoute = route;
        routeName = name as keyof typeof routes;
        params = match?.groups ?? { };
        break;
      }

      const url = new URL(routePath, window.location.origin);
      searchParams = url.searchParams;
    }

    if (options?.replace) {
      window.history.replaceState(null, matchedRoute.pageTitle, routePath);
      return;
    }

    window.history.pushState(null, matchedRoute.pageTitle, routePath);

    set({
      routePath,
      route: matchedRoute,
      routeName,
      params,
      searchParams: searchParams
    });
  },
}));

export { useRouteStore };