import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import api from "../api";
import ErrorNotification from "../components/Error";
type LoginProps = {
  setToken?: (token: string | null) => void;
};

export default function Login({ setToken }: LoginProps) {
  const location = useLocation();
  const message = location.state?.message;
  const from = location.state?.from?.pathname || "/";
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);

  async function handleSubmit(e: React.SubmitEvent) {
      e.preventDefault();
      setError(null);
    try {
      const res = await api.post("auth/login", { email, password });

      localStorage.setItem("token", res.data.token);
      setToken?.(res.data.token);
      console.log("Logged in token:", res.data.token);

      navigate(from);
    } catch (err: any) {
        console.error(err);
        if (err.response) {
            const status = err.response.status;

            if (status === 401) {
                setError(err.response.data.message || "Invalid email or password. Please try again.");
            } else if (status === 400) {
                setError(err.response.data.message || "Invalid input. Please check your data and try again.");
            } else if (status === 500) {
                setError("Server error. Please try again later.");
            } else {
                setError("An unexpected error occurred. Please try again.");
            }
        }
    }
  }

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="w-full max-w-sm bg-white p-8 rounded-xl shadow-md border border-gray-200">
        <h2 className="text-2xl font-semibold mb-6 text-center text-gray-800">
          Sign in
        </h2>{" "}
        {message && (
          <div className="mb-4 px-4 py-2 rounded-md bg-yellow-100 text-yellow-800 border border-yellow-300 text-sm">
            {message}
          </div>
        )}
              <form onSubmit={handleSubmit} className="flex flex-col gap-4">
                  {error && <ErrorNotification message={error} />}
          <input
            type="text"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            className="px-4 py-2 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />

          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            className="px-4 py-2 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />

          <button
            type="submit"
            className="mt-2 py-2 rounded-md bg-blue-600 text-white font-medium hover:bg-blue-700 transition"
          >
            Login
          </button>
        </form>
        <div className="mt-6 text-center text-sm text-gray-600">
          Don’t have an account?{" "}
          <button
            onClick={() => navigate("/register")}
            className="text-blue-600 hover:underline"
          >
            Register
          </button>
        </div>
      </div>
    </div>
  );
}
