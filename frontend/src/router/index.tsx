// import { lazy } from "react";
import { createBrowserRouter, redirect } from "react-router";
import { useAuthenticationService } from "@/services/authenticationService";

// Page components.
import HomePage from "@/pages/home";
import SignInPage from "@/pages/signIn";

// Services.
const authenticationService = useAuthenticationService();

// Route definitions.
const router = createBrowserRouter([
  { path: "/signIn", element: <SignInPage /> },
  {
    path: "/",
    loader: async () => {
      if (!await authenticationService.checkAuthenticationStatusAsync()) {
        return redirect("/signIn");
      }
    },
    children: [
      { index: true, element: <HomePage /> }
    ]
  },
]);

export { router };