import { LoginForm } from "@/components/auth/login-form"

export default function SignInPage() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-background px-4">
      <div className="w-full max-w-md">
        <LoginForm />
      </div>
    </div>
  )
}

export const metadata = {
  title: "Iniciar Sesión | DevTalles Blog",
  description: "Inicia sesión en DevTalles Blog",
}
