const routeUtils = {
  getHomeRoutePath(): string {
    return "/";
  },
  getSignInRoutePath(): string {
    return "/signIn";
  },
}

export function useRouteUtils() {
  return routeUtils;
}