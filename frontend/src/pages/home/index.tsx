import { useEffect, startTransition } from "react";
import { useNavigate } from "react-router";
import { useAuthenticationService } from "@/services/authenticationService";
import { useRouteUtils } from "@/utils/routeUtils";

// Services and utils.
const authenticationService = useAuthenticationService();
const routeUtils = useRouteUtils();

const colors = ["blue", "red", "purple", "green", "yellow", "black"];

// Component.
export default function HomePage() {
  // Dependencies.
  const navigate = useNavigate();

  // Effect.
  useEffect(() => {
    const checkAuthenticationAsync = async () => {
      const isAuthenticated = await authenticationService.checkAuthenticationStatusAsync();
      if (!isAuthenticated) {
        startTransition(() => {
          navigate(routeUtils.getSignInRoutePath());
        });
      }
    };

    checkAuthenticationAsync();
  }, []);

  return (
    <div className="content flex justify-center align-center">
      <h1>Rsbuild with React</h1>
      <p>Start building amazing things with Rsbuild.</p>
      {colors.map(color => (
        <button className={`btn btn-${color} mt-3`} key={color}>Click</button>
      ))}
      
      {colors.map(color => (
        <button className={`btn btn-${color}-outline mt-3`} key={color}>Click</button>
      ))}

      {colors.map(color => (
        <button className={`btn btn-${color}-outline btn-sm mt-3`} key={color}>Click</button>
      ))}

      {colors.map(color => (
        <button className={`btn btn-${color}-outline btn-lg mt-3`} key={color}>Click</button>
      ))}
    </div>
  );
}