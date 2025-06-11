import { useState, useMemo } from "react";
import { useAuthenticationService } from "@/services/authenticationService";
import { useModelFactory } from "@/models";
import { useHTMLUtils } from "@/utils/htmlUtils";

// Dependencies.
const authenticationService = useAuthenticationService();
const modelFactory = useModelFactory();
const { compute } = useHTMLUtils();

export default function SignInPage() {
  // States.
  const model = useState(() => modelFactory.signIn.create());

  // Template.
  return (
    <main className="flex bg-gray-100 w-full h-full justify-center items-center">
      <div className="flex flex-col justify-start items-stretch p-3">
        
      </div>
    </main>
  );
}